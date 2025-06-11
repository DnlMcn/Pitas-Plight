using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    public GameObject canvas;

    public TextMeshProUGUI text;
    public Button restartButton;
    public Button quitButton;

    void OnEnable()
    {
        EventBus<PlayerDied>.OnEvent += ShowGameOverScreen;
        EventBus<WinState>.OnEvent += ShowVictoryScreen;
    }

    void OnDisable()
    {
        EventBus<PlayerDied>.OnEvent -= ShowGameOverScreen;
        EventBus<WinState>.OnEvent -= ShowVictoryScreen;
    }

    void Awake()
    {
        canvas.SetActive(false);
    }

    void ShowGameOverScreen(PlayerDied e)
    {
        print("UI Manager received game over event!");

        canvas.SetActive(true);
    }

    void ShowVictoryScreen(WinState e)
    {
        text.text = "You win!";

        canvas.SetActive(true);
    }

    public void Restart()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        // stop play-mode in the editor
        EditorApplication.isPlaying = false;
        #else
        // quits the standalone build
        Application.Quit();
        #endif
    }

    void Start()
    {
        TextMeshProUGUI restartButtonText = restartButton.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI quitButtonText = quitButton.GetComponentInChildren<TextMeshProUGUI>();

        restartButtonText.text = "Restart";
        quitButtonText.text = "Quit";
    }
}
