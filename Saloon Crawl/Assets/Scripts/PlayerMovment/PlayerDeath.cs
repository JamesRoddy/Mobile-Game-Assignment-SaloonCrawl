using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathChecker : MonoBehaviour
{
    [SerializeField] private List<string> collsionTags;
    public bool isAlive = true;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Made contact with the enemy");

        foreach (string collsionTag in collsionTags)
        {
            if (collision.gameObject.CompareTag(collsionTag))
            {
                isAlive = false;
                Debug.Log("has died is " + isAlive);
                break;
            }
        }


    }



    public bool IsAlive
    {
        get { return isAlive; }
    }
}
