﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _velocity;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private CharacterController _controller;
    int jumps;
    [SerializeField]
    private float _jumpVelocity;
    private float _gravity;
    // Start is called before the first frame update
    Animator _animator;
    Interactable _objectToInteract;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _gravity = -Physics2D.gravity.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        //grounded = _controller.isGrounded;
        _velocity.x = Input.GetAxis("Horizontal") * _speed;
        if (_controller.isGrounded)
        {
            jumps = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y += _jumpVelocity;
                jumps++;
            }
            if (_velocity.y < 0) _velocity.y = 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumps < 2)
        {
            _velocity.y += _jumpVelocity;
            jumps++;
        }
        _velocity.y -= _gravity * Time.deltaTime;
        _controller.Move(new Vector3(_velocity.x, _velocity.y) * Time.deltaTime);
        Animate();
    }

    private void Animate()
    {
        if (_velocity.x < -float.Epsilon)
        {
            _animator.SetBool("LeftKey", true);
            _animator.SetBool("RightKey", false);
        }
        else if (_velocity.x > float.Epsilon)
        {
            _animator.SetBool("LeftKey", false);
            _animator.SetBool("RightKey", true);
        }
        else
        {
            _animator.SetBool("LeftKey", false);
            _animator.SetBool("RightKey", false);
        }
    }
}
