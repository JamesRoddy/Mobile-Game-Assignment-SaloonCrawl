using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    playerController player;
    TMP_Text scoreText;

    void Start()
    {
        player = FindFirstObjectByType<playerController>();
        scoreText = GetComponent<TMP_Text>();
       
        scoreText.text = player.currentScoreString;

    }

    // Update is called once per frame
    void Update()
    {
     
        scoreText.text = player.currentScoreString;


    }
}
