using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;

    public Text scoreText;

    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }

    int currentScore = 0;

    bool gameOver = false;

    public bool GameOver
    {
        get
        {
            return gameOver;
        }
    }

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    private void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;

        }
    }

    private string ScoreStr
    {
        get
        {
            return $"{currentScore}";
        }
    }

    public void ConfirmGameOver()
    {
        OnGameOverConfirmed();
        currentScore = 0;
        scoreText.text = ScoreStr;
        SetPageState(PageState.Start);
    }

    private void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }

    private void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
    }


    private void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        currentScore = 0;
        OnGameStarted();
        gameOver = false;
    }

    private void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("Highscore");
        if (currentScore > savedScore)
        {
            PlayerPrefs.SetInt("Highscore", currentScore);
        }
        SetPageState(PageState.GameOver);

    }

    private void OnPlayerScored()
    {
        currentScore++;
        scoreText.text = ScoreStr;
    }

    public void StartGame()
    {
        SetPageState(PageState.Countdown);
    }

    public int CurrentScore
    {
        get
        {
            return currentScore;
        }
    }

    public int Highscore
    {
        get
        {
            return PlayerPrefs.GetInt("Highscore");
        }
    }
}
