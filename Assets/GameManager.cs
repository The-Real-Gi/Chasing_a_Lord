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
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeDuration = 1f;
    private Coroutine fadeRoutine;

void Awake()
{
    Time.timeScale = 1f;
    if (Instance != null && Instance != this) { Destroy(gameObject);return; }
    
    Instance = this;
    Debug.Log("GameManager Awake, Instance is " + (Instance == null ? "null" : "set"));
    DontDestroyOnLoad(gameObject);
    inputActions = new PlayersInputSet();
    SceneManager.sceneLoaded += OnSceneLoaded;
    FindPanels();
    //StartCoroutine(FadeInScene()); // covers the very first scene, before sceneLoaded ever fires
}

void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    FindPanels();
    Time.timeScale = 1f;
    if (fadeRoutine != null) StopCoroutine(fadeRoutine);
    fadeRoutine = StartCoroutine(FadeInScene());
}

System.Collections.IEnumerator FadeInScene()
{
    CanvasGroup fadeCanvasGroup = ScreenFader.CreateOverlay(1f);
    Debug.Log("[Fade] overlay created, alpha=" + fadeCanvasGroup.alpha); 
    yield return ScreenFader.Fade(fadeCanvasGroup, 1f, 0f, fadeInDuration);
    Destroy(fadeCanvasGroup.gameObject);
}
void FindPanels()
{
    pausePanel = FindPanel("PausePanel");
    deathPanel = FindPanel("DeathPanel");
    winPanel = FindPanel("WinPanel");
}

GameObject FindPanel(string panelName)
{
    Transform[] transforms = FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Transform panelTransform in transforms)
        {
            if (panelTransform.name == panelName && panelTransform.gameObject.scene.IsValid())
            {
                return panelTransform.gameObject;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetSceneByName("Menu").isLoaded && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(FadeAndLoadNextLevel());
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
        inputActions.Movement.ToMainMenu.performed += ctx =>
        {    
        if (deathPanel == null || pausePanel == null || winPanel == null)
        {
            return;
        }

        if(deathPanel.activeInHierarchy||pausePanel.activeInHierarchy||winPanel.activeInHierarchy)
        {
            SceneManager.LoadScene("Menu");
        }else{return;}
        };

        inputActions.Movement.NextLevel.performed+=ctx=>{    
        if (winPanel == null)
        {
            return;
        }

        if(winPanel.activeInHierarchy)
        {
            LoadNextLevel();
        }else{return;}
        };

        inputActions.Movement.RetryLevel.performed += ctx =>
        {
        if (deathPanel == null)
        {
            return;
        }

        if(deathPanel.activeInHierarchy)
        {
            SceneManager.LoadScene(level);
        }   else{return;}
        };

    }

    void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Movement.Disable();
        }
    }
     System.Collections.IEnumerator FadeAndLoadNextLevel()
{
    CanvasGroup fadeCanvasGroup = ScreenFader.CreateOverlay(0f);
    yield return ScreenFader.Fade(fadeCanvasGroup, 0f, 1f, fadeDuration);
    GameManager.Instance.LoadNextLevel();
    }
    public void LoadNextLevel()
    {
        level++;
        SceneManager.LoadScene("Level "+level.ToString());
    }

    public void PauseGame()
    {
        if (pausePanel == null)
        {
            return;
        }

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
        if (winPanel == null)
        {
            return;
        }

        Time.timeScale=0f;
        winPanel.SetActive(true);
    }

    public void DeathPanel()
    {
        if (deathPanel == null)
        {
            return;
        }

        Time.timeScale=0f;
        deathPanel.SetActive(true);
    }

    public void TryAgain()
    {
        Time.timeScale=1f;
        SceneManager.LoadScene(level);
    }
   

}
