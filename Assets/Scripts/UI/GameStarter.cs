using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Game _game;
    [SerializeField] private List<UIComponent> _defaultComponents;
    [SerializeField] private List<UIComponent> _startedComponents;

    private bool _playerSpawned = false;

    private void Start()
    {
        foreach (var component in _startedComponents)
        {
            component.TurnOff();
        }
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(StartGame);
        Player.PlayerDied += SetDefaultMenu;
        Player.PlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(StartGame);
        Player.PlayerDied -= SetDefaultMenu;
        Player.PlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        _playerSpawned = false;
    }

    public void SetDefaultMenu()
    {
        _button.enabled = true;
        foreach (var component in _defaultComponents)
        {
            component.TurnOn(0.3f);
        }
        foreach (var component in _startedComponents)
        {
            component.TurnOff(0.1f);
        }
    }

    public void SetGameMenu()
    {
        _button.enabled = false;
        foreach (var component in _defaultComponents)
        {
            if (component.gameObject.activeSelf)
            {
                component.TurnOff(0.1f);
            }
        }
        foreach (var component in _startedComponents)
        {
            if (component.gameObject.activeSelf)
            {
                component.TurnOn(0.3f);
            }
        }
    }

    private void StartGame()
    {
        if (_playerSpawned)
        {
            SetGameMenu();
        }
        else if (_game.Spawn())
        {
            _playerSpawned = true;
            SetGameMenu();
        }
    }
}
