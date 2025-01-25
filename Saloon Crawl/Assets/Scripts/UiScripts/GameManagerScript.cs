using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManagerScript : MonoBehaviour
{
    public TMP_Text highestScoreText;
    public int finalScore;
    public int highestScore;

    // Start is called before the first frame update
    void Start()
    {
        highestScore = PlayerPrefs.GetInt("HighestScore");
        try
        {
            highestScoreText.text = highestScore.ToString();
        }
        catch
        {}
    }

    // Update is called once per frame
    void Update()
    {
        if(finalScore > highestScore)
        {
            highestScore = finalScore;
            PlayerPrefs.SetInt("HighestScore", highestScore);
        }
    }

}
