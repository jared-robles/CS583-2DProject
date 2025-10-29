using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int maxHP = 3;
    [SerializeField] int xpReward = 5;

    [Header("Death")]
    [SerializeField] string dieTrigger = "Die";     
    [SerializeField] float deathDestroyDelay = 0.8f; 
    [SerializeField] MonoBehaviour[] disableOnDeath; // disable enemy scripts upon death
    [SerializeField] AudioClip deathSFX;
    [SerializeField, Range(0f, 1f)] float deathSFXVolume = 0.5f;

    int hp;
    bool dead;
    Animator anim;
    Rigidbody2D rb;
    Collider2D col;
    PlayerExperience playerXP;

    void Awake()
    {
        hp = maxHP;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        if (PlayerController.Instance)
            playerXP = PlayerController.Instance.GetComponent<PlayerExperience>();
    }

    public void TakeDamage(int dmg)
    {
        // enemy takes damage, if damage is more than life, call the death function
        if (dead) return;
        hp -= dmg;
        if (hp <= 0) Die();
    }

    void Die()
    {
        if (dead) return;
        dead = true;

        // award XP once
        if (playerXP) playerXP.AddXP(xpReward);

        // stop any interaction
        if (col) col.enabled = false;
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
        }
        foreach (var mb in disableOnDeath)
            if (mb) mb.enabled = false;

        // play the death animation
        if (anim && !string.IsNullOrEmpty(dieTrigger))
        {
            anim.applyRootMotion = false;
            anim.SetTrigger("Die");
        }


        // play death sfx and remove enemy (if animation doesnt work)
        if (deathSFX) AudioSource.PlayClipAtPoint(deathSFX, transform.position, deathSFXVolume);
        Destroy(gameObject, deathDestroyDelay);
    }

    // when the animation completes play death sfx and remove enemy
    public void OnDeathAnimationComplete()
    {
        if (deathSFX) AudioSource.PlayClipAtPoint(deathSFX, transform.position, deathSFXVolume);
        Destroy(gameObject);
    }
}

