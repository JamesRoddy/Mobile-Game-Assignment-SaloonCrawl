using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class GameManagerScript : MonoBehaviour
{
    public TMP_Text highestScoreText;
    public TMP_Text enemiesKilled;
    public int finalScore;
    public int highestScore;
    public int enemiesKilledThisRun;
    public int enemiesKilledTotal;
    private bool enemiesKilledUpdated = false;

    // Start is called before the first frame update
    void Start()
    {
        highestScore = PlayerPrefs.GetInt("HighestScore");
        enemiesKilledTotal = PlayerPrefs.GetInt("EnemiesKilled");
        try
        {
            enemiesKilled.text = enemiesKilledTotal.ToString();
            highestScoreText.text = highestScore.ToString();

        }
        catch
        {}
        enemiesKilledUpdated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(finalScore > highestScore)
        {
            highestScore = finalScore;
            PlayerPrefs.SetInt("HighestScore", highestScore);
        }

        if(enemiesKilledUpdated == false)
        {
            enemiesKilledTotal += enemiesKilledThisRun;
            PlayerPrefs.SetInt("EnemiesKilled", enemiesKilledTotal);
            enemiesKilledUpdated = true;
        }
        

    }

}
