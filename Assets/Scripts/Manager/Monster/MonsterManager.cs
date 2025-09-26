using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    public MonsterData monsterData;
    public MonsterWaveData monsterWaveData;
    public Transform monsterPrefab;
    public Transform spawnPoint;
    public Transform targetPoint;
    public float waveEndTimeInterval;
    public Action waveEndEvent;

    private int currentWave;
    public int CurrentWave
    {
        get => currentWave;
        set
        {
            currentWave = value;
            StartWave();
        }
    }

    /// <summary>
    /// 몬스터 오브젝트 풀.
    /// </summary>
    private Queue<MonsterObject> monsterQueue = new Queue<MonsterObject>();
    public List<MonsterObject> Monsters => new List<MonsterObject>(monsterQueue);

    /// <summary>
    /// 오브젝트 풀에서 가져오거나 생성합니다.
    /// </summary>
    /// <param name="count"></param>
    private void GenerateMonster(int monsterKey)
    {
        MonsterObject monster;

        if (monsterQueue.Count > 0)
        {
            monster = monsterQueue.Dequeue();
        }
        else
        {
            monster = Instantiate(monsterPrefab).GetComponent<MonsterObject>();
        }

        monster.gameObject.SetActive(true);
        monster.MoveTarget = targetPoint;
        monster.MonsterKey = monsterKey;
        monster.transform.position = spawnPoint.position;
        Monsters.Add(monster);
    }

    /// <summary>
    /// 몬스터를 오브젝트 풀에 반환합니다.
    /// </summary>
    /// <param name="monster"></param>
    public void EnqueueMonster(MonsterObject monster)
    {
        monsterQueue.Enqueue(monster);
    }

    /// <summary>
    /// 웨이브 시작
    /// </summary>
    /// <param name="count">몬스터 마릿수</param>
    /// <param name="interval">몬스터 생성 주기</param>
    public void StartWave()
    {
        StartCoroutine(WaveCoroutine(CurrentWave));
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

        for (int i = 0; i < monsterWave.MonsterCount; i++)
        {
            GenerateMonster(monsterWave.MonsterKey);
            yield return new WaitForSeconds(monsterWave.SpawnInterval);
        }

        yield return new WaitForSeconds(waveEndTimeInterval);

        waveEndEvent?.Invoke();
        CurrentWave += 1;
    }

    /// <summary>
    /// 초기화
    /// </summary>
    private void Init()
    {
        monsterData?.LoadData();
        monsterWaveData?.LoadData();
    }

    public void Awake()
    {
        Init();
        CurrentWave = 1;
    }
}
