using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    const int WORDS_TO_GUESS = 8;
    private int wordsGuessed = 0;
    Interaction _objectToInteract;
    bool _inInteraction;
    bool _inNearbyInteraction;
    [SerializeField]
    Animator _animator;
    [SerializeField]
    UIManager _uiManager;
    private bool _inInput;
    private bool _inRemembered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_objectToInteract != null)
            Interact();
        Animate();
    }

    private void Animate()
    {
        if (_inInteraction || _inInput)
        {
            _animator.SetBool("Interaction", true);
        }
        else
        {
            _animator.SetBool("Interaction", false);
        }
    }

    private void Interact()
    {
        if (!_uiManager.Interaction && !_uiManager.Inputting)
        {
            if (_inNearbyInteraction)
            {
                _inNearbyInteraction = false;
                _inInteraction = false;
                _objectToInteract.AlreadySeen = true;
            }
            else if (_inInput)
            {
                string inputWord = _uiManager.GetInput();
                if (inputWord.ToUpper() == _objectToInteract.CorrectText.ToUpper())
                {
                    _objectToInteract.AlreadyGuessed = true;
                    _objectToInteract.AlreadySeen = true;
                    wordsGuessed++;
                    Debug.Log($"words remembered = {wordsGuessed}");
                    StartRememberedInteraction();
                }
                else
                {
                    StartWrongAnswerInteraction();
                }
                _inInput = false;
            }
            else
            {
                _inInteraction = false;
                _inRemembered = false;
            }
        }
        if (_objectToInteract != null && Input.GetKeyDown(KeyCode.F) && 
            !_objectToInteract.AlreadyGuessed && !_uiManager.Inputting)
        {
            _inInteraction = true;
            _uiManager.CacheText(_objectToInteract.Scan());
            _uiManager.StartInteraction();
            _objectToInteract.AlreadySeen = true;
        }
        else if(_objectToInteract != null && !_uiManager.Interaction && !_objectToInteract.AlreadySeen && 
            !_objectToInteract.AlreadyGuessed && !_uiManager.Inputting)
        {
            _inInteraction = true;
            _inNearbyInteraction = true;
            _uiManager.CacheText(_objectToInteract.Nearby());
            _uiManager.StartInteraction();
        }
        else if (_objectToInteract != null && /*!_uiManager.Interaction &&*/
            !_objectToInteract.AlreadyGuessed && !_uiManager.Inputting && Input.GetKeyDown(KeyCode.R))
        {
            _uiManager.StopInteraction();
            _uiManager.StartInput();
            _inInput = true;
        }
        else if (_objectToInteract != null && _objectToInteract.AlreadyGuessed && 
            !_uiManager.Inputting && Input.GetKeyDown(KeyCode.R))
        {
            _uiManager.StopInteraction();
            StartRememberedInteraction();
        }
       
    }

    private void StartWrongAnswerInteraction()
    {
        _inInteraction = true;
        _uiManager.CacheText(_objectToInteract.WrongAnswer());
        _uiManager.StartInteraction();
    }

    private void StartRememberedInteraction()
    {
        _inRemembered = true;
        _inInteraction = true;
        _uiManager.CacheText(_objectToInteract.Remember());
        _uiManager.StartInteraction();

        //AudioManager.instance.TriggerParameter("Reminiscence", AudioManager.instance.GetTimeToNextDownbeat(2), AudioManager.instance.GetBarDuration(), 4 * AudioManager.instance.GetBarDuration());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with " + collision.name);
        _objectToInteract = collision.GetComponent<Interaction>();

        AudioManager.instance.TriggerParameter("Discovery", 0, AudioManager.instance.GetTimeToNextDownbeat(2), 2 * AudioManager.instance.GetBarDuration());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _objectToInteract.AlreadySeen = false;
        _objectToInteract = null;
        _inInteraction = false;
        _uiManager.StopInteraction();
        _uiManager.StopInput();
        _inNearbyInteraction = false;
    }
}
