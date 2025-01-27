using RDG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class MenuButtonScript : MonoBehaviour
{

    public bool vibrate;

    void Start()
    {
        vibrate = (PlayerPrefs.GetInt("Vibrate", 1) == 1);
        FindObjectOfType<Toggle>().isOn = vibrate;
    }

    public void EnableMenu()
    {
        Vibrate();
        GetComponent<Animator>().ResetTrigger("Disable");
        GetComponent<Animator>().SetTrigger("Enable");
        
    }

    public void DisableMenu()
    {
        Vibrate();
        GetComponent<Animator>().ResetTrigger("Enable");
        GetComponent<Animator>().SetTrigger("Disable");
    }

    public void Vibrate()
    {
        if(vibrate)
        {
            Handheld.Vibrate();
        }
    }

    public void VibrationToggleChange(Toggle toggle)

    {

        PlayerPrefs.SetInt("Vibrate", Convert.ToInt32(toggle.isOn));

        vibrate = toggle.isOn;

        Vibrate();

    }

}
