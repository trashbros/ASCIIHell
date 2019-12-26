using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]private bool GameRunning = false;

    [SerializeField] private string EnterButton = "Submit";
    [SerializeField] private string QuitButton = "Cancel";

    [SerializeField] private GameObject TitleScreen;
    [SerializeField] private GameObject GameOverScreen;

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
        if (Input.GetButtonDown(QuitButton))
        {
            if (GameRunning)
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

        if(Input.GetButtonDown(EnterButton))
        {
            if (currentState == GameState.Paused)
            {
                // Unpause current game
                currentState = GameState.InGame;
                SetGameState();
            }
            else
            {
                // Start new game
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
        print(evt.type);
        print(evt.args);
        // Destroy current level
        currentState = GameState.GameOver;
        SetGameState();
        
    }

    private void SetGameState()
    {
        switch(currentState)
        {
            case GameState.Title:
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
                GameRunning = false;
                TitleScreen.SetActive(false);
                GameOverScreen.SetActive(true);
                Debug.Log("Player has died");
                break;
            case GameState.Paused:
                GameRunning = true;
                //PauseScreen.SetActive(true);
                TitleScreen.SetActive(false);
                GameOverScreen.SetActive(false);
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
