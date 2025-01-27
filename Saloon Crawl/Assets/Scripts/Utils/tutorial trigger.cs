using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class tutorialtrigger : MonoBehaviour
{
    // Start is called before the first frame update
     Collider2D trigger;
     TouchControls playerTouch;
     SpriteRenderer tutorialSpirte;
     [SerializeField]  string expectedInput;
    void Start()
    {
        
        trigger  = GetComponent<Collider2D>();
        playerTouch = FindFirstObjectByType<TouchControls>();
        Debug.Log("tutorial trigger bool set to  " + Convert.ToBoolean(1 - PlayerPrefs.GetInt(expectedInput)));
        tutorialSpirte = GetComponent < SpriteRenderer >();
        tutorialSpirte.enabled = false;
        trigger.enabled = Convert.ToBoolean(1 - PlayerPrefs.GetInt(expectedInput)); ;


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("has input "+playerTouch.inputDictionary[expectedInput]);
        if (Convert.ToBoolean( playerTouch.inputDictionary[expectedInput])) 
        {
            Debug.Log("input recived " + expectedInput);
            Time.timeScale = 1.0f;
            PlayerPrefs.SetInt(expectedInput,playerTouch.inputDictionary[expectedInput]); 
            
            
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("Player"))
        {
            
            Time.timeScale = 0.2f;
            Debug.Log("input required " + expectedInput);
            tutorialSpirte.enabled=true;

        }


    }


}
