using System.Collections;
using UnityEngine;

public class RigManager : Singleton<RigManager>
{
    public Transform rig;
    public float moveDuration;
    public TowerObject possessionTower;

    public void SetOriginPos()
    {
        rig.transform.localPosition = Vector3.zero;
        rig.transform.eulerAngles = new Vector3(90, 0, 0);
        // StartCoroutine(MovePosition(Vector3.zero));
        possessionTower = null;
    }

    public void SetTowerPos(TowerObject t)
    {
        rig.transform.position = new Vector3(t.transform.position.x, t.transform.position.y + t.transform.localScale.y * 3f, t.transform.position.z);
        rig.transform.eulerAngles = t.transform.eulerAngles;
        // StartCoroutine(MovePosition(Vector3.zero));
        possessionTower = t;
    }

    private IEnumerator MovePosition(Vector3 pos)
    {
        Vector3 vel = pos - transform.position;
        float duration = 0;

        while (duration < moveDuration)
        {
            Debug.Log("move");
            float prevTime = Time.deltaTime;
            
            yield return new WaitForFixedUpdate();
            
            float nowTime = Time.deltaTime;

            transform.Translate(vel / ((nowTime - prevTime) / moveDuration));

            duration += nowTime - prevTime;
        }
    }
}
