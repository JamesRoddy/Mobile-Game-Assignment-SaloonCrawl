using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class GameManagerScript : MonoBehaviour
{
    public TMP_Text highestScoreText;
    public int finalScore;
    public int highestScore;
    public AudioMixer audioMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    // Start is called before the first frame update
    void Start()
    {
        //audioMixer = GetComponent<AudioMixer>();
        //AudioListener.volume = PlayerPrefs.GetFloat("AudioLevel", 1);
        //MusicSlider = GetComponent<Slider>();
        //SFXSlider = GetComponent<Slider>();

        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);

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

    /*public void AudioValueChange(Slider slider)
    {
        PlayerPrefs.SetFloat("AudioLevel", slider.value);
        AudioListener.volume = slider.value;
    }*/

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
