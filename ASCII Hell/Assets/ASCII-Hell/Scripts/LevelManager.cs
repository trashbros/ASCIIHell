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

    private int m_currentWave = -1;

    private bool m_allWavesDefeated = false;
    private bool m_bossDefeated = false;

    private void Start()
    {
        Spawners = GetComponentsInChildren<Enemy>();
        PlayerObject = GetComponentInChildren<Player>();
        BossObject = GetComponentInChildren<Boss>();

        ClearLevel();

        CustomEvents.EventUtil.AddListener(CustomEventList.STOP_LEVEL, StopLevel);
        CustomEvents.EventUtil.AddListener(CustomEventList.START_LEVEL, StartLevel);

        //levelDetails = new LevelData(Resources.Load<TextAsset>("LevelLayout/Level01").text);

        /*
                foreach (var spawner in Spawners)
                {
                    spawner.gameObject.SetActive(false);
                }
                BossObject.gameObject.SetActive(false);


                levelDetails = new LevelData()
                {
                    Waves = new WaveData[1]
                    {
                        new WaveData()
                        {
                            Spawners = new SpawnerData[2]
                            {
                                new SpawnerData()
                                {
                                    Health = 1,
                                    PointValue = 5,
                                    Speed = 1,
                                    StartingLocation = new Vector2(0, 2),
                                    MovementPattern = new EnemyPathStep[2]
                                                {
                                        new EnemyPathStep()
                                        {
                                            MoveDirection = new Vector2(-.5f, -.5f),
                                            TimeDuration = 1
                                        },
                                        new EnemyPathStep()
                                        {
                                            MoveDirection = new Vector2(.5f, -.5f),
                                            TimeDuration = 1
                                        }
                                    }
                                },
                                new SpawnerData()
                                {
                                    Health = 1,
                                    PointValue = 5,
                                    Speed = 1,
                                    StartingLocation = new Vector2(2, 2),
                                    MovementPattern = new EnemyPathStep[2]
                                                {
                                        new EnemyPathStep()
                                        {
                                            MoveDirection = new Vector2(-.5f, -.5f),
                                            TimeDuration = 1
                                        },
                                        new EnemyPathStep()
                                        {
                                            MoveDirection = new Vector2(.5f, -.5f),
                                            TimeDuration = 1
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Boss = new BossData()
                    {
                        PointValue = 1000,
                        StartingLocation = new Vector2(0, 2),
                        BossPhases = new BossPhase[2]
                        {
                            new BossPhase()
                            {
                                StartingHealth = 5
                            },
                            new BossPhase()
                            {
                                StartingHealth = 3
                            }
                        }
                    }
                };
                */

        //InitializeLevel();
    }

    private void Update()
    {
    }

    public void StartLevel(CustomEvents.EventArgs evt)
    {
        levelDetails = new LevelData(Resources.Load<TextAsset>("LevelLayout/Level01").text);

        m_currentWave = -1;
        m_allWavesDefeated = false;
        m_bossDefeated = false;

        PlayerObject.Initialize(PlayerStart, PlayerHealth, PlayerSpeed);
        StartCoroutine(StartNextPhase());
    }

    public void ClearLevel()
    {
        foreach (var spawner in Spawners)
        {
            spawner.SetActive(false);
        }

        BossObject.SetActive(false);
        PlayerObject.SetActive(false);
    }

    public void StopLevel(CustomEvents.EventArgs evt)
    {
        ClearLevel();
    }

    private void InitializeLevel()
    {
        m_currentWave = -1;
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
        else if(m_bossDefeated)
        {
            CustomEvents.EventUtil.DispatchEvent(CustomEventList.LEVEL_COMPLETE);
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
        else if (m_bossDefeated)
        {
            StartCoroutine(CheckPhaseDone());
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

    private void OnDestroy()
    {
        CustomEvents.EventUtil.RemoveListener(CustomEventList.STOP_LEVEL, StopLevel);
        CustomEvents.EventUtil.RemoveListener(CustomEventList.START_LEVEL, StartLevel);
    }
}