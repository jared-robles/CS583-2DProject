using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [Header("Level / XP")]
    [SerializeField] int level = 1;
    [SerializeField] int currentXP = 0;
    [SerializeField] int xpToNext = 10;
    [SerializeField] float xpGrowth = 1.5f; // multiplier for experience needed per level

    [Header("Current Stats")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float fireRate = 5f;   
    [SerializeField] int damage = 1;

    [Header("Player Scaling")]
    [SerializeField] float moveSpeedPerLevel = 0.5f;
    [SerializeField] float fireRatePerLevel = 0.5f;
    [SerializeField] int damagePerLevel = 1;

    [SerializeField] int maxLevel = 10;
    public bool IsMaxLevel => level >= maxLevel;


    // Getters
    public int Level => level;
    public float MoveSpeed => moveSpeed;
    public float FireRate => fireRate;
    public int Damage => damage;
    public int XPToNext => xpToNext;
    public int CurrentXP => currentXP;

    void Start()
    {
        // Initialize UI at start
        UIController.Instance.UpdateExperienceBar();
    }

    public void AddXP(int amount)
    {
        if (amount <= 0 || IsMaxLevel) return;

        currentXP += amount;

        // handle level-ups
        while (!IsMaxLevel && currentXP >= xpToNext)
        {
            currentXP -= xpToNext;
            LevelUp(); // updates UI
        }

        // if max level is hit during this add, fill the bar
        if (IsMaxLevel) currentXP = xpToNext;

        // final refresh
        UIController.Instance.UpdateExperienceBar();
    }

    void LevelUp()
    {
        level++;

        // apply buffs
        moveSpeed += moveSpeedPerLevel;
        fireRate += fireRatePerLevel;
        damage += damagePerLevel;

        // cap at max
        if (level >= maxLevel)
        {
            level = maxLevel;
            currentXP = xpToNext; // keep bar full
        }
        else
        {
            xpToNext = Mathf.RoundToInt(xpToNext * xpGrowth);
        }

        // refresh on each level-up so the bar visibly resets/fills
        UIController.Instance.UpdateExperienceBar();
    }

}

