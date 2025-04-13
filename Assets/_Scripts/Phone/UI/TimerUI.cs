using UnityEngine;
using TMPro;
using System;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        StopTime();
    }

    public void SetTime(float timeLeft)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeLeft);
        timerText.text = time.ToString(@"ss\:ff"); // 03:48
    }

    public void StopTime()
    {
        timerText.text = "XO:XO";
    }
}
