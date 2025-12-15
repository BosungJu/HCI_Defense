using UnityEngine;
using OVR;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float movementSpeed = 3.0f; 

    private float fixedYPosition;    

    void Start()
    {
        // 씬 시작시, 현재 오브젝트의 Y축 위치 고정 값으로 저장 
        fixedYPosition = transform.position.y;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // 아날로그 스틱 입력 값(2D 벡터) 가져옴 
        Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        // 아날로그 스틱 입력이 미세할 때 계산 Skip
        if (thumbstickInput.magnitude < 0.1f)
        {
            return;
        }

        // 3D 이동 벡터 생성 (X와 Y축만 사용, Y축은 0으로 설정하여 수직 이동 방지)
        Vector3 localMovementVector = new Vector3(thumbstickInput.x, 0f, thumbstickInput.y);

        // 현재 오브젝트의 Angle 기준으로 이동 벡터를 월드 좌표로 변환 
        Quaternion rotation = transform.rotation;
        Vector3 worldMovement = rotation * localMovementVector;

        // Y축 고정 
        worldMovement.y = 0f;

        // 실제 이동 
        transform.Translate(worldMovement * movementSpeed * Time.deltaTime, Space.World);

        // Y축 고정 
        Vector3 currentPosition = transform.position;
        currentPosition.y = fixedYPosition; 
        transform.position = currentPosition;
    }
}