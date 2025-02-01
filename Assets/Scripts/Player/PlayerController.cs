using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputHandler _inputHandler;
    [SerializeField] float _moveSpeed = 2f;
    SpriteRenderer _spriteRenderer;
    bool _isFacingLeft;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
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