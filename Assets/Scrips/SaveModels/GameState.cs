using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{
    private WaveStatus _status = WaveStatus.Completed;
    public WaveStatus status
    {
        get
        {
            return _status;
        }
        set
        {
            _status = value;
            HudController.Instance.OnWaveStateChanged(_status);
        }
    }

    

    public float spawnTimer;
    public int pointsRemaining;
    public int currentWaveIndex = 0;
    public int Gold
    {
        get
        {
            return _Gold;
        }
        set
        {
            _Gold = value;
            HudController.Instance.UpdateGold(_Gold);
        }
    }
    [SerializeField]
    private int _Gold;
    public int lives
    {
        get
        {
            return _lives;
        }
        set
        {
            _lives = value;
            HudController.Instance.UpdateLives(_lives);
        }
    }
    [SerializeField]
    private int _lives;
}


public enum WaveStatus
{
    Spawning,
    Running,
    Completed
}
