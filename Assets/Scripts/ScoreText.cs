using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour
{
    Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = "0";
    }

    void OnEnable()
    {
        scoreText.text = GameManager.Instance.CurrentScore.ToString();
    }
}
