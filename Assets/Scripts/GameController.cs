using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static ActionTypes;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject finishTutorialCanvas;
    [SerializeField] private GameObject disableActionsBP;
    [SerializeField] private InputActionReference RestartLevelInput;
    [SerializeField] private GameObject StopwatchPrefab;
    private bool finished = false;
    private PlayerMovement player;
    public static GameController Instance;
    public int[] Levels;
    public int currentLevel = 0;
    private GameObject currentDisableActionsUI;
    private StopwatchController currentStopWatch;

    void Awake()
    {
        if (Instance != null)
        {
            Instance.FindPlayer();
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Instance.FindPlayer();
    }

    public void TouchedDoor()
    {
        if (!finished)
        {
            finished = true;
            GoToNextLevel(false);
        }
    }

    void OnEnable()
    {
        RestartLevelInput.action.started += RestartLevel;
    }

    void OnDisable()
    {
        RestartLevelInput.action.started -= RestartLevel;
    }
    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialLevel", LoadSceneMode.Single);
    }

    public void StartGame()
    {
        ShuffleLevels();
        GoToNextLevel(true);
    }

    public void GoToNextLevel(bool isStartingGame)
    {
        finished = false;
        if (currentLevel >= Levels.Length)
        {
            if (SceneManager.GetActiveScene().name != "TutorialLevel")
            {
                currentStopWatch.PauseStopwatch();
                HighScoreUiController.PutHighScore(currentStopWatch.GetTime());
                Destroy(currentStopWatch.gameObject);
            }
            Destroy(Instance.gameObject);
            Destroy(ActionController.Instance.gameObject);
            Destroy(ActionUIController.Instance.gameObject);
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }
        else
        {
            if (!isStartingGame)
            {
                DisableItemMenu();
            }
            else
            {
                currentStopWatch = Instantiate(StopwatchPrefab, transform).GetComponent<StopwatchController>();
                string nextLevel = "Stage " + Levels[currentLevel++];
                SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
                currentStopWatch.StartStopwatch();
            }
        }
    }

    public void RestartLevel(InputAction.CallbackContext callbackContext)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void ShuffleLevels()
    {
        Levels = new int[] {1, 2, 3, 4, 5};
        for (int i = 0; i < 5; i++)
        {
            int swapWith = Random.Range(i, 5);
            (Levels[i], Levels[swapWith]) = (Levels[swapWith], Levels[i]);
        }
    }

    private void FindPlayer()
    {
        GameObject playerGameObject = GameObject.Find("Player");
        if (playerGameObject == null)
        {
            player = null;
            return;
        }
        player = playerGameObject.GetComponent<PlayerMovement>();
    }

    private void DisableItemMenu()
    {
        currentDisableActionsUI = Instantiate(disableActionsBP);
        currentDisableActionsUI.GetComponent<ActionChoiceCanvasController>().Initialize(ActionController.Instance.possibleActions);
    }

    public void DisableItem(ActionsEnum type)
    {   
        ActionController.Instance.DisableItem(type);
        Destroy(currentDisableActionsUI);
        string nextLevel = "Stage " + Levels[currentLevel++];
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
