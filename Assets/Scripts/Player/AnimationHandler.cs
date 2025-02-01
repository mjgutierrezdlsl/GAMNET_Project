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
    private void Update()
    {
        _animator.SetBool("isMoving", _inputHandler.Direction != Vector2.zero);
    }
}