using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    private int monsterKey;
    public int MonsterKey
    {
        get => monsterKey;
        set
        {
            monsterKey = value;

            SetMonsterData();
        }
    }
    [SerializeField] private float speed;
    public float Speed => speed;
    [SerializeField] private int health;
    public int Health => health;
    [SerializeField] private int damage;
    public int Damage => damage;
    [SerializeField] private int reward;
    public int Reward => reward;
    [SerializeField] private string monsterName;
    public string MonsterName => monsterName;

    [SerializeField] private Transform moveTarget;
    public Transform MoveTarget { get => moveTarget; set => moveTarget = value; }
    [SerializeField] private Dictionary<int, float> burnDamage = new Dictionary<int, float>();
    public Dictionary<int, float> BurnDamage => burnDamage;
    [SerializeField] private float slowDuration;
    public float SlowDuration => slowDuration;

    public NavMeshAgent agent;
    public Animator animator;

    private void SetMonsterData()
    {
        Monster monster = MonsterManager.Instance.monsterData.GetMonsterByKey(MonsterKey);

        if (monster != null)
        {
            speed = monster.Speed;
            health = monster.Health;
            damage = monster.Damage;
            reward = monster.Reward;
            monsterName = monster.Name;

            animator.Play("Walk");

            agent.Warp(MonsterManager.Instance.targetPoint[0].position);
            agent.stoppingDistance = 1f;
            agent.speed = speed;
            agent.SetDestination(MoveTarget.position);

            StartCoroutine(BurnMonster());
            StartCoroutine(CheckDestination());
        }
        else
        {
            Debug.LogError($"Monster data not found for key: {MonsterKey}");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 몬스터가 데미지를 입을 때.
    /// </summary>
    /// <param name="damage"></param>
    public void DamagedMonster(int damage, int burnDamage = 0, float debuffDuration = 0)
    {
        health -= damage;

        if (burnDamage == 0) // slow
        {
            slowDuration = debuffDuration;
        }
        else // burn
        {
            if (BurnDamage.ContainsKey(burnDamage))
            {
                BurnDamage[burnDamage] = debuffDuration;
            }
            else
            {
                this.burnDamage.Add(burnDamage, debuffDuration);
            }
        }

        if (health <= 0)
        {
            StartCoroutine(DieMonster());
        }
    }

    /// <summary>
    /// 몬스터가 죽을 때.
    /// </summary>
    private IEnumerator DieMonster()
    {
        // TODO 플레이어에게 돈 지급.
        if (agent.enabled)
        {
            agent.SetDestination(transform.position);
        }
        animator.Play("Death");

        MonsterManager.Instance.Monsters.Remove(this);

        yield return new WaitForSeconds(2);

        TowerManager.Instance.Gold += Reward;
        
        Destroy(gameObject);
    }

    private void Update() 
    {
        if (slowDuration > 0)
        {
            slowDuration -= Time.deltaTime;
            agent.speed = speed / 2;
        }
        else if (slowDuration <= 0)
        {
            agent.speed = speed;
        }
    }

    private IEnumerator BurnMonster()
    {
        while (true)
        {
            int maxDamage = 0;
            List<int> deleteKey = new List<int>();

            Dictionary<int, float> tempBurnDamage = new Dictionary<int, float>(burnDamage);

            foreach (var burn in tempBurnDamage)
            {
                if (burn.Value > 0)
                {
                    if (burn.Key > maxDamage)
                    {
                        maxDamage = burn.Key;
                    }

                    if (burnDamage.ContainsKey(burn.Key))
                    {
                        burnDamage[burn.Key] -= 1f;
                    }
                }
                else
                {
                    deleteKey.Add(burn.Key);
                }
            }

            foreach (int k in deleteKey)
            {
                burnDamage.Remove(k);
            }

            DamagedMonster(maxDamage);

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator CheckDestination()
    {
        while (true)
        {
            if (agent.hasPath)
            {
                if (agent.stoppingDistance >= agent.remainingDistance)
                {
                    MoveTarget = MonsterManager.Instance.GetNextTarget(MoveTarget);
                    agent.SetDestination(MoveTarget.position);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void SelectToTower()
    {
        if (RigManager.Instance.possessionTower != null)
        {
            RigManager.Instance.possessionTower.UpdateTargetOnPossession(this);
        }
    }
}
