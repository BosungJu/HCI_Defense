using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction.Input;
using UnityEngine;

public class TowerExam : MonoBehaviour
{
    public Controller controller;

    /// <summary>
    /// 플레이어가 빙의할 타워를 선택할 때.
    /// </summary>
    private void PossessionOnTower() // player func.
    {
        var rayHits = Physics.RaycastAll(controller.transform.position, controller.transform.forward, 100f); // TODO 컨트롤러의 방향으로 변경. transform.forward => controller 방향

        List<Tower> t = rayHits.Where(r => r.collider.TryGetComponent<Tower>(out var tower)).Select(r => r.collider.GetComponent<Tower>()).ToList();

        t[0].Possession();
    }
}
