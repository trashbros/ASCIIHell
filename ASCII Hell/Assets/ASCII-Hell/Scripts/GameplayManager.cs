using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [Header("Splash Screens")]
    [SerializeField] private GameObject TitleScreen;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject PauseScreen;

    [Header("Scene List")]
    [SerializeField] private string GameScene = "GameScene";

    [Header("Game State Info")]
    [SerializeField] private bool GameRunning = false;
    [SerializeField] GameState currentState = GameState.Title;

    enum GameState
    {
        Title,
        GameOver,
        InGame,
        Paused
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.Title;
        SetGameState();
        CustomEvents.EventUtil.AddListener(CustomEventList.PLAYER_DIED, OnPlayerDied);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputContainer.instance.cancel.pressed)
        {
            if (GameRunning && currentState != GameState.Paused)
            {
                currentState = GameState.Paused;
                SetGameState();
            }
            else if(currentState == GameState.Paused)
            {
                currentState = GameState.Title;
                SetGameState();
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
              Application.Quit();
#endif
            }
        }

        if (GameRunning)
        {
            return;
        }

        if(InputContainer.instance.confirm.pressed)
        {
            if (currentState == GameState.Paused)
            {
                // Unpause current game
                CustomEvents.EventUtil.DispatchEvent(CustomEventList.GAME_PAUSED, new object[1]{ false });
                currentState = GameState.InGame;
                SetGameState();
            }
            else
            {
                // Start new game
                SceneLoader.Instance.LoadNewScene(GameScene);
                GameplayParameters.instance.ResetParameters();
                currentState = GameState.InGame;
                SetGameState();
            }
        }
    }

    private void OnPlayerDied(CustomEvents.EventArgs evt)
    {
        if (!GameRunning)
        {
            return;
        }

        GameplayParameters.instance.Lives -= 1;

        if (GameplayParameters.instance.Lives <= 0)
        {
            // Destroy current level
            currentState = GameState.GameOver;
            SetGameState();
        }
        
    }

    private void SetGameState()
    {
        switch(currentState)
        {
            case GameState.Title:
                SceneLoader.Instance.UnloadOldScene(GameScene);
                CustomEvents.EventUtil.DispatchEvent(CustomEventList.GAME_PAUSED, new object[1] { false });
                GameRunning = false;
                TitleScreen.SetActive(true);
                GameOverScreen.SetActive(false);
                Debug.Log("Game opened");
                break;
            case GameState.InGame:
                GameRunning = true;
                TitleScreen.SetActive(false);
                GameOverScreen.SetActive(false);
                Debug.Log("Game is starting!");
                break;
            case GameState.GameOver:
                SceneLoader.Instance.UnloadOldScene(GameScene);
                CustomEvents.EventUtil.DispatchEvent(CustomEventList.GAME_PAUSED, new object[1] { false });
                GameRunning = false;
                TitleScreen.SetActive(false);
                GameOverScreen.SetActive(true);
                Debug.Log("Player has died");
                break;
            case GameState.Paused:
                GameRunning = false;
                //PauseScreen.SetActive(true);
                TitleScreen.SetActive(false);
                GameOverScreen.SetActive(false);
                CustomEvents.EventUtil.DispatchEvent(CustomEventList.GAME_PAUSED, new object[1] { true });
                Debug.Log("Game paused");
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        CustomEvents.EventUtil.RemoveListener(CustomEventList.PLAYER_DIED,OnPlayerDied);
    }
}
