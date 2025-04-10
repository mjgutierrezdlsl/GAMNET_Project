using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    #region Role
    [Header("Role")]
    private NetworkVariable<PlayerRole> _role = new(PlayerRole.CREWMATE);
    public PlayerRole Role => _role.Value;

    public void SetRole(PlayerRole role) => _role.Value = role;
    #endregion

    #region State
    private NetworkVariable<PlayerState> _state = new(PlayerState.IDLE, writePerm: NetworkVariableWritePermission.Server);
    public PlayerState State => _state.Value;
    #endregion

    #region Detection
    [Header("Detection")]
    [SerializeField] private float _detectionRadius = 1.0f;
    [SerializeField] private LayerMask _playerLayer;
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
            case PlayerState.INTERACT:
                _animator.SetTrigger(_attackHashAnim);
                break;
            case PlayerState.DEAD:
                _animator.SetTrigger(_deadHashAnim);
                break;
        }
    }
    #endregion

    #region Events
    public delegate void PlayerReportEvent(PlayerController reporter, PlayerController reported);
    public event PlayerReportEvent CorpseFound;
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
        PlayerManager.Instance.AddPlayer(this);
        _state.OnValueChanged += OnStateChanged;
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        PlayerManager.Instance.RemovePlayer(this);
        _state.OnValueChanged -= OnStateChanged;
    }

    private void OnStateChanged(PlayerState previousValue, PlayerState newValue)
    {
        SetAnimationState(newValue);
    }

    private void Update()
    {
        _spriteRenderer.flipX = _isFacingLeft.Value;

        if (!IsOwner) { return; }
        if (State == PlayerState.DEAD) { return; }

        var colliders = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _playerLayer);
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D nearestPlayer = null;
            foreach (var collider in colliders)
            {
                if (collider.transform.root == transform) { continue; }
                nearestPlayer = collider;
                if (Vector3.Distance(transform.position, collider.transform.position) < Vector3.Distance(transform.position, nearestPlayer.transform.position))
                {
                    nearestPlayer = collider;
                }
            }

            if (nearestPlayer == null) { return; }

            if (nearestPlayer.TryGetComponent<PlayerController>(out var player))
            {
                switch (Role)
                {
                    case PlayerRole.CREWMATE:
                        if (player.State == PlayerState.DEAD)
                        {
                            CorpseFound?.Invoke(this, player);
                        }
                        break;
                    case PlayerRole.IMPOSTOR:
                        KillVictimRpc(player.OwnerClientId);
                        break;
                }
            }
        }

        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        UpdateStateRpc(_moveDirection != Vector2.zero ? PlayerState.MOVE : PlayerState.IDLE);
        SetFaceDirection();
    }

    private void FixedUpdate()
    {
        if (_state.Value == PlayerState.DEAD) { return; }
        _rigidbody.MovePosition(_rigidbody.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);
    }

    [Rpc(SendTo.Server)]
    public void UpdateStateRpc(PlayerState state)
    {
        _state.Value = state;
    }

    [Rpc(SendTo.Server)]
    public void KillVictimRpc(ulong clientId)
    {
        foreach (var player in PlayerManager.Instance.PlayerList)
        {
            if (player.OwnerClientId == clientId)
            {
                player.UpdateStateRpc(PlayerState.DEAD);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var system = IsServer ? "Server" : "Client";
        Handles.Label(transform.position, $"Client {OwnerClientId} ({system})");
#endif
    }
}