using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.StartCue("Finale", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
