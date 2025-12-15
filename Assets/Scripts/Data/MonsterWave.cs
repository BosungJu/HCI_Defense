using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class MonsterWave : IDataKey
{
    [SerializeField] private int key;
    [SerializeField] private int waveNumber;
    [SerializeField] private string monsterKeyString;
    [SerializeField] private string monsterCountString;
    [SerializeField] private float spawnInterval;

    public int Key { get => key; set => key = value; }
    [JsonProperty("wave_number")] public int WaveNumber { get => waveNumber; private set => waveNumber = value; }
    [JsonProperty("monster_keys")] public string MonsterKeyString { get => monsterKeyString; private set => monsterKeyString = value; }
    [JsonProperty("monster_count")] public string MonsterCountString { get => monsterCountString; private set => monsterCountString = value; }
    [JsonProperty("spawn_interval")] public float SpawnInterval { get => spawnInterval; private set => spawnInterval = value; }

    public List<int> MonsterKey = new List<int>();
    public List<int> MonsterCounts = new List<int>();

    [JsonConstructor]
    public MonsterWave(int wave_number, string monster_keys, string monster_count, float spawn_interval)
    {
        WaveNumber = wave_number;
        MonsterKeyString = monster_keys;
        MonsterCountString = monster_count;
        SpawnInterval = spawn_interval;

        InitializeData();
    }

    public void InitializeData()
    {
        string trimmed = MonsterKeyString.Trim('{', '}');
        string[] keys = trimmed.Split('/').Select(s => s.Trim('[', ']')).ToArray();
        foreach (string k in keys)
        {
            if (int.TryParse(k, out int key))
            {
                MonsterKey.Add(key);
            }
        }

        trimmed = MonsterCountString.Trim('{', '}');
        keys = trimmed.Split('/').Select(s => s.Trim('[', ']')).ToArray();
        foreach (string k in keys)
        {
            if (int.TryParse(k, out int key))
            {
                MonsterCounts.Add(key);
            }
        }

        Debug.Log($"Monster Key count : {MonsterKey.Count}");
        Debug.Log($"Monster counts : {MonsterCounts.Count}");
    }

    public override string ToString()
    {
        return $"Key {Key}, Wave Number : {WaveNumber}, Monster Keys : {MonsterKeyString}, Monster Count : {MonsterCountString}, Spawn Interval : {SpawnInterval}";
    }
}
