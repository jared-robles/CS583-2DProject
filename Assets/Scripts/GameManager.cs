using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
    
    // when game is over, set game over screen to appear
    public void GameOver()
    {
        UIController.Instance.GameOver.SetActive(true);
    }

    // when player presses the restart button, reload the game scene
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
