using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] PlayerRole _role = PlayerRole.CREWMATE;
    public float _detectRadius = 1.5f;

    public PlayerRole Role => _role;

    InputHandler _inputHandler;
    SpriteRenderer _spriteRenderer;
    bool _isFacingLeft;

    PlayerController _playerInRange = null;
    GameObject _taskInRange;


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

    public delegate void PlayerReportEvent(PlayerController reporter, PlayerController reported);
    public PlayerReportEvent PlayerCorpseFound;

    public event Action OnDeath;

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
        // Detect Task
        if (Role == PlayerRole.CREWMATE)
        {
            if (_taskInRange)
            {
                //TODO: task.DoTask;
                print(_taskInRange.name);
            }
        }

        // Detect Player
        if (_playerInRange != null)
        {
            if (_playerInRange.IsDead)
            {
                PlayerCorpseFound?.Invoke(this, _playerInRange);
            }

            if (Role == PlayerRole.IMPOSTOR)
            {
                if (_playerInRange.Role != PlayerRole.CREWMATE) { return; }
                _playerInRange.KillPlayer();
            }
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

        // Detect collisions every frame
        var colliders = Physics2D.OverlapCircleAll(transform.position, _detectRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player") && collider.gameObject != this.gameObject)
            {
                print(collider.name);
                _playerInRange = collider.GetComponent<PlayerController>();
                // set highlight to player in range to red
            }
            else if (collider.CompareTag("Task"))
            {
                _taskInRange = collider.gameObject;
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