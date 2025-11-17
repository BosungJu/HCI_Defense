using UnityEngine;
using OVR; // OVRInput을 사용하기 위해 필요합니다.

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    // [Range(최소값, 최대값)] 속성 추가: Inspector에서 슬라이더로 미세 조정 가능
    [Range(1f, 10f)]
    public float movementSpeed = 3.0f; // 기본 속도 설정 (3.0f는 비교적 느리고 안전한 속도입니다)

    private float fixedYPosition;      // 고정할 Y축 높이 변수

    void Start()
    {
        // 1. 씬 시작 시, 현재 오브젝트의 Y축 위치를 고정 값으로 저장합니다.
        //    이 값은 게임 내내 변하지 않습니다.
        fixedYPosition = transform.position.y;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // 1. 왼쪽 컨트롤러의 아날로그 스틱 입력 값 (2D 벡터)을 가져옵니다.
        Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        // 아날로그 스틱 입력이 거의 없을 때는 계산을 건너뜁니다.
        if (thumbstickInput.magnitude < 0.1f)
        {
            return;
        }

        // 2. 3D 이동 벡터 생성 (X와 Z축만 사용, Y축은 0으로 설정하여 수직 이동 방지)
        Vector3 localMovementVector = new Vector3(thumbstickInput.x, 0f, thumbstickInput.y);

        // 3. 컨트롤러의 방향(Rotation)을 기준으로 이동 벡터를 월드 좌표로 변환
        //    (World 기준이 아닌 Angle 기준 처리)
        //    스크립트가 붙은 오브젝트(OVRCameraRig)의 회전 값을 사용합니다.
        Quaternion rotation = transform.rotation;
        Vector3 worldMovement = rotation * localMovementVector;

        // **중요:** 회전 변환 후 Y축이 미세하게 변경될 수 있으므로 다시 0으로 설정합니다.
        worldMovement.y = 0f;

        // 4. 실제로 이동 적용
        transform.position += worldMovement * movementSpeed * Time.deltaTime;

        // 5. Y축 위치 강제 고정 (공중 탑뷰 높이 유지)
        //    이동 후 Y축 위치를 Start()에서 저장한 값으로 강제로 고정시킵니다.
        Vector3 currentPosition = transform.position;
        currentPosition.y = fixedYPosition; // 저장된 고정 Y축 값 사용
        transform.position = currentPosition;
    }
}