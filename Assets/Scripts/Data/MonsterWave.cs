using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class MonsterWave : IDataKey
{
    [SerializeField] private int key;
    [JsonProperty("wave_number")][SerializeField] private int waveNumber;
    [JsonProperty("monster_keys")][SerializeField] private string monsterKeyString;
    [JsonProperty("spawn_interval")][SerializeField] private float spawnInterval;
    [JsonProperty("monster_count")][SerializeField] private string monsterCountString;

    public int Key { get => key; set => key = value; }
    public int WaveNumber { get => waveNumber; private set => waveNumber = value; }
    public string MonsterKeyString { get => monsterKeyString; private set => monsterKeyString = value; }
    public float SpawnInterval { get => spawnInterval; private set => spawnInterval = value; }
    public string MonsterCountString { get => monsterCountString; private set => monsterCountString = value; }

    public List<int> MonsterKey = new List<int>();
    public List<int> MonsterCounts = new List<int>();

    [JsonConstructor]
    public MonsterWave(int key, int waveNumber, string monsterKey, float spawnInterval, string monsterCount)
    {
        Key = key;
        WaveNumber = waveNumber;
        MonsterKeyString = monsterKey;
        SpawnInterval = spawnInterval;
        MonsterCountString = monsterCount;

        InitializeData();
    }

    public void InitializeData()
    {
        string trimmed = MonsterKeyString.Trim('{', '}');
        string[] keys = trimmed.Split('/');
        foreach (string k in keys)
        {
            if (int.TryParse(k, out int key))
            {
                MonsterKey.Add(key);
            }
        }

        trimmed = MonsterCountString.Trim('{', '}');
        keys = trimmed.Split('/');
        foreach (string k in keys)
        {
            if (int.TryParse(k, out int key))
            {
                MonsterCounts.Add(key);
            }
        }
    }
}
