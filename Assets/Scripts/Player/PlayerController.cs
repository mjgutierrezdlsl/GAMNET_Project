using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] PlayerRole _role = PlayerRole.CREWMATE;

    public PlayerRole Role => _role;

    InputHandler _inputHandler;
    SpriteRenderer _spriteRenderer;
    bool _isFacingLeft;

    public static bool _isDead;

    public void SetRole(PlayerRole role) => _role = role;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        _inputHandler.OnDeadPress += DeadPressed;
    }
    private void OnDisable()
    {
        _inputHandler.OnDeadPress -= DeadPressed;
    }

    private void DeadPressed()
    {
        _isDead = true;
    }

    private void Update()
    {

        if (_isDead) return;

        transform.Translate(_inputHandler.Direction * _moveSpeed * Time.deltaTime);
        if (_inputHandler.Direction.x < 0)
        {
            _isFacingLeft = true;
        }
        else if (_inputHandler.Direction.x > 0)
        {
            _isFacingLeft = false;
        }
        _spriteRenderer.flipX = _isFacingLeft;
    }
}

public enum PlayerRole
{
    CREWMATE, IMPOSTOR
}