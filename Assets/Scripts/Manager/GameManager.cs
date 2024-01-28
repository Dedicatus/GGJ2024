using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public GAMESTATE GameState;

    public float TargetDistance = 2400;
    public float GameTime = 90;
    [ReadOnly]
    public float remainDistance = 2400;
    [ReadOnly]
    public float remainTime = 90;
    public UnityAction OnGameStart;

    // Start is called before the first frame update
    void Start()
    {
        remainDistance = TargetDistance;
        remainTime = GameTime;
        GameState = GAMESTATE.Intro;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(GameState == GAMESTATE.Intro)
            {               
                StartGame();
            }
            else if(GameState == GAMESTATE.Start)
            {
                PauseGame();
            }
            else if(GameState == GAMESTATE.Pause)
            {
                ContinueGame();
            }
            else if(GameState == GAMESTATE.Ending)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        OnGameStart?.Invoke();
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
        Intro,
        Start,
        Pause,
        Ending
    }
}
