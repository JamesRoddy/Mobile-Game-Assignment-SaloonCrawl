using RDG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonScript : MonoBehaviour
{
    public void EnableMenu()
    {
        //Debug.Log("Trying to vibrate");
        //Vibration.Vibrate(100);
        Handheld.Vibrate();
        GetComponent<Animator>().ResetTrigger("Disable");
        GetComponent<Animator>().SetTrigger("Enable");
        
    }

    public void DisableMenu()
    {
        //Vibration.Vibrate(30);
        Handheld.Vibrate();
        GetComponent<Animator>().ResetTrigger("Enable");
        GetComponent<Animator>().SetTrigger("Disable");
    }

}
