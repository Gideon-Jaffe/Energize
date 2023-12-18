using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class HighScoreUiController : MonoBehaviour
{
    private List<string> highScores = new();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 5; i++)
        {
            string currentHighScore = PlayerPrefs.GetString("HighScore" + i, "");
            highScores.Add(currentHighScore);
            CreateHighScoreText(i, currentHighScore);
        }
    }

    private void CreateHighScoreText(int location, string highScore)
    {
        GameObject newHighScore = new GameObject("HighScore" + location);
        newHighScore.transform.SetParent(transform);

        TMP_Text myText = newHighScore.AddComponent<TextMeshProUGUI>();
        myText.color = Color.black;
        myText.enableWordWrapping = false;
        myText.text = location + ": " + highScore;
    }

    public static void PutHighScore(TimeSpan timeSpan)
    {
        for (int i = 1; i <= 5; i++)
        {
            string currentHighScore = PlayerPrefs.GetString("HighScore" + i, "");
            if (currentHighScore == "")
            {
                PlayerPrefs.SetString("HighScore" + i, timeSpan.ToString("mm\\:ss\\.ff"));
                return;
            }
            TimeSpan duration = TimeSpan.ParseExact(currentHighScore, "mm\\:ss\\.ff", CultureInfo.InvariantCulture);
            if (timeSpan < duration)
            {
                MoveHighScoresDown(i);
                PlayerPrefs.SetString("HighScore" + i, timeSpan.ToString("mm\\:ss\\.ff"));
                return;
            }
        }
    }

    private static void MoveHighScoresDown(int starting)
    {
        for (; starting < 5; starting++)
        {
            string currentHighScore = PlayerPrefs.GetString("HighScore" + starting, "");
            if (currentHighScore == "") return;
            PlayerPrefs.SetString("HighScore" + (starting + 1), currentHighScore);
        }
    }
}
