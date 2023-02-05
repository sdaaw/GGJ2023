using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public bool AllowMovement;
    public float speed;

    public float score;

    private Rigidbody m_rigidbody;
    private Transform m_transform;

    private Quaternion m_oldRotation;
    private float m_horAxis;
    private float m_verAxis;
    private Vector3 m_move;
    private Vector3 m_mousePos;

    [SerializeField]
    private Camera m_playerCamera;

    [SerializeField]
    private LayerMask m_layerMask;

    public Animator animator;

    public AudioSource bgMusic;

    private Animator m_anim;

    private Stats m_stats;

    public Melee mainHand;
    public Melee offHand;

    private int currComboIdx = 0;

    public float comboTimer = 0;
    public bool comboStarted = false;
    private bool attemptEat;

    public AudioSource walkSound;

    private bool isDead = false;

    private SpriteRenderer sr;
    private Vector3 spriteLocalScale;

    private void Awake()
    {
        m_transform = transform;
        m_rigidbody = GetComponent<Rigidbody>();
        m_playerCamera = FindObjectOfType<Camera>();
        m_anim = GetComponentInChildren<Animator>();
        m_stats = GetComponent<Stats>();
        sr = GetComponentInChildren<SpriteRenderer>();
        spriteLocalScale = sr.transform.localScale;
    }

    void FixedUpdate()
    {
        if (AllowMovement && !GameManager.Instance.isGameFinished)
        {
            DoMovement();
            Rotate(m_move);
        }
    }

    private void Start()
    {
        DoMelee();
    }

    private void Update()
    {
        if (isDead)
        {

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
            }


            return;
        }
           

        if (m_transform.position.y < -20)
            GetComponent<Stats>().TakeDmg(10000);
        
        if(m_stats != null && m_stats.isDead)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        if (m_anim != null)
        {
            float speed = Mathf.Abs(m_move.magnitude);
            m_anim.SetFloat("Speed", speed);

            if(walkSound)
            {
                if (speed > 0 && !walkSound.isPlaying)
                    walkSound.Play();
                else if (speed <= 0)
                    walkSound.Pause();
            }

          
        }

        // flip sprite
        if (m_transform.eulerAngles.y > 180)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }


        if (!AllowMovement)
            return;


        if(comboStarted)
        {
            comboTimer += 1 * Time.deltaTime;
            if (comboTimer > .75f)
            {
                comboTimer = 0;
                currComboIdx = 0;
                comboStarted = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoMelee();
        }
    }

    private void DoMelee()
    {
        comboStarted = true;
        if (mainHand.canMelee)
        {

            switch (currComboIdx)
            {
                case 0:
                    if (m_anim != null)
                    {
                        m_anim.SetTrigger("Attack1");
                        m_anim.speed = mainHand.swingTimerMax;
                    }

                    mainHand.Swing();
                    currComboIdx++;
                    comboTimer = 0;
                    SoundManager.PlayASource("Melee1");
                    break;
                case 1:
                    if (m_anim != null)
                    {
                        m_anim.SetTrigger("Attack2");
                        m_anim.speed = mainHand.swingTimerMax;
                    }

                    mainHand.Swing();
                    currComboIdx++;
                    comboTimer = 0;
                    SoundManager.PlayASource("Melee1");
                    break;
                case 2:
                    if (m_anim != null)
                    {
                        m_anim.SetTrigger("Attack3");
                    }

                    mainHand.Swing();
                    currComboIdx = 0;
                    SoundManager.PlayASource("Melee2");
                    break;
            }
        }
    }

    void DoMovement()
    {
        m_oldRotation = m_playerCamera.transform.rotation;

        Vector3 temp = m_oldRotation.eulerAngles;
        temp.x = 0;
        m_playerCamera.transform.rotation = Quaternion.Euler(temp);

        m_horAxis = Input.GetAxis("Horizontal");
        m_verAxis = Input.GetAxis("Vertical");

        m_move.x = m_horAxis;
        m_move.y = 0;
        m_move.z = m_verAxis;

        m_move = m_playerCamera.transform.TransformDirection(m_move);

        m_playerCamera.transform.rotation = m_oldRotation;

        m_move.y = 0;

        Move(m_move);
    }


    protected void Move(Vector3 moveVector)
    {
        if (AllowMovement)
        {
            m_transform.Translate(moveVector * Time.fixedDeltaTime * speed, Space.World);
        }
    }

    protected void Rotate(Vector3 rotateVector)
    {
        if (AllowMovement && rotateVector != Vector3.zero)
        {
            m_transform.rotation = Quaternion.LookRotation(new Vector3(rotateVector.x,0,rotateVector.z));
        }
    }

    private Vector3 MouseDir()
    {
        m_mousePos.x = Input.mousePosition.x;
        m_mousePos.y = Input.mousePosition.y;
        m_mousePos.z = Camera.main.WorldToScreenPoint(m_transform.position).z;

        return Camera.main.ScreenToWorldPoint(m_mousePos);
    }

    public void UpdateHealthImage()
    {
        //int hp = (int)m_stats.health;
        if (GameManager.Instance.playerHealthbar)
            GameManager.Instance.playerHealthbar.value = m_stats.health / m_stats.maxHealth;
        //playerHp.sprite = playerHps[hp];
    }

    public void Dead()
    {
        m_anim.SetTrigger("Death");
        AllowMovement = false;
        isDead = true;

        if (GetComponent<Gun>())
            GetComponent<Gun>().enabled = false;

        if (GetComponentInChildren<Melee>())
            GetComponentInChildren<Melee>().enabled = false;
    }
}