using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighScoreText : MonoBehaviour
{
    Text highScoreText;

    private void Start()
    {
        highScoreText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        highScoreText.text = $"Highscore: {GameManager.Instance.Highscore}";
    }
}
