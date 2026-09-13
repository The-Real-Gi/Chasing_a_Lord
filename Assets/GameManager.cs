using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{
    public static GameManager Instance { get; private set; }

    PlayersInputSet inputActions;
    public  int level=1;

    public GameObject pausePanel;
    public GameObject deathPanel;
    public GameObject winPanel;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        inputActions = new PlayersInputSet();
    }

    void OnDestroy()
    {
        inputActions?.Dispose();

        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetSceneByName("Menu").isLoaded && Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    void OnEnable()
    {
        if (Instance != this || inputActions == null)
        {
            return;
        }
        inputActions.Movement.Enable();
        inputActions.Movement.PauseGame.performed += ctx => PauseGame();
    }

    void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Movement.Disable();
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void LoadNextLevel()
    {
        level++;
        SceneManager.LoadScene("Level"+level.ToString());
    }

    public void PauseGame()
    {
        if(Time.timeScale==0f)
        {
            Time.timeScale=1f;
        }
        else
        {
            Time.timeScale=0f;
        }
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
    }
    
    public void GoToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void OpenWinPanel()
    {
        Time.timeScale=0f;
        winPanel.SetActive(true);
    }

    public void DeathPanel()
    {
        Time.timeScale=0f;
        deathPanel.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(level);
    }
   

}
