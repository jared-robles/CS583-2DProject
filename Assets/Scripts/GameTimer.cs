using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] float durationSeconds = 120f;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject winPanel;

    float timeLeft;
    bool running;

    void Start()
    {
        Time.timeScale = 1f;
        timeLeft = durationSeconds;
        running = true;
        if (winPanel) winPanel.SetActive(false); // win screen is false until timer is up
        UpdateUI();
    }

    void Update()
    {
        if (!running) return;
        if (!PlayerController.Instance || !PlayerController.Instance.gameObject.activeSelf) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            UpdateUI();
            Win();
            return;
        }
        UpdateUI();
    }

    void Win()
    {
        running = false;
        if (winPanel) winPanel.SetActive(true);

        FreezePlayer();         // stop movement and wand
        Time.timeScale = 0f;    // pause everything else
    }

    // freeze the player when the game is done
    void FreezePlayer()
    {
        var player = PlayerController.Instance;
        if (!player) return;

        // stop physics
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false; 
        }

        
        var aim = player.GetComponent<PlayerAimWeapon>();
        if (aim) aim.enabled = false;

        player.enabled = false; 
    }

    // update the timer
    void UpdateUI()
    {
        if (!timerText) return;
        int seconds = Mathf.CeilToInt(timeLeft);
        int m = seconds / 60;
        int s = seconds % 60;
        timerText.text = $"{m:00}:{s:00}";
    }

    // restart when button is pressed on game win screen
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
