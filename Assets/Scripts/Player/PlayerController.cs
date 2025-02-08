using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] PlayerRole _role = PlayerRole.CREWMATE;
    [SerializeField] float _detectRadius = 1.5f;

    public PlayerRole Role => _role;

    InputHandler _inputHandler;
    SpriteRenderer _spriteRenderer;
    bool _isFacingLeft;

    PlayerController _playerInRange;

    private bool _isDead;
    public bool IsDead
    {
        get => _isDead;
        set
        {
            _isDead = value;
            if (_isDead)
            {
                OnDeath?.Invoke();
            }
        }
    }

    public delegate void PlayerEvent();
    public event PlayerEvent OnDeath;

    public event Action<PlayerController> PlayerCorpseFound;

    public void SetRole(PlayerRole role) => _role = role;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        _inputHandler.OnAttackPress += AttackPressed;
    }
    private void OnDisable()
    {
        _inputHandler.OnAttackPress -= AttackPressed;
    }

    private void AttackPressed()
    {
        if (!_playerInRange) { return; }
        if (Role == PlayerRole.IMPOSTOR)
        {
            if (_playerInRange.Role != PlayerRole.CREWMATE) { return; }
            _playerInRange.KillPlayer();
        }
        else if (Role == PlayerRole.CREWMATE)
        {
            if (!_playerInRange.IsDead) { return; }
            PlayerCorpseFound?.Invoke(_playerInRange);
        }
    }

    public void KillPlayer()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        IsDead = true;
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

        var colliders = Physics2D.OverlapCircleAll(transform.position, _detectRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player") && collider.gameObject != this.gameObject)
            {
                print(collider.name);
                _playerInRange = collider.GetComponent<PlayerController>();
            }
            else
            {
                _playerInRange = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectRadius);
    }
}

public enum PlayerRole
{
    CREWMATE, IMPOSTOR
}