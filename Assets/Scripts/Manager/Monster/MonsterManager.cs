using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManager : Singleton<MonsterManager>
{
    public MonsterData monsterData;
    public MonsterWaveData monsterWaveData;
    public List<Transform> monsterPrefab;
    public Transform spawnPoint;
    public List<Transform> targetPoint;
    public float waveEndTimeInterval;
    public Action waveEndEvent;
    public Action<int> waveStartEvent;
    private Coroutine waveStartRoutine;
    public TMP_Text roundText;

    public AudioSource audioSource;

    private int currentWave;
    public int CurrentWave
    {
        get => currentWave;
        set
        {
            currentWave = value;
            roundText.gameObject.SetActive(true);
            roundText.SetText($"Round {value} Start");
            StartCoroutine(RoundTextAutoOff());
            StartWave();
        }
    }

    /// <summary>
    /// 몬스터 오브젝트 풀.
    /// </summary>
    // private Queue<MonsterObject> monsterQueue = new Queue<MonsterObject>();
    public List<MonsterObject> Monsters;

    private IEnumerator RoundTextAutoOff()
    {
        yield return new WaitForSeconds(5);

        roundText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 오브젝트 풀에서 가져오거나 생성합니다.
    /// </summary>
    /// <param name="count"></param>
    private void GenerateMonster(int monsterKey)
    {
        MonsterObject monster;
        Monster mData = monsterData.GetMonsterByKey(monsterKey);

        monster = Instantiate(monsterPrefab[mData.Name == "bear" ? 0 : 1]).GetComponent<MonsterObject>();

        Debug.Log(mData);

        monster.gameObject.SetActive(true);
        monster.MoveTarget = targetPoint[0];
        monster.MonsterKey = monsterKey;
        monster.transform.position = spawnPoint.position;
        monster.transform.parent = transform;
        Monsters.Add(monster);
    }

    /// <summary>
    /// 몬스터를 오브젝트 풀에 반환합니다.
    /// </summary>
    /// <param name="monster"></param>
    // public void EnqueueMonster(MonsterObject monster)
    // {
    //     monsterQueue.Enqueue(monster);
    // }

    /// <summary>
    /// 웨이브 시작
    /// </summary>
    /// <param name="count">몬스터 마릿수</param>
    /// <param name="interval">몬스터 생성 주기</param>
    public void StartWave()
    {
        waveStartEvent?.Invoke(CurrentWave);
        waveStartRoutine = StartCoroutine(WaveCoroutine(CurrentWave));
    }

    /// <summary>
    /// 웨이브 코루틴
    /// </summary>
    /// <param name="count">몬스터 마릿수</param>
    /// <param name="interval">몬스터 생성 주기</param>
    /// <returns></returns>
    private IEnumerator WaveCoroutine(int wave)
    {
        MonsterWave monsterWave = monsterWaveData.GetWaveData(wave);

        if (monsterWave == null)
        {
            // TODO end.
        }

        Debug.Log($"start wave : {monsterWave}");

        int idx = 0;

        foreach (int count in monsterWave.MonsterCounts)
        {
            for (int i = 0; i < count; ++i)
            {
                GenerateMonster(monsterWave.MonsterKey[idx]);
                yield return new WaitForSeconds(monsterWave.SpawnInterval);
            }
            idx += 1;
        }

        yield return new WaitForSeconds(waveEndTimeInterval);

        CurrentWave += 1;
        waveEndEvent?.Invoke();
    }

    public Transform GetNextTarget(Transform t)
    {
        int nextPos = targetPoint.IndexOf(t);
        nextPos = nextPos + 1 >= targetPoint.Count ? 0 : nextPos + 1;

        return targetPoint[nextPos];
    }

    /// <summary>
    /// 초기화
    /// </summary>
    private void Init()
    {
        Monsters = new List<MonsterObject>();
        monsterData?.LoadData();
        monsterWaveData?.LoadData();
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        CurrentWave = 1;
    }

    private void Update()
    {
        if (Monsters.Count == 0 && waveStartRoutine != null)
        {
            StopCoroutine(waveStartRoutine);
            CurrentWave += 1;
        }
    }
}
