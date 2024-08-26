using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private LevelButton _levelButtonPrefab;

    //can be got from this object
    [SerializeField] private GameObject _scrollMenu;
    [SerializeField] private Game _game;

    private List<LevelButton> _levelButtons = new List<LevelButton>();

    private void Start()
    {
        CreateLevelMenu(_game.LevelsNumber, _game.MaxLevel);
        _game.MaxLevelChanged += ActiveLevelsUpTo;
    }

    //private void OnEnable()
    //{
    //    _game.MaxLevelChanged += ActiveLevelsUpTo;
    //}

    //private void OnDisable()
    //{
    //    _game.MaxLevelChanged -= ActiveLevelsUpTo;
    //}
    
    private void CreateLevelMenu(int levels, int maxLevel)
    {
        for (int i = 0; i < levels; i++)
        {
            LevelButton levelButton = Instantiate(_levelButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity, _scrollMenu.transform);

            levelButton.SetLevelButton(i, _game, gameObject, (maxLevel >= i));
            _levelButtons.Add(levelButton);
        }
        //levelButton.gameObject.transform.SetParent(_scrollMenu);
    }

    private void ActiveLevelsUpTo(int maxLevel)
    {
        Debug.Log("---");
        for (int i = 0; i < _levelButtons.Count; i++)
        {
            if(i <= maxLevel && !_levelButtons[i].IsActive)
            {
                _levelButtons[i].SetActive(true);
            }
        }
    }
}
