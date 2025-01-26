using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class GameManagerScript : MonoBehaviour
{
    public TMP_Text highestScoreText;
    public TMP_Text currentScoreText;
    public TMP_Text EnemiesKilled;
    public int finalScore;
    public int highestScore;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;
    public int mostEnemiesKilled;
    public int enemiesKilledThisRun;

    // Start is called before the first frame update
    void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);

        highestScore = PlayerPrefs.GetInt("HighestScore");
        mostEnemiesKilled = PlayerPrefs.GetInt("MostEnemiesKilled");
        try
        {
            highestScoreText.text = highestScore.ToString();
            EnemiesKilled.text = mostEnemiesKilled.ToString();
            
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
        if(enemiesKilledThisRun > mostEnemiesKilled) 
        {
            mostEnemiesKilled = enemiesKilledThisRun;
            PlayerPrefs.SetInt("MostEnemiesKilled", mostEnemiesKilled);
        }

/*        currentScoreText.text = finalScore.ToString();*/
    }

    public void SetMusicVolume(Slider slider)
    {
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(slider.value) * 20);
    }

    public void SetSFXVolume(Slider slider)
    {
        PlayerPrefs.SetFloat("SFXVolume", slider.value);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(slider.value) * 20);
    }

}
