using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    private Dictionary<int, Monster> monsterDictionary;

    /// <summary>
    /// 데이터 로드
    /// </summary>
    public void LoadData()
    {
        if (monsterDictionary == null)
        {
            monsterDictionary = DataParser.GetDataTable<Monster>(DataParser.MonsterTablePath);
        }
    }

    /// <summary>
    /// 키로 몬스터 데이터를 가져옵니다.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Monster GetMonsterByKey(int key)
    {
        if (monsterDictionary != null && monsterDictionary.ContainsKey(key))
        {
            return monsterDictionary[key];
        }
        else
        {
            Debug.LogError($"Monster with key {key} not found.");
            return null;
        }
    }

    /// <summary>
    /// 모든 몬스터 데이터를 가져옵니다.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, Monster> GetAllMonsters()
    {
        return monsterDictionary;
    }
}
