using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBandit : MonoBehaviour
{

    private DeathChecker banditDeath;
    // Start is called before the first frame update
    void Start()
    {
        banditDeath = GetComponent<DeathChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        isDead();
    }

    private void isDead()
    {
        if (banditDeath.isAlive == false)
        {
            this.gameObject.SetActive(false);
        }
    }
}
