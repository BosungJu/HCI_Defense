using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Target")]
    public MonsterObject target;

    [Header("Stats")]
    public float speed = 20f;
    public int damage;
    public bool isAOE = false;
    public float aoeRadius = 0f;

    [Header("Fantasy Effects")]
    public bool isFire = false;
    public bool isIce = false;
    public bool isRPG = false;

    // Fire DOT settings
    private float burnRatio = 0.5f;
    private float burnDuration = 5f;

    // Ice Slow settings
    private float slowDuration = 3f;

    // RPG settings
    private float rpgMultiplier = 2f;       // RPG는 순간폭딜 매우 강함
    private float rpgAoeRadius = 1.2f;      // 좁은 범위 폭발

    [Header("FX & Audio (Optional)")]
    public GameObject hitEffect;
    public AudioClip hitSound;
    private AudioSource audioSource;


    private void Start()
    {
        // 사운드 재생용 AudioSource 자동 설정
        audioSource = GetComponent<AudioSource>();
    }


    private void OnHit()
    {
        // 이펙트 재생
        if (hitEffect)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // 사운드 재생
        if (hitSound && audioSource)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (!isAOE)
        {
            ApplySingleDamage(target);
        }
        else
        {
            ApplyAOE();
        }

        StopCoroutine("ShootMonster");
        Destroy(gameObject);
    }

    public void StartShootMonster()
    {
        StartCoroutine("ShootMonster");
    }

    private IEnumerator ShootMonster()
    {
        while (true)
        {
            if (target == null || !target.gameObject.activeSelf)
            {
                StopCoroutine("ShootMonster");
                Destroy(gameObject);
            }

            // 타겟 방향
            Vector3 dir = (target.transform.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            // 명중 판정
            if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)
            {
                OnHit();
            }

            yield return new WaitForFixedUpdate();
        }
    }


    // --------------------------------------------------------------------
    // 단일 타겟 데미지
    // --------------------------------------------------------------------
    private void ApplySingleDamage(MonsterObject m)
    {
        if (m == null) return;

        m.DamagedMonster(damage);

        if (isFire)
        {
            int burnDmg = Mathf.RoundToInt(damage * burnRatio);
            m.DamagedMonster(0, burnDmg, burnDuration);
        }

        if (isIce)
        {
            m.DamagedMonster(0, 0, slowDuration);
        }
    }


    // --------------------------------------------------------------------
    // AOE (광역 공격)
    // --------------------------------------------------------------------
    private void ApplyAOE()
    {
        float radius = aoeRadius;

        if (isRPG)
            radius = rpgAoeRadius; // RPG는 좁은 폭발 범위

        List<MonsterObject> monsters = MonsterManager
            .Instance.Monsters.Where(m =>
                Vector3.Distance(m.transform.position, target.transform.position) <= radius
            )
            .ToList();

        foreach (var m in monsters)
        {
            if (isRPG)
            {
                // RPG 폭발 = 매우 강한 순간 폭딜
                m.DamagedMonster(Mathf.RoundToInt(damage * rpgMultiplier));
            }
            else
            {
                // 일반 Fire/Ice AOE 기본 데미지
                m.DamagedMonster(damage);
            }

            if (isFire)
            {
                int burnDmg = Mathf.RoundToInt(damage * burnRatio);
                m.DamagedMonster(0, burnDmg, burnDuration);
            }

            if (isIce)
            {
                m.DamagedMonster(0, 0, slowDuration);
            }
        }
    }
}
