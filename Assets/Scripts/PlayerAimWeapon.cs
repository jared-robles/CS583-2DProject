using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    [Header("Aim")]
    [SerializeField] private Transform aimTransform;      
    [SerializeField] private float angleOffset = 0f;      

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;         
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float projectileLifetime = 3f;

    private Vector3 aimDirection = Vector3.right; // aim direction of wand
    private float nextShootTime;
    private PlayerExperience stats; // weapon fire rate based on player level
    

    void Awake()
    {
        if (aimTransform == null) aimTransform = transform.Find("Aim");
        if (firePoint == null && aimTransform != null) firePoint = aimTransform.Find("FirePoint");
        stats = GetComponent<PlayerExperience>();
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    // aiming logic, wand points where player mouse is on screen
    void HandleAiming()
    {
        if (aimTransform == null) return;

        Vector3 mouseWorld = GetMouseWorldPosition(aimTransform.position.z); 
        aimDirection = (mouseWorld - aimTransform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + angleOffset;
        aimTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // shooting logic, fire rate is based on player level
    void HandleShooting()
    {
        float fireRate = stats ? stats.FireRate : 1f;
        if (Time.time < nextShootTime) return;
        if (projectilePrefab == null || firePoint == null) return;

        Shoot(aimDirection);
        nextShootTime = Time.time + 1f / fireRate;
    }

    // handles the projectiles and the damage it has based on plaer level.
    void Shoot(Vector3 dir)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Destroy(proj, projectileLifetime);

        int damage = stats ? stats.Damage : 1;
        var projDamage = proj.GetComponent<ProjectileDamage>();
        if (projDamage != null)
        {
            projDamage.damage = damage;
        }

        if (proj.TryGetComponent<Rigidbody2D>(out var rb))
            rb.linearVelocity = dir * projectileSpeed;

        // direction of projectiles
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;
        proj.transform.rotation = Quaternion.Euler(0f, 0f, ang);
    }

    // mouse world on Z = 0 plane
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    // screen to world
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    // for correct depth with perspective cameras
    public static Vector3 GetMouseWorldPosition(float worldZ)
    {
        var cam = Camera.main;
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = worldZ - cam.transform.position.z; // distance from camera to gameplay plane
        return cam.ScreenToWorldPoint(screenPos);
    }
}
