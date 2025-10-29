using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] int damage;
    private Vector3 direction;


    // When the enemy game object is active
    void FixedUpdate()
    {
        if (PlayerController.Instance.gameObject.activeSelf)
        {
            // flip enemy sprite based on player x position
            if (PlayerController.Instance.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
            // enemy follows player
            direction = (PlayerController.Instance.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * speed, direction.y * speed);
        }
        else
        {
            // if player is not active, stop following player
            rb.linearVelocity = Vector2.zero;
        }
     
    }

    // when enemy hits player, player takes damage and enemy is destroyed
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
