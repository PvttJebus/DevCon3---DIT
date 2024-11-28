using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Scoredetection : MonoBehaviour
{
    public int score;
    public Text scoretxt;
    public int activePins;
    public Text activePinstxt;
    // Start is called before the first frame update
    private void Start()
    {
        activePins = 10;
        score = 0;

    }

    private void Update()
    {
        activePinstxt.text = "Pins Remaining: " + activePins;
        scoretxt.text = "Score: " + score;
    }

    private void OnTriggerExit(Collider other)
        {
        if (other.gameObject.CompareTag("Pin"))
        {
            


            score++;
            activePins--;



        }
    }
}

