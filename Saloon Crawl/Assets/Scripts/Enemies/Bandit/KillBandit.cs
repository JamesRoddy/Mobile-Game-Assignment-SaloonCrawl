using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBandit : MonoBehaviour
{

    private DeathChecker banditDeath;
    private  InstaniateScorePopUp scorePopUp;
    // Start is called before the first frame update
   private void Start()
    {
        banditDeath = GetComponent<DeathChecker>();
        scorePopUp = GetComponent<InstaniateScorePopUp>();
    }

    // Update is called once per frame
    private  void Update()
    {
        isDead();
    }

    private void isDead()
    {
        if (banditDeath.isAlive == false)
        {
            scorePopUp.inistantiateScorePop(transform.position,Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
}
