using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Enemy[] Spawners;
    [SerializeField] private Player PlayerObject;
    [SerializeField] private Boss BossObject;

    [Header("Level Parameters")]
    [SerializeField] private Vector2 PlayerStart = Vector2.zero;
    [SerializeField] private int PlayerHealth = 3;
    [SerializeField] private float PlayerSpeed = 3;
    [SerializeField] private LevelData levelDetails;

    private int m_currentWave = 0;

    private bool m_allWavesDefeated = false;
    private bool m_bossDefeated = false;

    private void Start()
    {
        Spawners = GetComponentsInChildren<Enemy>();
        PlayerObject = GetComponentInChildren<Player>();
        BossObject = GetComponentInChildren<Boss>();

        InitializeLevel();
    }

    private void Update()
    {
    }

    private void InitializeLevel()
    {
        m_currentWave = 0;
        m_allWavesDefeated = false;
        m_bossDefeated = false;

        PlayerObject.Initialize(PlayerStart, PlayerHealth, PlayerSpeed);
        StartCoroutine(StartNextPhase());
    }

    private bool AllSpawnersDead()
    {
        foreach(var spawner in Spawners)
            {
                if( spawner.IsAlive)
                {
                    return false;
                }
            }
            return true;
    }

    #region Coroutines

    private IEnumerator CheckPhaseDone()
    {
        if(!m_allWavesDefeated)
        {
            while(!AllSpawnersDead())
            {
                yield return new WaitForSeconds(3);
            }

            StartCoroutine(StartNextPhase());
            yield break;
        }
        else if(!m_bossDefeated)
        {
            while(BossObject.IsAlive)
            {
                yield return new WaitForSeconds(3);
            }
            m_bossDefeated  = true;
            StartCoroutine(StartNextPhase());
            yield break;
        }
    }

    private IEnumerator StartNextPhase()
    {
        var waves = levelDetails.Waves;
        if(m_currentWave < waves.Length - 1)
        {
            m_currentWave++;
            StartCoroutine(PopulateWave());
            yield break;
        }
        else if (!m_bossDefeated)
        {
            m_allWavesDefeated = true;
            StartCoroutine(PopulateBoss());
            yield break;
        }
        else
        {
            // Generate a new level data set
            yield break;
        }
    }

    private IEnumerator PopulateWave()
    {
        var spawnerList = levelDetails.Waves[m_currentWave].Spawners;
        for(int i = 0; i < spawnerList.Length; i++)
        {
            Spawners[i].Initialize(spawnerList[i]);
        }
        StartCoroutine(CheckPhaseDone());
        yield break;
    }

    private IEnumerator PopulateBoss()
    {
        BossObject.Initialize(levelDetails.Boss);
        StartCoroutine(CheckPhaseDone());
        yield break;
    }

    #endregion
}