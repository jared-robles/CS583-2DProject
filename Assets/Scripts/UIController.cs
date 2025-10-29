using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Slider xpBar;
    [SerializeField] private TMP_Text levelText;

    public GameObject GameOver;


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
    }

    // updates the health bar
    public void UpdateHealthBar()
    {
        playerHealthBar.maxValue = PlayerController.Instance.playerMaxHealth;
        playerHealthBar.value = PlayerController.Instance.playerHealth;
        healthText.text = playerHealthBar.value + " / " + playerHealthBar.maxValue;
    }

    // updates the experience bar, handles max level
    public void UpdateExperienceBar()
    {
        var xp = PlayerController.Instance.GetComponent<PlayerExperience>();
        xpBar.maxValue = xp.XPToNext;

        if (xp.IsMaxLevel)
        {
            xpBar.value = xpBar.maxValue;
            levelText.text = "Lv. MAX";
            return;
        }

        xpBar.value = xp.CurrentXP;
        levelText.text = "Lv. " + xp.Level;
    }
}
