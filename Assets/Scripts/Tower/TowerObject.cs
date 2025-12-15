using System.Collections;
using System.Linq;
using Oculus.Interaction.Input;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    private int towerKey;
    private float attackTimer;

    private MonsterObject currentTarget;
    [SerializeField] private MonsterObject debugTarget; // Debug 보기용

    [Header("Possession Controller")]
    public Controller controller;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Tower Runtime Data")]
    [SerializeField] private int damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int sellPrice;
    [SerializeField] private TowerType towerType;
    [SerializeField] private int towerTier;
    [SerializeField] private FantasyType fantasyType;

    public AnimationClip animation;

    public int TowerKey
    {
        get => towerKey;
        set
        {
            towerKey = value;
            InitializeTowerData();
        }
    }

    // --------------------------------------------------
    //      데이터 초기화
    // --------------------------------------------------
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

        StartCoroutine(ShootProjectileToMonster());
    }

    // --------------------------------------------------
    // 타겟 갱신 
    // --------------------------------------------------
    private void UpdateTarget()
    {
        if (currentTarget != null &&
            currentTarget.gameObject.activeSelf &&
            currentTarget.Health > 0)
        {
            return;
        }

        currentTarget = FindClosestMonster();
    }


    private MonsterObject FindClosestMonster()
    {
        float closestDist = Vector3.Distance(transform.position, MonsterManager.Instance.Monsters[0].transform.position);
        MonsterObject closest = MonsterManager.Instance.Monsters[0];

        foreach (var m in MonsterManager.Instance.Monsters)
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

    private IEnumerator ShootProjectileToMonster()
    {
        while (true) 
        {
            UpdateTarget();

            if (!projectilePrefab || !firePoint || currentTarget == null) 
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            LookAtTarget();

            GameObject obj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile proj = obj.GetComponent<Projectile>();

            proj.target = currentTarget;
            proj.damage = damage;

            // 🔥 판타지 타입 설정
            if (fantasyType == FantasyType.Fire)
            {
                proj.isAOE = true;
                proj.isFire = true;
                proj.aoeRadius = 2.3f;
            }
            else if (fantasyType == FantasyType.Ice)
            {
                proj.isAOE = true;
                proj.isIce = true;
                proj.aoeRadius = 2.3f;
            }

            // 💥 RPG 타워 (Modern Tier 4)
            if (towerType == TowerType.Modern && towerTier == 4)
            {
                proj.isAOE = true;
                proj.isRPG = true;
            }

            yield return new WaitForSeconds(attackSpeed);
        }
    }

    // --------------------------------------------------
    // 타워 방향 회전
    // --------------------------------------------------
    private void LookAtTarget()
    {
        if (currentTarget == null) return;

        Vector3 dir = currentTarget.transform.position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir);
    }

    // --------------------------------------------------
    // 플레이어 빙의 기능
    // --------------------------------------------------
    public void Possession()
    {
        // TODO: 1인칭 조준, 카메라 전환 등
    }

    // --------------------------------------------------
    // 빙의 상태에서 컨트롤러로 직접 타겟 지정
    // --------------------------------------------------
    private void ShootMonster()
    {
        if (controller == null) return;

        RaycastHit[] hits = Physics.RaycastAll(
            controller.transform.position,
            controller.transform.forward,
            100f
        );

        var found = hits
            .Where(h => h.collider.TryGetComponent(out MonsterObject _))
            .Select(h => h.collider.GetComponent<MonsterObject>())
            .ToList();

        if (found.Count == 0) return;

        debugTarget = found[0];
        currentTarget = found[0];
    }
}