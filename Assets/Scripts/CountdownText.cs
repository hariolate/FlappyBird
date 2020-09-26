using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CountdownText : MonoBehaviour
{
    Text countDownText;

    public int countDownSeconds = 3;

    private void OnEnable()
    {
        countDownText = GetComponent<Text>();
        countDownText.text = countDownSeconds.ToString();
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown()
    {
        for (int i = 0; i < countDownSeconds; i++)
        {
            countDownText.text = (countDownSeconds - i).ToString();
            yield return new WaitForSeconds(1);
        }

        OnCountdownFinished();
    }

    public delegate void CountdownFinished();
    public static event CountdownFinished OnCountdownFinished;
}
