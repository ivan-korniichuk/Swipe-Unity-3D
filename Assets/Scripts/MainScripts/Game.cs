using System;
//
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;
    [SerializeField] private CameraController _cameraController;

    private int _levelNumber = -1;
    private Level _level = null;
    private PlayerSpawnBlock _nextLevelSpawnBlock = null;

    public static bool LevelStarted { get; private set; } = false;

    public int LevelsNumber {
        get
        {
            return _levels.Count();
        }
    }
    public int MaxLevel { get; private set; } = 0;

    public UnityAction<int> MaxLevelChanged;

    private void Awake()
    {
        _levels = GetComponentsInChildren<Level>(true).ToList();

        foreach(Level level in _levels)
        {
            level.gameObject.SetActive(true);
        }

        PrepareLevels();
    }

    private void PrepareLevels()
    {
        _levels[0].PrepareLevel(Vector3.zero);
        for (int i = 1; i < _levels.Count; i++)
        {
            _levels[i].PrepareLevel(_levels[i - 1].EndBlockPosition);
        }
    }

    private void RepositionLevels(int thisLevel, int newLevel)
    {
        if(thisLevel != newLevel)
        {
            _levels[newLevel].SetLevelPosition(_levels[thisLevel].SpawnBlockPosition);
            _levels[newLevel + 1].SetLevelPosition(_levels[newLevel].EndBlockPosition);
        }
    }

    private void OnEnable()
    {
        foreach (var level in _levels)
        {
            level.LevelStarted += PrepareLevel;
        }
    }

    private void OnDisable()
    {
        foreach (var level in _levels)
        {
            level.LevelStarted -= PrepareLevel;
        }
    }

    private void Start()
    {
        LoadData();
        SetLevel(_levelNumber);
    }

    private void PrepareLevel(bool started)
    {
        LevelStarted = started;
    }

    public bool Spawn()
    {
        return _level.SpawnBlock.Respawn();
    }

    //private void Restart()
    //{

    //}

    public void ChangeLevel(int levelNumber)
    {
        Debug.Log("Change Level");
        if(levelNumber < _levels.Count - 1)
        {
            RepositionLevels(_levelNumber, levelNumber);
        }

        Player player = _level.SpawnBlock.GetPlayer();
        SetLevel(levelNumber);
        if (player)
        {
            player.Restart(_level.SpawnBlockPosition);
            player.ChangeSpawnBlock(_level.SpawnBlock);
        }
    }

    private void SetLevel(int levelNumber)
    {
        Debug.Log("SetLevel");

        //is important
        if (_nextLevelSpawnBlock)
        {
            _nextLevelSpawnBlock.Reached -= NextLevel;
        }

        if ( _levels.Count - 1 >= levelNumber && _levelNumber != levelNumber || !_levels[levelNumber].IsActive)
        {
            if (_nextLevelSpawnBlock != null && _levelNumber + 1 != levelNumber)
            {
                _nextLevelSpawnBlock.OffBlock(1);
                //_level.SpawnBlock.Restart(_levels[levelNumber].SpawnBlockPosition);
            }

            if (_level)
            {
                _levels[levelNumber].SetLevel(_level.Blocks);
                if (_levelNumber != levelNumber)
                {
                    _level.SetActive(false);
                    _level.PlayerRespawned -= _cameraController.ChangePlayer;
                }
            }
            else
            {
                _levels[levelNumber].SetLevel();
            }
            _level = _levels[levelNumber];
            _level.PlayerRespawned += _cameraController.ChangePlayer;
            _levelNumber = levelNumber;

            if (levelNumber + 1 <= _levels.Count - 1)
            {
                _nextLevelSpawnBlock = _levels[levelNumber + 1].SetSpawnBlock();
                _nextLevelSpawnBlock.Reached += NextLevel;
            }

            _cameraController.SetPosition(_levels[_levelNumber].SpawnBlock.Position);

            if (MaxLevel < _levelNumber)
            {
                MaxLevel = _levelNumber;
                MaxLevelChanged?.Invoke(MaxLevel);
            }
            SaveData();
        }
    }

    public void OffLevel()
    {
        _levels[_levelNumber].OffLevel();
    }

    private void NextLevel()
    {
        Debug.Log("NExt Level");
        Debug.Log("_levelNumber + " + _levelNumber + " _levels.Count " + _levels.Count);
        if (_levelNumber + 2 < _levels.Count)
        {
            Debug.Log("True");
            _levels[_levelNumber + 2].SetLevelPosition(_levels[_levelNumber + 1].EndBlockPosition);
        }

        SetLevel(_levelNumber + 1);
        //SetLevelAndChangePlayersSpawnBlock(_levelNumber + 1);
    }

    private void SaveData()
    {
        Dictionary<string, bool[]> levelsDiamondsStatus = new Dictionary<string, bool[]>();
        foreach(Level level in _levels)
        {
            levelsDiamondsStatus.Add(level.name, level.DiamondStatus);
        }

        SaveGameData.SaveData(_levelNumber, Player.Hp, Player.Diamonds, MaxLevel, levelsDiamondsStatus);
    }

    public void RetundDataToDefault()
    {
        SaveGameData.SaveData(0, 3, 0, 0, new Dictionary<string, bool[]>());
        LoadData();
    }

    private void LoadData()
    {
        GameData data = SaveGameData.LoadData();

        if (data != null)
        {
            _levelNumber = data.Level;
            Player.SetLifes(data.Health);
            Player.SetDiamonds(data.Diamonds);
            MaxLevel = data.MaxLevel;
            MaxLevelChanged?.Invoke(MaxLevel);
            foreach (Level level in _levels)
            {
                if (data.LevelsDiamondsStatus.ContainsKey(level.name))
                {
                    level.SetDiamonds(data.LevelsDiamondsStatus[level.name]);
                }
            }
        }
        else
        {
            _levelNumber = 0;
            Player.SetLifes(3);
            Player.SetDiamonds(0);
            MaxLevel = 0;
        }
    }
}
