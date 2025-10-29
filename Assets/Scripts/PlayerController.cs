using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField] float speed = 5f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sprite;   

    Animator animator;
    Vector2 input;
    bool facingRight = true;                  
    const float deadzone = 0.01f;

    public float playerMaxHealth;
    public float playerHealth;

    [SerializeField] AudioClip hurtSFX;
    [SerializeField, Range(0f, 1f)] float hurtVolume = 0.5f;


    PlayerExperience stats;
    float CurrentSpeed => stats ? stats.MoveSpeed : speed; // speed based on player experience

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        animator = GetComponent<Animator>();
        if (!sprite) sprite = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<PlayerExperience>(); 
    }

    // set health to max and update the health bar when game begins
    private void Start()
    {
        playerHealth = playerMaxHealth;
        UIController.Instance.UpdateHealthBar();
    }

    void Update()
    {
        // read input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        // update facing only when there is horizontal input
        if (Mathf.Abs(input.x) > deadzone)
            facingRight = input.x > 0f;

        // apply visual flip based on last facing
        if (sprite) sprite.flipX = !facingRight;
        else transform.localScale = new Vector3(facingRight ? 1f : -1f, 1f, 1f);

        // animator speed
        animator.SetFloat("Speed", input.magnitude * speed);
    }

    // player movement scaled with speed
    void FixedUpdate()
    {
        rb.linearVelocity = input * CurrentSpeed;
        
    }

    // player takes damage, updates health bar accordingly
    public void TakeDamage(float damage)
    {

        if (hurtSFX) AudioSource.PlayClipAtPoint(hurtSFX, transform.position, hurtVolume);

        playerHealth -= damage;
        UIController.Instance.UpdateHealthBar();

        // destroy player and make game over screen appear when health is 0 or below
        if (playerHealth <= 0f)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }
}

