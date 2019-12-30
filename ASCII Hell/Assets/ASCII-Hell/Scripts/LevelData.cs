using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public WaveData[] Waves;
    public BossData Boss;
}

[System.Serializable]
public class WaveData
{
    public SpawnerData[] Spawners;
}

[System.Serializable]
public class SpawnerData
{
    public int Health;
    public int PointValue;
    public float Speed;
    public Vector2 StartingLocation;
    public string BulletPattern;
    public EnemyPathStep[] MovementPattern;
}

[System.Serializable]
public class BossData
{
    public int PointValue;
    public Vector2 StartingLocation;
    public BossPhase[] BossPhases;
}

[System.Serializable]
public class BossPhase
{
    public int StartingHealth;
    public string BulletPattern;
}