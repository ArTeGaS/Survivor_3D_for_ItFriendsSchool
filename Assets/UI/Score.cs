using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static float score;
    private void Update()
    {
        scoreText.text = $"Score: {score}";
    }
}
