using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighScoreText : MonoBehaviour
{
    private Text _highScoreText;

    private void Start()
    {
        _highScoreText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        _highScoreText.text = $"HighScore: {GameManager.HighScore}";
    }
}
