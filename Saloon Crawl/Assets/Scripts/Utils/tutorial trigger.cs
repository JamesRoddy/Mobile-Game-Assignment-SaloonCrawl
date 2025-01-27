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
    [SerializeField] private GameObject symbol;
    [SerializeField] private GameObject text;
    SpriteRenderer symbolSprite;
    SpriteRenderer textSprite;
    [SerializeField] private float timeScalar;
    [SerializeField] private float distcanceCheck;
    private bool shouldUpdateDistance = false;
    playerController playerController;
    void Start()
    {
        
        trigger  = GetComponent<Collider2D>();
        playerTouch = FindFirstObjectByType<TouchControls>();
        Debug.Log("tutorial trigger bool set to  " + Convert.ToBoolean(1 - PlayerPrefs.GetInt(expectedInput)));
        Debug.Log(expectedInput);
        symbolSprite = symbol.GetComponent < SpriteRenderer >(); 
        textSprite = text.GetComponent < SpriteRenderer >();
        textSprite.enabled = false;
        symbolSprite.enabled = false;
        playerController = FindFirstObjectByType<playerController>();
        trigger.enabled = Convert.ToBoolean(1 - PlayerPrefs.GetInt(expectedInput)); ;


    }



    private void updateDistance()
    {
        if (shouldUpdateDistance)
        {

            if (Vector3.Distance(transform.position, playerController.transform.position) > distcanceCheck) {
                
                Time.timeScale = 1.0f;
                gameObject.SetActive(false);
            
            }



        }
    }

    // Update is called once per frame
    void Update()
    {

        updateDistance();
        Debug.Log("has input "+playerTouch.inputDictionary[expectedInput]);
        if (Convert.ToBoolean( playerTouch.inputDictionary[expectedInput] ) || PlayerPrefs.GetInt(expectedInput)>0) 
        {
            Debug.Log("input recived " + expectedInput);
            Time.timeScale = 1.0f;
            PlayerPrefs.SetInt(expectedInput,playerTouch.inputDictionary[expectedInput]);
            symbolSprite.enabled = false;
            textSprite.enabled = false;
            gameObject.SetActive(false);

        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("Player"))
        {
            shouldUpdateDistance = true;
            Time.timeScale = timeScalar;
            Debug.Log(Time.timeScale + "new time scale for " + expectedInput);
            Debug.Log("input required " + expectedInput);
            symbolSprite.enabled=true;
            textSprite.enabled=true;
        }


    }


}
