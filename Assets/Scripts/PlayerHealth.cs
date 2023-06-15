using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int maxCorruption = 100;

    public int currentHealth; 
    public int currentCorruption;

    public HealthBar healthBar;
    public CorruptionBar corruptionBar;

    public Animator animator;

    public int decayRate = 1000;
    float nextDecayDamage = 0f;

    public bool isInvincible = false;
    public int InvicibleAfterHit = 3;

    float ypos;
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        healthBar = FindObjectOfType<HealthBar>();
        corruptionBar = FindObjectOfType<CorruptionBar>();

        currentHealth = maxHealth;
        currentCorruption = 0;

        healthBar.SetMaxHealth(maxHealth);
        corruptionBar.ResetCorruption();
    }

    void Update()
    {

        ypos = rb.position.y;


        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(20);
        }

        if (ypos < 0 && currentCorruption != 100)
        {
            if (Time.time >= nextDecayDamage)
            {
                TakeCorruption(1);
                nextDecayDamage = Time.time + 1f / decayRate;
            }
        }

        if (ypos > 0 && currentCorruption !=0)
        {
            GetComponent<SpriteRenderer>().color = CodeMonkey.Utils.UtilsClass.GetColorFromString("FFFFFF");

            if (Time.time >= nextDecayDamage)
            {
                TakeCorruption(-1);
                nextDecayDamage = Time.time + 1f / decayRate;
            }
        }

        if (currentCorruption == 100)
        {
            if (Time.time >= nextDecayDamage)
            {
                TakeCorruptionDamage(1);
                nextDecayDamage = Time.time + 1f / 10;
            }
        }

        //if the corruption bar is empty, disable it
        if (currentCorruption == 0)
        {
            corruptionBar.gameObject.SetActive(false);
        }
        else
        {
            corruptionBar.gameObject.SetActive(true);
        }
    }



    public void TakeDamage(int damage)
    {
        if (!isInvincible) 
        {
            animator.SetTrigger("Hit");
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            isInvincible = true;
            StartCoroutine(InviciblityFlash());
        }
    }
    public void TakeCorruptionDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        StartCoroutine(CorruptionFlash());
    }

    public void TakeCorruption(int corruptionHit)
    {
        currentCorruption += corruptionHit;
        corruptionBar.SetCorruption(currentCorruption);
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Healed for " + heal + " health");
    }

    public IEnumerator InviciblityFlash()
    {
        isInvincible = true;
        for (int i = 0; i < InvicibleAfterHit; i++)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
        isInvincible = false;
    }

    public IEnumerator CorruptionFlash()
    {
        GetComponent<SpriteRenderer>().color = CodeMonkey.Utils.UtilsClass.GetColorFromString("7f59bd");
        yield return new WaitForSeconds(0.06f);
        GetComponent<SpriteRenderer>().color = CodeMonkey.Utils.UtilsClass.GetColorFromString("5d23ba");
        yield return new WaitForSeconds(0.03f);

        

    }

}
