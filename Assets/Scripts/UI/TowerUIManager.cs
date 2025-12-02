using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerUIManager : MonoBehaviour
{
    public InputActionReference towerCreateAction;

    public InputActionReference towerSellAction;

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
    public void OnCreateTower(InputAction.CallbackContext context)
    {
        // TODO: 타워 생성 코드 작성
        Debug.Log("타워 생성!");
    }

    // 타워 편집 함수
    public void OnEditTower()
    {
        // TODO: 타워 편집 코드 작성
        Debug.Log("타워 편집 모드!");
    }

    // 타워 판매 함수 
    public void OnsellTower(InputAction.CallbackContext context)
    {
        // TODO: 타워 판매 로직 작성
        // 1. 현재 조준하고 있는 위치/타워가 있는지 확인
        // 2. 타워를 제거하고 플레이어에게 자원 반환 
        Debug.Log("타워 판매 기능 실행!")
    }
}
