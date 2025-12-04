using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerGenerateData", menuName = "Scriptable Objects/TowerGenerateData")]
public class TowerGenerateData : ScriptableObject
{
    private Dictionary<int, TowerGenerate> towerGenerateDictionary;

    /// <summary>
    /// 데이터 로드
    /// </summary>
    public void LoadData()
    {
        if (towerGenerateDictionary == null)
        {
            towerGenerateDictionary = DataParser.GetDataTable<TowerGenerate>(DataParser.TowerGenerateTablePath);
        }
    }

    /// <summary>
    /// 키로 타워 데이터를 가져옵니다.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TowerGenerate GetTowerGenerateByKey(int key)
    {
        if (towerGenerateDictionary != null && towerGenerateDictionary.ContainsKey(key))
        {
            return towerGenerateDictionary[key];
        }
        else
        {
            Debug.LogError($"TowerGenerate with key {key} not found.");
            return null;
        }
    }

    /// <summary>
    /// 모든 타워 데이터를 가져옵니다.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, TowerGenerate> GetAllTowerGenerates()
    {
        return towerGenerateDictionary;
    }
}
