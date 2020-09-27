using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CountdownText : MonoBehaviour
{
    private Text _countDownText;

    public int countDownSeconds = 3;

    private void OnEnable()
    {
        _countDownText = GetComponent<Text>();
        _countDownText.text = countDownSeconds.ToString();
        StartCoroutine(nameof(Countdown));
    }

    private IEnumerator Countdown()
    {
        for (var i = 0; i < countDownSeconds; i++)
        {
            _countDownText.text = (countDownSeconds - i).ToString();
            yield return new WaitForSeconds(1);
        }

        OnCountdownFinished?.Invoke();
    }

    public delegate void CountdownFinished();
    public static event CountdownFinished OnCountdownFinished;
}
