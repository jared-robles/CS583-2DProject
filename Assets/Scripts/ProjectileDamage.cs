using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    // default damage
    public int damage = 1;
    [SerializeField] bool destroyOnHit = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        // only damage if the target has EnemyHealth
        EnemyHealth health = other.GetComponent<EnemyHealth>();
        if (health == null)
        {
            return;
        }

        health.TakeDamage(damage);

        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
        
    }
}

