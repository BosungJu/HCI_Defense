using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    private Dictionary<int, Tower> towerDictionary;

    [Header("Tower Prefabs")]
    [SerializeField] private List<GameObject> towerPrefabs;


    private void Awake()
    {
        LoadTowerData();
    }


    private void LoadTowerData()
    {
        towerDictionary = DataParser.GetDataTable<Tower>(DataParser.TowerTablePath);

        if (towerDictionary == null)
            Debug.LogError("Failed to load TowerData JSON");
    }


    public Tower GetTowerByKey(int key)
    {
        if (towerDictionary.TryGetValue(key, out Tower tower))
            return tower;

        Debug.LogError($"Tower not found for key: {key}");
        return null;
    }


    public GameObject GetTowerPrefab(int key)
    {
        if (key < 0 || key >= towerPrefabs.Count)
        {
            Debug.LogError($"Tower prefab not found for key: {key}");
            return null;
        }

        return towerPrefabs[key];
    }


    public Dictionary<int, Tower> GetAllTowers()
    {
        return towerDictionary;
    }
}