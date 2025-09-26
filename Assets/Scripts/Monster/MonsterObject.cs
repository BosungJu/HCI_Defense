using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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

    public NavMeshAgent navMeshAgent;

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

            navMeshAgent.speed = speed;
            navMeshAgent.SetDestination(moveTarget.position);
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
    public void DamagedMonster(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DieMonster();
        }
    }

    /// <summary>
    /// 몬스터가 죽을 때.
    /// </summary>
    private void DieMonster()
    {
        // TODO 플레이어에게 돈 지급.
        gameObject.SetActive(false);
        MonsterManager.Instance.Monsters.Remove(this);
        MonsterManager.Instance.EnqueueMonster(this);
    }
}
