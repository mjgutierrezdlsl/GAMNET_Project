using System;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    Animator _animator;
    InputHandler _inputHandler;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _inputHandler = GetComponent<InputHandler>();
    }

    private void OnEnable()
    {
        _inputHandler.OnAttackPress += AttackPressed;
        _inputHandler.OnDeadPress += DeadPressed;
    }

    private void OnDisable()
    {
        _inputHandler.OnAttackPress -= AttackPressed;
        _inputHandler.OnDeadPress -= DeadPressed;
    }

    private void AttackPressed()
    {
        if (PlayerController._isDead) return;
        _animator.SetTrigger("attack");
    }

    private void DeadPressed()
    {
        _animator.SetTrigger("dead");
    }

    private void Update()
    {
        if (PlayerController._isDead) return;
        _animator.SetBool("isMoving", _inputHandler.Direction != Vector2.zero);
    }
}