using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float health;
    public float maxHealth;

    [SerializeField]
    private int fameLevel;

    [SerializeField]
    private float fameExperience;

    [SerializeField]
    private float experienceToNextLevel;

    public bool isDead;

    public bool hasDeadAnim;

    public float scoreOnDeath;

    public float playerScale;

    public GameObject levelUpParticle;

    private ParticleSystem _levelUpParticle;

    private CameraFollow _cFollow;

    private bool isBossGoober;

    void Start()
    {
        _cFollow = FindObjectOfType<Camera>().GetComponent<CameraFollow>();
        if(transform.tag == "Player") _levelUpParticle = levelUpParticle.GetComponent<ParticleSystem>();
        fameLevel = 1;
        fameExperience = 0;
        experienceToNextLevel = 100;

        if (GetComponentInChildren<SpriteRenderer>())
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
        if(GetComponentInChildren<BossGoober>())
        {
            isBossGoober = true;
        }
       
    }

    private IEnumerator FlashSprite(SpriteRenderer sprite)
    {
        sprite.color = Color.red;
        //sprite.material.SetColor("_EmissionColor", new Color(10, 10, 10));
        yield return new WaitForSeconds(0.1f);
        //sprite.material.SetColor("_EmissionColor", new Color(1, 1, 1));
        sprite.color = Color.white;
    }

    private IEnumerator PlayerTakeDamageEffect()
    {
        GameManager.Instance.playerTakeDamageEffect.SetActive(true);
        //sprite.material.SetColor("_EmissionColor", new Color(10, 10, 10));
        yield return new WaitForSeconds(0.1f);
        //sprite.material.SetColor("_EmissionColor", new Color(1, 1, 1));
        GameManager.Instance.playerTakeDamageEffect.SetActive(false);
    }

    public void TakeDmg(float dmg)
    {

        if(isBossGoober)
        {
            if(GameManager.Instance.player.GetComponent<Stats>().fameLevel < 5)
            {
                //cant damage
                return;
            }
        }
        //particle effect/flash
        if (isDead)
            return;

        if(GetComponentInChildren<SpriteRenderer>())
        {
            StartCoroutine(FlashSprite(GetComponentInChildren<SpriteRenderer>()));
        }


        if (GetComponent<PlayerController>())
        {
            //player takes damage sound
            //PlayerController pc = GetComponent<PlayerController>();
            SoundManager.PlayASource("lose");

            StartCoroutine(PlayerTakeDamageEffect());
        }

        health -= dmg;
        if (health <= 0 && !isDead)
            Dead();

        if (GetComponent<Enemy>())
        {
            GetComponent<Enemy>().FlashHealthBar();
            GetComponent<Enemy>().chase = true;

            /*int r = Random.Range(0, 2);
            if (!isDead && !GetComponent<Enemy>().isMelee)
                SoundManager.PlayASource((r == 0) ? "EnemyHurt" : "EnemyHurt2");
            else if (GetComponent<Enemy>().isMelee)
            {
                SoundManager.PlayASource("Dog2");
            }*/

            //  FindObjectOfType<Blood>().SpawnBlood(transform.position);
        }
        else if (GetComponent<PlayerController>())
            GetComponent<PlayerController>().UpdateHealthImage();
    }

    public void Dead()
    {
        isDead = true;

        if (!GetComponent<PlayerController>())
            StartCoroutine(WaitDestroy());
        else
        {
            GetComponent<PlayerController>().Dead();
        }
    }

    public bool isHealing = false;

    public void Heal(float amount, float durationSeconds)
    {
        if (amount <= 0 || isHealing)
            return;

        isHealing = true;

        StartCoroutine(HealOvertime(amount, durationSeconds));

    }

    public void GainExperience(float amount)
    {
        fameExperience += amount;
        if (fameExperience >= experienceToNextLevel)
        {
            _levelUpParticle.Play();
            fameLevel++;
            float overflowXP = fameExperience - experienceToNextLevel;
            fameExperience = 0;
            GainExperience(overflowXP);
            // experienceToNextLevel *= 1.15f; //arbitrary multipliers
            playerScale *= 1.1f;
            transform.localScale *= 1.25f;
            _cFollow.distance *= 1.23f;
            _cFollow.offsetZ *= 1.23f;
            _cFollow.offsetX *= 1.23f;
            GameManager.Instance.SpawnEnemies(3);

            GetComponent<PlayerController>().speed *= 1.15f;
            GetComponent<Rigidbody>().mass *= 1.2f;

            // Debug.Log(playerScale);
            Heal(2, 2);

            // increase player damage
            Weapon wep = transform.root.GetComponentInChildren<Weapon>();
            wep.damage += .5F;

            GameManager.Instance.Celebrate();
        }
    }

    IEnumerator HealOvertime(float amount, float duration)
    {
        float healPerSecond = amount / duration;
        float totalHealed = 0;

        while (totalHealed < amount)
        {
            health += healPerSecond;
            if (health >= maxHealth)
                health = maxHealth;
            totalHealed += healPerSecond;

            if (GetComponent<PlayerController>())
                GetComponent<PlayerController>().UpdateHealthImage();
            yield return new WaitForSeconds(1);
        }

        isHealing = false;
    }



    IEnumerator WaitDestroy()
    {
        if (GetComponentInChildren<Animator>())
        {
            if (hasDeadAnim)
                GetComponentInChildren<Animator>().SetTrigger("Death");
            if (GetComponent<Enemy>())
            {
                /*Enemy e = GetComponent<Enemy>();
                if (e.isRanged)
                {
                    //fix death animation...
                    e.GetComponentInChildren<Animator>().gameObject.transform.position += new Vector3(0, 2, 0);
                }

                if (e.isScout)
                {
                    e.GetComponentInChildren<Animator>().gameObject.transform.Rotate(-75, 0, 0);
                    e.GetComponentInChildren<Animator>().gameObject.transform.position += new Vector3(0, 1f, 0);
                }*/
                FindObjectOfType<PlayerController>().GetComponent<Stats>().GainExperience(50);
                SoundManager.PlayASource("win");
            }
        }


        //TODO: Spawn atte blood here
        // FindObjectOfType<Blood>().SpawnBlood(transform.position);

        if (GetComponent<Enemy>())
        {
            GetComponent<Enemy>().AllowMovement = false;
            if(GetComponent<Enemy>().healthBar)
                GetComponent<Enemy>().healthBar.gameObject.SetActive(false);
            GetComponent<Enemy>().isEnabled = false;

            if (GetComponentInChildren<Light>())
                GetComponentInChildren<Light>().gameObject.SetActive(false);


            FindObjectOfType<PlayerController>().score += scoreOnDeath;


            if (GetComponent<Enemy>().isMelee)
            {
                SoundManager.PlayASource("Dog1");
            }
            else
                SoundManager.PlayASource("EnemyDie");
        }

        if (GetComponent<Gun>())
            GetComponent<Gun>().enabled = false;

        if (GetComponentInChildren<Melee>())
            GetComponentInChildren<Melee>().enabled = false;

        if (hasDeadAnim)
            yield return new WaitForSeconds(4);
        else
            yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }
}
