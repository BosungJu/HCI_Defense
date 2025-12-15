using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    [Header("타워 스폰 위치들")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("타워 생성 규칙 (JSON → ScriptableObject)")]
    [SerializeField] private TowerGenerateData towerRuleData;

    private List<TowerObject> towers = new List<TowerObject>();

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

        for (int i = 0; i < spawnPoints.Count; ++i)
        {
            towers.Add(null);
        }
    }



///
/// <summary>
/// 랜덤 생성
/// </summary>
    public void GenerateTowers()
    {
        List<TowerObject> towers = this.towers.Where(t => t != null).ToList();

        int ranIdx = Random.Range(0, towers.Count);

        SpawnTower(spawnPoints[ranIdx].position, FindTowerKey(GetRandomTowerType(), GetRandomTier()), ranIdx);
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
    private void SpawnTower(Vector3 position, int towerKey, int idx)
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

        towers[idx] = towerObj;
    }
}
