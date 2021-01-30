using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    Interactable _objectToInteract;
    bool _inInteraction;
    [SerializeField]
    Animator _animator;
    [SerializeField]
    UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
        Animate();
    }

    private void Animate()
    {
        if (_inInteraction)
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
        if (_objectToInteract != null && Input.GetKeyDown(KeyCode.F) && !_uiManager.Interaction)
        {
            _inInteraction = true;
            _uiManager.CacheText(_objectToInteract.Scan());
            _uiManager.StartInteraction();
        }
        if (!_uiManager.Interaction)
        {
            _inInteraction = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with " + collision.name);
        _objectToInteract = collision.GetComponent<Interaction>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _objectToInteract = null;
        _inInteraction = false;
    }
}
