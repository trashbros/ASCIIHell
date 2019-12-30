using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPathStep
{
    public Vector2 MoveDirection;
    public float TimeDuration;
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