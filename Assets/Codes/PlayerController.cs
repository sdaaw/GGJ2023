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
    public string heroName;

    private Rigidbody m_rigidbody;
    private Transform m_transform;

    public ParticleSystem dashParticle;

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

    [SerializeField]
    private LayerMask m_dashMask;

    public float dashDistance;
    public float dashCooldown;
    public bool canDash = true;
    private bool m_allowDash;

    public List<AudioClip> clips = new List<AudioClip>();
    public AudioSource bgMusic;

    private Animator m_anim;

    public Image playerDash;
    private float dashTimerForUI;

    private Stats m_stats;

    public Melee mainHand;
    public Melee offHand;

    private int currComboIdx = 0;

    public float comboTimer = 0;
    public bool comboStarted = false;
    private bool attemptEat;

    public AudioSource walkSound;

    private bool isDead = false;

    private void Awake()
    {
        m_transform = transform;
        m_rigidbody = GetComponent<Rigidbody>();
        m_allowDash = true;
        m_playerCamera = FindObjectOfType<Camera>();
        m_anim = GetComponentInChildren<Animator>();
        m_stats = GetComponent<Stats>();
    }

    void FixedUpdate()
    {
        if (AllowMovement)
        {
            DoMovement();
            Rotate(m_move);
            //Rotate((MouseDir() - m_transform.position).normalized);
        }
    }

    private void Update()
    {
        if (isDead)
        {

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MikkoHideOut");
            }


            return;
        }
           

        if (m_transform.position.y < -20)
            GetComponent<Stats>().TakeDmg(10000);
        
        if(m_stats != null && m_stats.isDead)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("ForestScene");
            }
        }

        if (m_anim != null)
        {
            float speed = Mathf.Abs(m_move.magnitude);
            m_anim.SetFloat("Speed", speed);

            if (speed > 0 && !walkSound.isPlaying)
                walkSound.Play();
            else if(speed <= 0)
                walkSound.Pause();
        }

        if (!AllowMovement)
            return;

        /*if(Input.GetKeyDown(KeyCode.E))
        {
            if(m_allowDash && canDash)
            {
                StartCoroutine(Dash());
                dashTimerForUI = 0;
                m_anim.SetTrigger("Charge");
            }     
        }*/

        if(comboStarted)
        {
            comboTimer += 1 * Time.deltaTime;
            if (comboTimer > 3)
            {
                // GameManager gm = FindObjectOfType<GameManager>();
                comboTimer = 0;
                currComboIdx = 0;
                comboStarted = false;
                // if(gm.boboFace != null)
                //    gm.boboFace.sprite = gm.boboNormal;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            AttemptEat();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            comboStarted = true;
            if(mainHand.canMelee)
            {
                // SaveFile sf = FindObjectOfType<SaveFile>();
                /*GameManager gm = FindObjectOfType<GameManager>();
                if(gm.boboFace != null)
                    gm.boboFace.sprite = gm.boboAngry;*/

                /*if (currComboIdx >= sf.loadedSave.currentMaxCombo)
                {
                    currComboIdx = 0;
                }
                else
                {
                    currComboIdx++;
                }*/

                //Debug.Log(currComboIdx);

                switch (currComboIdx)
                {      
                    case 0:
                        if(m_anim != null)
                        {
                            m_anim.SetTrigger("Attack1");
                            m_anim.speed = mainHand.swingTimerMax;
                        }
                      
                        mainHand.Swing();
                        currComboIdx++;
                        SoundManager.PlayASource("YetiSound1");
                        break;
                    case 1:
                        if (m_anim != null)
                        {
                            m_anim.SetTrigger("Attack2");
                            m_anim.speed = mainHand.swingTimerMax;
                        }
                           
                        offHand.Swing();
                        SoundManager.PlayASource("YetiSound2");
                        // sf.loadedSave.currentMaxCombo
                        if (currComboIdx < 2)
                            currComboIdx++;
                        else
                        {
                            currComboIdx = 0;
                            comboTimer = 3;
                        } 
                        break;
                    case 2:
                        if (m_anim != null)
                        {
                            m_anim.SetTrigger("Attack3");
                        }
                           
                        mainHand.Swing();
                        offHand.Swing();
                        currComboIdx = 0;
                        SoundManager.PlayASource("YetiSound3");
                        break;
                }

                //SoundManager.PlayASource("Swing");
            }
        }

        if (dashTimerForUI < dashCooldown)
            dashTimerForUI += Time.deltaTime;

        if(playerDash != null)
            UpdateDashImage();
    }

    public void AttemptEat()
    {
        if (attemptEat)
            return;

        SoundManager.PlayASource("YetiShout");
        AllowMovement = false;
        m_anim.SetTrigger("Eat");
        attemptEat = true;
        StartCoroutine(WaitEat());
    }

    private IEnumerator WaitEat()
    {
        yield return new WaitForSeconds(1);
        attemptEat = false;
        AllowMovement = true;
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

    private IEnumerator Dash()
    {
        SoundManager.PlayASource("GunSound");
        m_allowDash = false;

        Vector3 dir = m_transform.forward;

        if (m_move.magnitude > 0)
        {
            dir = m_move;
        }

        RaycastHit[] hit = Physics.RaycastAll(m_transform.position, dir, dashDistance + 1, m_dashMask);

        if (hit.Length > 1 && hit[1].collider != null)
        {
            //???
            m_transform.position = hit[1].point - dir;
        }
        else
        {
            m_transform.Translate(dir * dashDistance, Space.World);

            //TODO:
            //move towards dashpoint with lerp
        }
        dashParticle.Play();
        yield return new WaitForSeconds(dashCooldown);

        m_allowDash = true;
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
        int hp = (int)m_stats.health;
        //playerHp.sprite = playerHps[hp];
    }

    public void UpdateDashImage()
    {
        playerDash.fillAmount = dashTimerForUI / dashCooldown;
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


        //reset save data
        //go to mikko scene

        // FindObjectOfType<SaveFile>().loadedSave = new SaveData();
        // FindObjectOfType<SaveFile>().NewSave();
    }

    public void OnTriggerStay(Collider other)
    {
        if (!attemptEat)
            return;

        Enemy e = other.transform.root.GetComponent<Enemy>();
        if (e != null && e.isEatable)
        {
            // do something
            // heal?
            // score?
            m_stats.Heal(m_stats.maxHealth / 2, 5);
            //add exp?

            Destroy(e.gameObject);

        }
    }

}