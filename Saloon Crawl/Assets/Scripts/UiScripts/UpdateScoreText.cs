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
    public TMP_Text finalScore;

    void Start()
    {
        player = FindFirstObjectByType<playerController>();
        scoreText = GetComponent<TMP_Text>();
       
    

    }

    // Update is called once per frame
    public void UpdateText(string value)
    {
        scoreText.text = value;
        finalScore.text = player.currentScoreString;
    }
}
