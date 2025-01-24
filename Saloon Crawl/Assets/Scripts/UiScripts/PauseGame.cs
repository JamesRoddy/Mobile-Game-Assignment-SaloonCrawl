using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Stop()
    { 
        Time.timeScale = 0f;
        

        //StartCoroutine(WaitStop());
    }

    IEnumerator WaitStop()
    {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0f;
    }

    public void Play()
    {
        Time.timeScale = 1f;
    }
}
