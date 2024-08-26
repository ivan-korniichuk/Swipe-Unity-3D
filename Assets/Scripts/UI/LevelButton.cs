using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private TMP_Text _textWindow;
    private Button _button;
    private GameObject _levelMenu;
    //private bool _isActive;
    //private int _levelNumber;

    public bool IsActive { get; private set; } = false;

    private void Awake()
    {
        _textWindow = GetComponentInChildren<TMP_Text>();
        _button = GetComponent<Button>();
    }

    public void SetLevelButton(int levelNumber, Game game, GameObject menu, bool isActive = false)
    {
        _textWindow.text = "Level " + levelNumber;
        _levelMenu = menu;
        _button.onClick.AddListener(() => {
            game.ChangeLevel(levelNumber);
            menu.SetActive(false);
        });
        SetActive(isActive);
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        if(isActive)
        {
            //_button.enabled = true;
            gameObject.SetActive(true);
        }
        else
        {
            //_button.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
