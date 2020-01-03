using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[System.Serializable]
public class LevelData
{
    public WaveData[] Waves;
    public BossData Boss;

    public LevelData() { }

    public LevelData(string jsonString)
    {
        JSONNode jsonData = SimpleJSON.JSON.Parse(jsonString);
        List<WaveData> wavesList = new List<WaveData>();

        foreach(JSONNode wave in jsonData["Waves"])
        {
            wavesList.Add(new WaveData(wave));
        }

        Waves = wavesList.ToArray();

        Boss = new BossData(jsonData["Boss"]);
    }
}

[System.Serializable]
public class WaveData
{
    public SpawnerData[] Spawners;

    public WaveData() { }

    public WaveData(JSONNode waveInfo)
    {
        List<SpawnerData> spawnerList = new List<SpawnerData>();
        foreach (JSONNode spawner in waveInfo["Spawners"])
        {
            spawnerList.Add(new SpawnerData(spawner));
        }
        Spawners = spawnerList.ToArray();
    }
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

    public SpawnerData(){ }

    public SpawnerData(JSONNode spawnerInfo)
    {
        Health = spawnerInfo["Health"];
        PointValue = spawnerInfo["PointValue"];
        Speed = spawnerInfo["Speed"];
        StartingLocation = new Vector2(spawnerInfo["StartX"], spawnerInfo["StartY"]);
        BulletPattern = spawnerInfo["BulletPattern"];

        List<EnemyPathStep> patternList = new List<EnemyPathStep>();
        foreach (JSONNode patStep in spawnerInfo["MovementPattern"])
        {
            patternList.Add(new EnemyPathStep(patStep));
        }

        MovementPattern = patternList.ToArray();
    }
}

[System.Serializable]
public class EnemyPathStep
{
    public Vector2 MoveDirection;
    public float TimeDuration;

    public EnemyPathStep(){ }

    public EnemyPathStep(JSONNode pathInfo)
    {
        MoveDirection = new Vector2(pathInfo["MoveDirX"], pathInfo["MoveDirY"]);
        TimeDuration = pathInfo["TimeDuration"];
    }
}

[System.Serializable]
public class BossData
{
    public int PointValue;
    public Vector2 StartingLocation;
    public BossPhase[] BossPhases;

    public BossData() { }

    public BossData(JSONNode bossInfo)
    {
        PointValue = bossInfo["PointValue"];
        StartingLocation = new Vector2(bossInfo["StartX"], bossInfo["StartY"]);

        List<BossPhase> bossList = new List<BossPhase>();
        foreach (JSONNode bossPhase in bossInfo["BossPhases"])
        {
            bossList.Add(new BossPhase(bossPhase));
        }

        BossPhases = bossList.ToArray();
    }
}

[System.Serializable]
public class BossPhase
{
    public int StartingHealth;
    public string BulletPattern;

    public BossPhase() { }

    public BossPhase(JSONNode phaseInfo)
    {
        StartingHealth = phaseInfo["StartingHealth"];
        BulletPattern = phaseInfo["BulletPattern"];
    }
}

public static class EnemyPathReader
{
    public static EnemyPathStep[] ReadPathData(string pathData)
    {
        string[] lines = pathData.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        List<EnemyPathStep> pathSteps = new List<EnemyPathStep>();
        foreach (var line in lines)
        {
            var vals = line.Split(',');
            EnemyPathStep step = new EnemyPathStep()
            {
                MoveDirection = new Vector2(float.Parse(vals[0]), float.Parse(vals[1])),
                TimeDuration = float.Parse(vals[2])
            };
            pathSteps.Add(step);
        }
        return pathSteps.ToArray();
    }
}