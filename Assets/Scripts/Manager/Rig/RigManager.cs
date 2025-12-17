using System.Collections;
using UnityEngine;

public class RigManager : Singleton<RigManager>
{
    public Transform rig;
    public float moveDuration;
    public TowerObject possessionTower;

    public void SetOriginPos()
    {
        rig.transform.SetParent(transform);
        StartCoroutine(MovePosition(Vector3.zero));
        possessionTower = null;
    }

    public void SetTowerPos(TowerObject t)
    {
        rig.transform.SetParent(t.transform);
        StartCoroutine(MovePosition(Vector3.zero));
        possessionTower = t;
    }

    private IEnumerator MovePosition(Vector3 pos)
    {
        Vector3 vel = pos - transform.position;
        float duration = 0;

        while (duration < moveDuration)
        {
            float prevTime = Time.deltaTime;
            
            yield return new WaitForFixedUpdate();
            
            float nowTime = Time.deltaTime;

            transform.Translate(vel / ((nowTime - prevTime) / moveDuration));

            duration += nowTime - prevTime;
        }
    }
}
