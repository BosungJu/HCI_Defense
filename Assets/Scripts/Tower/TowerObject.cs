using System.Collections;
using System.Linq;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TowerObject : MonoBehaviour
{
    private int towerKey;
    private float attackTimer;

    [SerializeField] private MonsterObject currentTarget;

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
    [SerializeField] TMP_Text towerInfo;

    public string stateName;
    public Animator animator;
    public RayInteractor leftInteractor;
    public RayInteractor rightInteractor;
    private Coroutine shootProjectile;

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
        Tower tower = TowerManager.Instance.towerData.GetTowerByKey(TowerKey);
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
        stateName = tower.Name;

        towerInfo.SetText($"Tier:{tower.TowerTier}\nType:{tower.TowerType}\nFantasy Type:{tower.FantasyType}");

        attackTimer = 1f / attackSpeed;

        if (shootProjectile == null) 
        {
            shootProjectile = StartCoroutine(ShootProjectileToMonster());
        }
    }

    // --------------------------------------------------
    // 타겟 갱신 
    // --------------------------------------------------
    private void UpdateTarget()
    {
        if ((currentTarget != null &&
            currentTarget.gameObject.activeSelf &&
            currentTarget.Health > 0) ||
            MonsterManager.Instance.Monsters.Count <= 0)
        {
            return;
        }

        currentTarget = FindClosestMonster();
    }


    private MonsterObject FindClosestMonster()
    {
        if (MonsterManager.Instance.Monsters.Count == 0)
        {
            return null;
        }

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

            firePoint = currentTarget.transform;

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

            proj.StartShootMonster();
            animator.Play(stateName);
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
        RigManager.Instance.SetTowerPos(this);
    }

    /// <summary>
    /// 빙의 중에 상대 지정.
    /// </summary>
    /// <param name="m"></param>
    public void UpdateTargetOnPossession(MonsterObject m)
    {
        currentTarget = m;
    }

    public void TowerSelected()
    {
        TowerManager.Instance.SelectTower = TowerManager.Instance.Towers.FindIndex(t => t == this);
    }

    public bool UpgradeTower(TowerObject t)
    {
        if (towerType == t.towerType && towerTier == t.towerTier && t.towerTier != 3)
        {
            TowerKey += 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TowerOutlineOnOff(bool isOn)
    {
        // TODO outline. or 표시.
    }
}