using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public Animator animator;
    playerController controller;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        animator = gameObject.GetComponent<Animator>();
        controller = FindObjectOfType<playerController>();
    }

    // Update is called once per frame
    public void Stop()
    { 
        Time.timeScale = 0f;
        controller.mainMusic.Pause();
        controller.runSound.Stop();
    }

    public void Play()
    {
        Time.timeScale = 1f;
        controller.mainMusic.Play();
        controller.runSound.Play();
        controller.deathChecker.isAlive = true;
    }
}
