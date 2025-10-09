using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerUIManager : MonoBehaviour
{
    public InputActionReference towerCreateAction;

    void OnEnable()
    {
        towerCreateAction.action.performed += OnCreateTower;
    }
    void OnDisable()
    {
        towerCreateAction.action.performed -= OnCreateTower;   
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 타워 생성 함수
    public void OnCreateTower()
    {
        // TODO : 타워 생성 코드 작성
        Debug.Log("타워 생성!");
    }

    // 타워 편집 함수
    public void OnEditTower()
    {
        // TODO : 타워 편집 코드 작성
        Debug.Log("타워 편집!");
    }
}
