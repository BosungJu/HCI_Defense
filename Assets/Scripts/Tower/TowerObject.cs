using UnityEngine;

public class TowerObject : MonoBehaviour
{
    private int towerKey;
    private float attackTimer;
    private MonsterObject currentTarget;

    [Header("Tower Runtime Data")]
    [SerializeField] private int damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int sellPrice;
    [SerializeField] private TowerType towerType;
    [SerializeField] private int towerTier;
    [SerializeField] private FantasyType fantasyType;

    public int TowerKey
    {
        get => towerKey;
        set
        {
            towerKey = value;
            InitializeTowerData();
        }
    }


    private void InitializeTowerData()
    {
        Tower tower = TowerManager.Instance.GetTowerByKey(TowerKey);
        if (tower == null)
        {
            Debug.LogError($"Tower data not found for key: {TowerKey}");
            return;
        }

        damage = tower.Damage;
        attackSpeed = tower.AttackSpeed;
        sellPrice = tower.SellPrice;
        towerType = tower.TowerType;
        towerTier = tower.TowerTier;
        fantasyType = tower.FantasyType;

        attackTimer = 1f / attackSpeed;
    }


    private void Update()
    {
        TowerTarget();
        if (currentTarget == null) return;

        LookAtTarget();

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            TowerAttack();
            attackTimer = 1f / attackSpeed;
        }
    }


    // ---------------------------------------
    //    Target Lock-On Logic
    // ---------------------------------------
    private void TowerTarget()
    {
        // 1) 현재 타겟이 살아 있으면 유지
        if (currentTarget != null && currentTarget.gameObject.activeSelf)
            return;

        // 2) 타겟이 없거나 죽었으면 → 새로운 타겟 검색
        currentTarget = FindClosestMonster();
    }


    private MonsterObject FindClosestMonster()
    {
        MonsterObject[] monsters = FindObjectsOfType<MonsterObject>();

        if (monsters.Length == 0)
            return null;

        float closestDist = Mathf.Infinity;
        MonsterObject closest = null;

        foreach (var m in monsters)
        {
            if (!m.gameObject.activeSelf) continue;

            float dist = Vector3.Distance(transform.position, m.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = m;
            }
        }

        return closest;
    }


    private void TowerAttack()
    {
        if (currentTarget == null) return;

        currentTarget.TakeDamage(damage);

        switch (fantasyType)
        {
            case FantasyType.Fire:
                currentTarget.ApplyBurn(damage * 0.2f, 3f);
                break;

            case FantasyType.Ice:
                currentTarget.ApplySlow(0.4f, 2f);
                break;
        }
    }


    private void LookAtTarget()
    {
        if (currentTarget == null) return;

        Vector3 dir = currentTarget.transform.position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir);
    }
}