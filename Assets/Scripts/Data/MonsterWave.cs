using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class MonsterWave : IDataKey
{
    [SerializeField] private int key;
    [JsonProperty("wave_number")][SerializeField] private int waveNumber;
    [JsonProperty("monster_keys")][SerializeField] private int monsterKey;
    [JsonProperty("spawn_interval")][SerializeField] private float spawnInterval;
    [JsonProperty("monster_count")][SerializeField] private int monsterCount;

    public int Key { get => key; set => key = value; }
    public int WaveNumber { get => waveNumber; private set => waveNumber = value; }
    public int MonsterKey { get => monsterKey; private set => monsterKey = value; }
    public float SpawnInterval { get => spawnInterval; private set => spawnInterval = value; }
    public int MonsterCount { get => monsterCount; private set => monsterCount = value; }

    [JsonConstructor]
    public MonsterWave(int key, int waveNumber, int monsterKey, float spawnInterval, int monsterCount)
    {
        Key = key;
        WaveNumber = waveNumber;
        MonsterKey = monsterKey;
        SpawnInterval = spawnInterval;
        MonsterCount = monsterCount;
    }
}
