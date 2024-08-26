using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnBlock : LongBlock
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private bool _isActive = false;

    private Player _player;
    private RaycastHit _hit;

    public UnityAction Reached;
    public UnityAction<Player> Respawned;

    private void OnEnable()
    {
        Player.ActionEnded += TryAddPlayer;
    }

    private void OnDisable()
    {
        Player.ActionEnded -= TryAddPlayer;
    }

    private void TryAddPlayer()
    {
        if (!_isActive)
        {
            Physics.Raycast(new Vector3(transform.position.x, LocalPosition.y, transform.position.z), Vector3.up, out _hit, 1);

            if (_hit.collider)
            {
                if (_hit.collider.TryGetComponent(out Player player))
                {
                    player.ChangeSpawnBlock(this);
                    Reached?.Invoke();
                }
            }
        }
    }

    public void SetPlayer(Player player)
    {
        _player = player;
        _isActive = true;
    }

    public void RevokePlayer()
    {
        _player = null;
        _isActive = false;
    }

    public Player GetPlayer()
    {
        _isActive = false;
        return _player;
    }

    public bool Respawn()
    {
        if (Player.Respawn())
        {
            Player newPlayer = Instantiate(_playerPrefab, LocalPosition + Vector3.up * 20, Quaternion.identity);
            newPlayer.Fall();
            newPlayer.ChangeSpawnBlock(this);
            Respawned?.Invoke(newPlayer);
            SetPlayer(newPlayer);
            return true;
        }
        return false;
    }

    public bool Restart(Vector3 spawnBlockPosition)
    {
        if (_player)
        {
            _player.Restart(spawnBlockPosition);
            return true;
        }
        return false;
    }
}
