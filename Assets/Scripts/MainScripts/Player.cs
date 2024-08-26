using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    [SerializeField] private float _jumpTime;
    [SerializeField] private LayerMask _staticBlocks;
    [SerializeField] private LayerMask _movableBlocks;

    private PlayerSpawnBlock _spawnBlock;
    private PlayerInput _playerInput;
    private static int _diamonds = 0;
    private static int _hp = 3;

    public readonly static int LimitHp = 3;
    public static UnityAction PlayerDied;
    public static UnityAction<int> DiamondsChanged;
    public static UnityAction<int> HpChanged;
    public static int Hp => _hp;
    public static int Diamonds => _diamonds;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += ctx => OnMove();
        _playerInput.Player.Click.performed += ctx => OnClick();
    }

    private void Start()
    {
        DiamondsChanged.Invoke(_diamonds);
        HpChanged?.Invoke(_hp);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        if (!Game.LevelStarted)
            return;


        if (IsFalling && transform.position.y <= -1)
        {
            PlayerDied?.Invoke();
            Destroy(this);
        }
        if (IsMoving != CoroutineIsActive)
        {
            IsMoving = CoroutineIsActive;
            if (!IsMoving)
            {
                if (!Physics.Raycast(transform.position, Vector3.down, 1))
                {
                    Fall();
                }
            }
        }

        if (Physics.Raycast(transform.position, Vector3.down, 0.5f, _staticBlocks) && IsFalling)
        {
            Fall(false);
            transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Diamond diamond))
        {
            PickUpDiamond(diamond);
        }
    }

    public static bool Respawn()
    {
        if (_hp >= 1)
        {
            _hp--;
            HpChanged?.Invoke(_hp);
            return true;
        }
        return false;
    }

    //add new position
    public void Restart(Vector3 position)
    {
        StartCoroutine(RestartCoroutine(position));
    }

    private IEnumerator RestartCoroutine(Vector3 respawnPosition)
    {
        Debug.Log("Restart Coroutine");
        Fall(false);
        Vector3 newPosition = new Vector3(transform.position.x, 20, transform.position.z);
        MoveCoroutine = TryMoveBlockCorutine(newPosition, 1);
        StartCoroutine(MoveCoroutine);
        yield return new WaitUntil(() => !CoroutineIsActive);

        respawnPosition.y = transform.position.y;
        transform.position = respawnPosition;
        Fall();
    }

    public static bool BuyTheme(Theme theme)
    {
        if (theme.Price <= _diamonds)
        {
            _diamonds -= theme.Price;
            DiamondsChanged?.Invoke(_diamonds);
            return true;
        }
        return false;
    }

    public static void GiveLifes(int lifes)
    {
        _hp += lifes;
        HpChanged?.Invoke(_hp);
    }

    //dangerous;
    public static void SetLifes(int lifes)
    {
        _hp = lifes;
        HpChanged?.Invoke(_hp);
    }

    public static void BuyLife(int lifes)
    {
        if(_diamonds >= lifes)
        {
            _diamonds -= lifes;
            _hp += lifes;
            HpChanged?.Invoke(_hp);
            DiamondsChanged?.Invoke(_diamonds);
        }
    }

    //dangerous;
    public static void AddDiamonds(int diamonds)
    {
        _diamonds += diamonds;
        DiamondsChanged?.Invoke(_diamonds);
    }

    //dangerous;
    public static void SetDiamonds(int diamonds)
    {
        _diamonds = diamonds;
        DiamondsChanged?.Invoke(_diamonds);
    }

    private void PickUpDiamond(Diamond diamond)
    {
        diamond.PickUp();
        _diamonds++;
        DiamondsChanged?.Invoke(_diamonds);
    }

    private void OnClick()
    {
    }

    private void OnMove()
    {
        if (!IsMoving && !IsFalling && Game.LevelStarted)
        {
            Vector2 move = _playerInput.Player.Move.ReadValue<Vector2>();
            if (move.x <= -0.7)
            {
                TryMove(new Vector3(0, 0, 1), _jumpTime);
            }
            if (move.x >= 0.7)
            {
                TryMove(new Vector3(0, 0, -1), _jumpTime);
            }
            if (move.y >= 0.7)
            {
                TryMove(new Vector3(1, 0, 0), _jumpTime);
            }
            if (move.y <= -0.7)
            {
                TryMove(new Vector3(-1, 0, 0), _jumpTime);
            }
        }
    }

    public void ChangeSpawnBlock(PlayerSpawnBlock playerSpawnBlock)
    {
        Debug.Log("ChangeSpawnBlock");
        if (_spawnBlock)
        {
            playerSpawnBlock.SetPlayer(_spawnBlock.GetPlayer());
            _spawnBlock.RevokePlayer();
        }
        _spawnBlock = playerSpawnBlock;
    }
}
