using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour
{
    private Text _scoreText;

    private void Start()
    {
        _scoreText = GetComponent<Text>();
        _scoreText.text = "0";
    }

    private void OnEnable()
    {
        _scoreText.text = GameManager.instance.CurrentScore.ToString();
    }
}
