using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchController : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    private readonly Stopwatch timer = new();

    public void StartStopwatch()
    {
        timer.Start();
    }

    public void PauseStopwatch()
    {
        timer.Stop();
    }

    public TimeSpan GetTime()
    {
        return timer.Elapsed;
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = GetTime().ToString("mm\\:ss\\.ff");
    }
}
