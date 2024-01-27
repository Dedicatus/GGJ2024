using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GAMESTATE GameState;

    public float TargetDistance = 24000;
    public float GameTime = 180;
    [ReadOnly]
    public float remainDistance = 24000;
    [ReadOnly]
    public float remainTime = 180;


    // Start is called before the first frame update
    void Start()
    {
        remainDistance = TargetDistance;
        remainTime = GameTime;
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
        remainDistance = TargetDistance - Motorcycle.Instance.passedDistance;
        remainTime -= Time.deltaTime;
        if (remainDistance <= 0)
        {
            EndGame(true);
        }
        if (remainTime <= 0)
        {
            EndGame(false);
        }
    }

    void StartGame()
    {
        GameState = GAMESTATE.Start;
        UIManager.Instance.OnGameStart();
    }

    public void EndGame(bool win)
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
