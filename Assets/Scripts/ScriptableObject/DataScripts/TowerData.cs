using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    private Dictionary<int, Tower> towerDictionary;

    /// <summary>
    /// 데이터 로드
    /// </summary>
    public void LoadData()
    {
        if (towerDictionary == null)
        {
            towerDictionary = DataParser.GetDataTable<Tower>(DataParser.TowerTablePath);
        }
    }

    /// <summary>
    /// 키로 타워 데이터를 가져옵니다.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Tower GetTowerByKey(int key)
    {
        if (towerDictionary != null && towerDictionary.ContainsKey(key))
        {
            return towerDictionary[key];
        }
        else
        {
            Debug.LogError($"Tower with key {key} not found.");
            return null;
        }
    }

    /// <summary>
    /// 모든 타워 데이터를 가져옵니다.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, Tower> GetAllTowers()
    {
        return towerDictionary;
    }
}
