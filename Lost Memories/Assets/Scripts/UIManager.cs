using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject _dialogueBox;
    [SerializeField]
    Text _dialogueText;
    private bool _inInteraction;
    public bool Interaction
    {
        get 
        {
            return _inInteraction;
        }
    }
    string[] cache;
    int currentTextInd;

    public void CacheText(string[] texts)
    {
        cache = texts;
        currentTextInd = 0;
    }

    public void StartInteraction()
    {
        _dialogueBox.SetActive(true);
        _inInteraction = true;
        SetText();
    }

    private void SetText()
    {
        _dialogueText.text = cache[currentTextInd] + " [Press Enter.]";
        currentTextInd++;
    }

    private void Start()
    {
        StopInteraction();
    }
    private void Update()
    {

        if (_inInteraction && currentTextInd < cache.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SetText();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            StopInteraction();
        }
    }

    private void StopInteraction()
    {
        _inInteraction = false;
        _dialogueBox.SetActive(false);
        currentTextInd = 0;
    }
}
