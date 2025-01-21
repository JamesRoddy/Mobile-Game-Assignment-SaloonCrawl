using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

  
    [SerializeField] private int maxAmount;
    [SerializeField] private int minAmount; 



    public int MaxAmount
    {
        get
        {
            return maxAmount;
        }

    }
    public int MinAmount
    {
        get
        {
            return minAmount;
        }

    }







}
