using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{
    [SerializeField]private bool GameRunning = false;

    [SerializeField] private string EnterButton = "Jump";

    // Start is called before the first frame update
    void Start()
    {
        GameRunning = false;
        CustomEvents.EventUtil.AddListener(CustomEventList.PLAYER_DIED, OnPlayerDied);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameRunning)
        {
            return;
        }

        if(Input.GetButtonDown(EnterButton))
        {
            GameRunning = true;
            Debug.Log("Game is starting!");
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
            GameRunning = false;
            // Print Game over screen
            Debug.Log("Player has died");
    }

    private void OnStartPressed(CustomEvents.EventArgs evt)
    {
        // Start fresh level
        GameRunning = true;
    }

    private void OnDestroy()
    {
        CustomEvents.EventUtil.RemoveListener(CustomEventList.PLAYER_DIED,OnPlayerDied);
    }
}
