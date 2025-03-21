using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    #region Role
    [Header("Role")]
    [SerializeField] private PlayerRole _role = PlayerRole.CREWMATE;
    public PlayerRole Role => _role;

    public void SetRole(PlayerRole role) => _role = role;
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    #endregion

    #region FaceDirection
    private bool _isFacingLeft;
    private void SetFaceDirection()
    {
        if (_moveDirection.x < 0)
        {
            _isFacingLeft = true;
        }
        else if (_moveDirection.x > 0)
        {
            _isFacingLeft = false;
        }
        transform.rotation = Quaternion.Euler(_isFacingLeft ? Vector3.up * 180f : Vector3.zero);
    }
    #endregion

    #region Animation
    private Animator _animator;
    private int _isMovingHashAnim = Animator.StringToHash("isMoving");
    private int _attackHashAnim = Animator.StringToHash("attack");
    private int _deadHashAnim = Animator.StringToHash("dead");

    private void SetAnimationState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                _animator.SetBool(_isMovingHashAnim, false);
                break;
            case PlayerState.MOVE:
                _animator.SetBool(_isMovingHashAnim, true);
                break;
            case PlayerState.ATTACK:
                _animator.SetTrigger(_attackHashAnim);
                break;
            case PlayerState.DEAD:
                _animator.SetTrigger(_deadHashAnim);
                break;
        }
    }
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsOwner) { return; }

        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        SetAnimationState(_moveDirection != Vector2.zero ? PlayerState.MOVE : PlayerState.IDLE);
        SetFaceDirection();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);
    }
}

public enum PlayerRole
{
    CREWMATE, IMPOSTOR
}

public enum PlayerState
{
    IDLE, MOVE, ATTACK, DEAD
}