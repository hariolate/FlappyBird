using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;

    public Text scoreText;

    private enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }

    public bool GameOver { get; private set; }

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
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

    private string ScoreStr => $"{CurrentScore}";

    public void ConfirmGameOver()
    {
        OnGameOverConfirmed?.Invoke();
        CurrentScore = 0;
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
        CurrentScore = 0;
        OnGameStarted?.Invoke();
        GameOver = false;
    }

    private void OnPlayerDied()
    {
        GameOver = true;
        var savedScore = PlayerPrefs.GetInt("HighScore");
        if (CurrentScore > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", CurrentScore);
        }
        SetPageState(PageState.GameOver);

    }

    private void OnPlayerScored()
    {
        CurrentScore++;
        scoreText.text = ScoreStr;
    }

    public void StartGame()
    {
        SetPageState(PageState.Countdown);
    }

    public int CurrentScore { get; private set; }

    public static int HighScore => PlayerPrefs.GetInt("HighScore");
}
