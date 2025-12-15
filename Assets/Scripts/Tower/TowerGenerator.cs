using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    [Header("타워 스폰 위치들")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("타워 생성 규칙 (JSON → ScriptableObject)")]
    [SerializeField] private TowerGenerateData towerRuleData;  

    private TowerGenerate towerRule;


    private void Start()
    {
        towerRuleData.LoadData();

        towerRule = towerRuleData.GetTowerGenerateByKey(300);

        if (towerRule == null)
        {
            Debug.LogError("TowerGenerate rule not found!");
            return;
        }

        GenerateTowers();
    }



///
/// <summary>
/// 랜덤 생성
/// </summary>
    private void GenerateTowers()
    {
        foreach (Transform point in spawnPoints)
        {
            TowerType type = GetRandomTowerType();
            int tier = GetRandomTier();

            int towerKey = FindTowerKey(type, tier);

            if (towerKey == -1)
            {
                Debug.LogError($"No tower found for {type} Tier {tier}");
                continue;
            }

            SpawnTower(point.position, towerKey);
        }
    }



///
/// <summary>
/// 확률 기반 TowerType 선택
/// </summary>
    private TowerType GetRandomTowerType()
    {
        float r = Random.value;

        if (r < towerRule.MedievalRate)
            return TowerType.Medieval;

        if (r < towerRule.MedievalRate + towerRule.ModernRate)
            return TowerType.Modern;

        return TowerType.Fantasy;
    }


///
/// <summary>
/// 확률 기반 Tier 선택
/// </summary>

    private int GetRandomTier()
    {
        float r = Random.value;

        if (r < towerRule.Tier1Rate) 
            return 1;
        if (r < towerRule.Tier1Rate + towerRule.Tier2Rate) 
            return 2;
        if (r < towerRule.Tier1Rate + towerRule.Tier2Rate + towerRule.Tier3Rate) 
            return 3;

        return 4;
    }



///
/// <summary>
/// towerDictionary(JSON)에서 type+tier에 맞는 key 찾기
/// </summary>

    private int FindTowerKey(TowerType type, int tier)
    {
        Dictionary<int, Tower> towers = TowerManager.Instance.GetAllTowers();

        foreach (var kv in towers)
        {
            Tower t = kv.Value;

            if (t.TowerType == type && t.TowerTier == tier)
                return kv.Key;
        }

        return -1;
    }



/// <summary>
/// 타워 생성
/// </summary>
/// <param name="position">생성 위치</param>
/// <param name="towerKey">타워 키</param>
    private void SpawnTower(Vector3 position, int towerKey)
    {
        GameObject prefab = TowerManager.Instance.GetTowerPrefab(towerKey);

        if (prefab == null)
        {
            Debug.LogError($"Prefab not found for TowerKey {towerKey}");
            return;
        }

        GameObject go = Instantiate(prefab, position, Quaternion.identity);

        TowerObject towerObj = go.GetComponent<TowerObject>();
        towerObj.TowerKey = towerKey;
    }
}