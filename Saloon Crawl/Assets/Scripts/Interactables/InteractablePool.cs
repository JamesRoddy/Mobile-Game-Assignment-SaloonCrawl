using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePool : MonoBehaviour
{


    private int maxSpawnCount = 0;
    private bool hasDeffered = false;
    private GameObject defferedInteractable;

    /* private int poolPointer = 0;*/
    private List<GameObject> pool = new List<GameObject>();
    private List<GameObject> active = new List<GameObject>();

    public void setValues(List<GameObject> interactables)
    {
        Debug.Log("setting values for interactables ");
        for (int i = 0; i < interactables.Count; i++)
        {
            GameObject interactable = Instantiate(interactables[i], Vector3.zero, Quaternion.identity);
            int count = interactable.GetComponent<Interactable>().NumberThatCanSpawn;
            interactable.SetActive(false);
            pool.Add(interactable);
            for (int j = 0; j < count - 1; j++)
            {
                GameObject interactableObj = Instantiate(interactable, Vector3.zero, Quaternion.identity);
                interactableObj.SetActive(false);
                pool.Add(interactableObj);
            }


        }
        maxSpawnCount = pool.Count;


    }

    public void updateActiveObjects()
    {
        Debug.Log("updating active objects ");
        for (int i = 0; i < active.Count; i++)
        {
     
            if (!(active[i].activeSelf))
            {
                pool.Add(active[i]);
                active.Remove(active[i]);
            }
        }





    }
    public GameObject getRandomAvailableObject()
    {

        if(hasDeffered && active.Count>0)
        {

            if (active[0].activeSelf == false)
            {
                Debug.Log(" SPAWNINNG INTERACTABLES  pool if no longer deffered found inactive object in active");
                hasDeffered = false;
                pool.Add(active[0]);
                active.Remove(active[0]);
                

                
            }



        }

        if (pool.Count > 0)
        {
            GameObject interactable;
            defferedInteractable = null;
            hasDeffered = false;
            Debug.Log("SPAWNING INTERACTABLES getting random interactable");
            interactable = pool[Random.Range(0, pool.Count)];
  
            active.Add(interactable);
            pool.Remove(interactable);
            Debug.Log("SPAWNING INTERACTABLES new active count after request  " + active.Count + "new pool count " + pool.Count);
            return interactable;
        }


        if (defferedInteractable == null)
        {
            Debug.Log(" SPAWNING INTERACTABLES pool has deffered ");


            defferedInteractable = active[Random.Range(0, active.Count)];
            defferedInteractable.GetComponent<Interactable>().DefferedSpawn = true;
        }


        return defferedInteractable;




    }

    public bool hasAvailableObject(TerrainType currentTerrain)
    {
        hasDeffered = pool.Count == 0 && !currentTerrain.hasMetInteractableRequest() ? true : false;
        if(hasDeffered)
        {
            Debug.Log("SPAWNING INTERACTABLES pool has deffered for type " + currentTerrain.GetClassification +" has deffered is true: "+ hasDeffered);
        }

        return pool.Count > 0 || hasDeffered;

    }

    public bool HasDeffered
    {
        get { return hasDeffered; }

    }
}

