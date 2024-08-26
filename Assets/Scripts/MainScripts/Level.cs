using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Block> _blocks;
    [SerializeField] private Vector3 _endBlockPosition;
    [SerializeField] private List<Diamond> _diamonds;

    private float _setTime = 1;
    private float _random = 0.3f;
    private bool _isActive = false;
    private PlayerSpawnBlock _spawnBlock;

    public List<Block> Blocks => _blocks;
    public UnityAction<bool> LevelStarted;
    public UnityAction<Player> PlayerRespawned;
    public PlayerSpawnBlock SpawnBlock => _spawnBlock;
    public bool IsActive => _isActive;
    public bool[] DiamondStatus => _diamonds.Select(diamond => diamond.IsPickedUp).ToArray();

    // I need a variable for random time;

    public Vector3 EndBlockPosition
    { 
        get 
        { 
            return _endBlockPosition + transform.position; 
        } 
    }

    public Vector3 SpawnBlockPosition
    {
        get
        {
            return _spawnBlock.LocalPosition;
        }
    }


    private void Awake()
    {
        _blocks = GetComponentsInChildren<Block>(true).ToList();
        foreach (var block in _blocks)
        {
            block.PositionChanged += StartLevel;
            if (block.TryGetComponent(out PlayerSpawnBlock spawnBlock))
            {
                _spawnBlock = spawnBlock;
                _spawnBlock.Respawned += RestartLevel;
            }
        }
    }

    private void StartLevel()
    {
        bool active = true;
        foreach (var block in _blocks)
        {
            if (block.Started == false && !block.TryGetComponent(out Diamond _))
            {
                active = false;
            }
        }
        if (active)
        {
            _isActive = true;
            LevelStarted?.Invoke(true);
        }
    }

    public void SetLevelPosition(Vector3 startPosition)
    {
        Vector3 newPositionDistance = startPosition - new Vector3(_spawnBlock.transform.position.x, _spawnBlock.LocalPosition.y, _spawnBlock.transform.position.z);
        transform.position += newPositionDistance;
        foreach (var block in _blocks)
        {
            block.RepositionBlock(newPositionDistance);
        }
    }

    public void PrepareLevel(Vector3 startPosition)
    {
        transform.position = startPosition - new Vector3(_spawnBlock.transform.position.x, _spawnBlock.LocalPosition.y, _spawnBlock.transform.position.z);
        foreach (var block in _blocks)
        {
            block.PrepareBlock();
        }
    }

    private void RestartLevel(Player player)
    {
        PlayerRespawned?.Invoke(player);
        Restart();
    }

    private void Restart()
    {
        foreach (var block in _blocks)
        {
            if (block.TryGetComponent(out MovableBlock movableBlock))
            {
                Debug.Log("Restart LEVEL " + movableBlock.name);
                movableBlock.gameObject.SetActive(true);
                LevelStarted?.Invoke(false);
                movableBlock.Restart(Random.Range(_setTime - _random, _setTime + _random));
            }
        }
    }

    public void SetLevel(List <Block> oldBlocks = null)
    {
        if (oldBlocks != null)
        {
            LevelStarted?.Invoke(false);
            foreach (var block in _blocks)
            {
                foreach (var oldBlock in oldBlocks)
                {
                    if (oldBlock.LocalPosition == block.LocalPosition && oldBlock.BlockType == block.BlockType && oldBlock.IsActive)
                    {
                        block.gameObject.SetActive(true);
                        block.Set();
                        oldBlock.Off(false);
                    }
                }
            }

            foreach (var oldBlock in oldBlocks)
            {
                if (oldBlock.TryGetComponent(out SandBlock sandBlock))
                {
                    sandBlock.Restart();
                }
                if (oldBlock.IsActive)
                {
                    if (oldBlock.TryGetComponent(out PortableBlock portableBlock))
                    {
                        portableBlock.Restart();
                    }
                    oldBlock.OffBlock(Random.Range(_setTime - _random, _setTime + _random));
                }
            }
        }

        foreach (var block in _blocks)
        {
            if (block)
            {
                if (!block.IsActive)
                {
                    if(block.TryGetComponent(out Diamond diamond))
                    {
                        if(diamond.IsPickedUp == false)
                        {
                            block.gameObject.SetActive(true);
                            block.StartBlock(Random.Range(_setTime - _random, _setTime + _random));
                        }
                    }
                    //else if (block.TryGetComponent(out MovableBlock movableBlock))
                    //{
                    //    Debug.Log("111");
                    //    movableBlock.gameObject.SetActive(true);
                    //    movableBlock.Restart(Random.Range(_setTime - _random, _setTime + _random));
                    //}
                    else
                    {
                        block.gameObject.SetActive(true);
                        block.StartBlock(Random.Range(_setTime - _random, _setTime + _random));
                    }
                }
            }
        }
    }

    public PlayerSpawnBlock SetSpawnBlock()
    {
        _spawnBlock.gameObject.SetActive(true);
        _spawnBlock.SetBlock(Random.Range(_setTime - _random, _setTime + _random));
        return _spawnBlock;
    }

    public void OffLevel()
    {
        if (_isActive)
        {
            foreach (var block in _blocks)
            {
                if (block)
                {
                    if (block.IsActive)
                    {
                        block.OffBlock(Random.Range(_setTime - _random, _setTime + _random));
                    }
                }
            }
            _isActive = false;
        }
    }

    public void SetActive(bool active)
    {
        _isActive = active;
    }

    public void SetDiamonds(bool[] diamondsStatus)
    {
        for (int i = 0; i < diamondsStatus.Length; i++)
        {
            if (diamondsStatus[i])
            {
                _diamonds[i].PickUp();
            }
        }
    }
}
