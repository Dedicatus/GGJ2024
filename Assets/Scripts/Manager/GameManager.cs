using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GAMESTATE GameState;
    // Start is called before the first frame update
    void Start()
    {
        GameState = GAMESTATE.Ending;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(GameState == GAMESTATE.Ending)
            {               
                StartGame();
            }
            else if(GameState == GAMESTATE.Start)
            {
                PauseGame();
            }else if(GameState == GAMESTATE.Pause)
            {
                ContinueGame();
            }
        }
    }

    void StartGame()
    {
        GameState = GAMESTATE.Start;
        UIManager.Instance.OnGameStart();
    }

    public void EndGame()
    {
        GameState = GAMESTATE.Ending;
    }

    public void PauseGame()
    {
        GameState = GAMESTATE.Pause;
        UIManager.Instance.OnGamePause();
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        GameState = GAMESTATE.Start;
        UIManager.Instance.OnGameContinue();
        Time.timeScale = 1;
    }

    public enum GAMESTATE
    {
        Start,
        Pause,
        Ending
    }



}
