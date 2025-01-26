using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBandit : MonoBehaviour
{

    private DeathChecker banditDeath;
    private  InstaniateScorePopUp scorePopUp;
    private EnemyDescriptorInfo enemyDescriptorInfo;
    private playerController controller;
    private bool hasDied = false;
       // Start is called before the first frame update
   private void Start()
    {
        banditDeath = GetComponent<DeathChecker>();
        scorePopUp = GetComponent<InstaniateScorePopUp>(); 
        enemyDescriptorInfo = GetComponent<EnemyDescriptorInfo>();
    }

    // Update is called once per frame
    private  void Update()
    {
        
            isDead();
        
    
    }

    private void isDead()
    {
       /* if (banditDeath.isAlive == false &&!hasDied)
        {
            hasDied = true;
            scorePopUp.inistantiateScorePop(transform.position,Quaternion.identity);
            controller.CurrentScore += enemyDescriptorInfo.ScoreIncrement;
            controller.conactToScore(Convert.ToString(enemyDescriptorInfo.ScoreIncrement));
            this.gameObject.SetActive(false);
        }*/
    }
}
