using System;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    Animator _animator;
    InputHandler _inputHandler;
    PlayerController _playerController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _inputHandler = GetComponent<InputHandler>();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        _inputHandler.OnAttackPress += AttackPressed;
        _playerController.OnDeath += DeadPressed;
    }

    private void OnDisable()
    {
        _inputHandler.OnAttackPress -= AttackPressed;
        _playerController.OnDeath -= DeadPressed;
    }

    private void AttackPressed()
    {
        if (_playerController.IsDead) return;
        _animator.SetTrigger("attack");
    }

    private void DeadPressed()
    {
        _animator.SetTrigger("dead");
    }

    private void Update()
    {
        if (_playerController.IsDead) return;
        _animator.SetBool("isMoving", _inputHandler.Direction != Vector2.zero);
    }
}