﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        if(Input.anyKeyDown) {
            SceneManager.LoadScene("Intro");
        }
    }
}
