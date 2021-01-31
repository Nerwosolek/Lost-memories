﻿using System;
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
    [SerializeField]
    GameObject _inputBox;
    [SerializeField]
    InputField _inputField;
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
    private bool _inInput;
    public bool Inputting { get { return _inInput;  } }

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

    public void ShowDialogueBox(bool show = true)
    {
        _dialogueBox.SetActive(show);
    }

    public void SetText(string text = null, bool showEnterPrompt = true)
    {
        if(text == null) {
            if(currentTextInd < cache.Length) {
                text = cache[currentTextInd] + (showEnterPrompt ? " [Press Enter.]" : "");
                currentTextInd++;
            }
        }

        _dialogueText.text = text;
    }

    private void Start()
    {
        StopInteraction();
        StopInput();
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
        else if (_inInput && Input.GetKeyDown(KeyCode.Return))
        {
            StopInput();
            _inInput = false;
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            StopInteraction();
        }
    }

    public void StopInteraction()
    {
        _inInteraction = false;
        _dialogueBox.SetActive(false);
        currentTextInd = 0;
    }

    public void StartInput()
    {
        _inputBox.SetActive(true);
        _inInput = true;
    }

    public string GetInput()
    {
        return _inputField.text;
    }

    public void StopInput()
    {
        _inputBox.SetActive(false);
        _inInput = false;
    }
}
