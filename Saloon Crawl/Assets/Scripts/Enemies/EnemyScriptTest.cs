using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptTest1 : EnemyDescriptorInfo// all enemies inherit from this class that acts as a middle man between the enemy spawner and the enemy providing certain information about the enemy like spawn interval
{


    // example class for enemy  

   
    BoxCollider2D boxCollider2D;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        // spawn interval will be set in editor this is here as an example
        /*SpawnInterval = 0.0f*/; // each enemy can have its own spawn interval i.e the zombie walks from right to left when the player is in the desert 
        // zombies will be spawned in intervals rather than immideatley where as cowboys wont have an interval at all  
        // terrain types have there own methods of spawing that do or dont take into account this interval so ultimatley the spawn method is set by the terrain type
        // for example the saloon has no interval as the enemies are just static turrets SPAWN INTERVAK CAN BE CHANGED IN CODE JUST DEFINE IT IN EDITOR FIRST
        Debug.Log("started enemy script 1 ");
    }

    void Update()
    {
        
    }
}
