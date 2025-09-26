using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterWaveData", menuName = "Scriptable Objects/MonsterWaveData")]
public class MonsterWaveData : ScriptableObject
{
    private Dictionary<int, MonsterWave> monsterWaveDictionary;

    public void LoadData()
    {
        if (monsterWaveDictionary == null)
        {
            monsterWaveDictionary = DataParser.GetDataTable<MonsterWave>(DataParser.MonsterWaveTablePath);
        }
    }

    public MonsterWave GetWaveData(int waveNumber)
    {
        return monsterWaveDictionary.Where(kv => kv.Value.WaveNumber == waveNumber).Select(kv => kv.Value).FirstOrDefault();
    }

    public MonsterWave GetWaveDataByKey(int key)
    {
        if (monsterWaveDictionary.TryGetValue(key, out MonsterWave wave))
        {
            return wave;
        }
        Debug.LogWarning($"MonsterWave with key {key} not found.");
        return null;
    }
}
