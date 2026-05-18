using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 3D 탑뷰 플레이어 이동 컨트롤러.
/// Input System의 Move 액션(Vector2)을 읽어 XZ 평면으로 이동시킨다.
///
/// [추후 모바일 전환 시]
/// 코드 수정 없이 InputSystem_Actions.inputactions 파일의 Move 액션에
/// On-Screen Stick 바인딩만 추가하면 모바일 조이스틱으로 동작한다.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("초당 이동 속도 (m/s)")]
    [SerializeField]
    private float moveSpeed = 5f;

    [Tooltip("이동 방향으로 회전하는 속도 (값이 클수록 즉각적)")]
    [SerializeField]
    private float rotationSpeed = 12f;

    [Header("Input")]
    [Tooltip("InputSystem_Actions 에셋의 Player/Move 액션을 연결")]
    [SerializeField]
    private InputActionReference moveAction;

    private Animator animator;

    private Rigidbody rb;
    private Vector2 moveInput;

    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 캐릭터가 충돌 시 넘어지지 않도록 회전은 코드로만 제어
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.action.Enable();
        }
        else
        {
            Debug.LogWarning(
                "[PlayerMovement] Move Action이 연결되지 않았습니다. Inspector에서 InputActionReference를 설정해주세요."
            );
        }
    }

    private void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();
    }

    private void Update()
    {
        // Vector2 (x, y) 형태로 입력값을 받음
        // 키보드 WASD/방향키 → 두 축의 -1~1 값
        // 게임패드/조이스틱 → 아날로그 -1~1 값
        if (moveAction != null)
            moveInput = moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // 탑뷰 3D : 입력의 y값은 카메라 기준 '앞/뒤'이므로 Z축으로 매핑
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);

        // 대각선 입력이 키보드일 경우 1.41배 빨라지지 않도록 정규화
        if (moveDir.sqrMagnitude > 1f)
            moveDir.Normalize();

        // --- 이동 ---
        // y(중력) 속도는 보존하고 수평 속도만 덮어쓴다
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        // --- 회전 (이동 방향을 바라보게) ---
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            Quaternion newRot = Quaternion.Slerp(
                rb.rotation,
                targetRot,
                rotationSpeed * Time.fixedDeltaTime
            );
            rb.MoveRotation(newRot);
        }

        if (animator != null)
        {
            bool IsWalking = moveDir.sqrMagnitude > 0.01f;
            animator.SetBool(IsWalkingHash, IsWalking);
        }
    }
}
