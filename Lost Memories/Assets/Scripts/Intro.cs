using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeElapsed = Time.time - startTime;

        if(timeElapsed < 1) {
            // Black
        } else if(timeElapsed < 4) {
            // Fade in
            // TODO 
        } else {
            // Done
            SceneManager.LoadScene("Scene01_CA_Wraparound");
        }
    }
}
