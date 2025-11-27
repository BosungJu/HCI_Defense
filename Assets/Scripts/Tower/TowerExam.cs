using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction.Input;
using UnityEngine;

public class TowerExam : MonoBehaviour
{
    public Controller controller;

    private void PossessionOnTower() // player func.
    {
        var rayHits = Physics.RaycastAll(controller.transform.position, controller.transform.forward, 100f); // TODO 컨트롤러의 방향으로 변경. transform.forward => controller 방향

        List<Tower> t = rayHits.Where(r => r.collider.TryGetComponent<Tower>(out var tower)).Select(r => r.collider.GetComponent<Tower>()).ToList();

        t[0].Possession();
    }

    private void ShotMonster() // tower func.
    {
        var rayHits = Physics.RaycastAll(controller.transform.position, controller.transform.forward, 100f); // TODO 컨트롤러의 방향으로 변경. transform.forward => controller 방향

        List<MonsterObject> m = rayHits.Where(r => r.collider.TryGetComponent<MonsterObject>(out var monster)).Select(r => r.collider.GetComponent<MonsterObject>()).ToList();

        // target = m[0];
    }
}
