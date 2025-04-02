using System;
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
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Determines whether the client is facing left.
    /// </summary>
    /// <remarks>
    /// The write permission is set to the Owner as we are only using this variable
    /// to update the face direction of this client in the network.
    /// </remarks>
    private NetworkVariable<bool> _isFacingLeft = new(writePerm: NetworkVariableWritePermission.Owner);

    /// <summary>
    /// Sets the value of <see cref="_isFacingLeft"/> based on the <see cref="_moveDirection"/>.
    /// </summary>
    /// <remarks>
    /// This setting of the variable only occurs locally.
    /// </remarks>
    private void SetFaceDirection()
    {
        if (_moveDirection.x < 0)
        {
            _isFacingLeft.Value = true;
        }
        else if (_moveDirection.x > 0)
        {
            _isFacingLeft.Value = false;
        }
    }

    /// <summary>
    /// Triggers when we set a new value for <see cref="_isFacingLeft">.
    /// </summary>
    /// <param name="previousValue"></param>
    /// <param name="newValue"></param>
    /// <remarks>
    /// This is where we adjust the spriteRenderer's flipX because this is synced across the network.
    /// </remarks>
    private void OnIsFacingLeftValueChanged(bool previousValue, bool newValue)
    {
        _spriteRenderer.flipX = newValue;
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
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _isFacingLeft.OnValueChanged += OnIsFacingLeftValueChanged;
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        _isFacingLeft.OnValueChanged -= OnIsFacingLeftValueChanged;
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