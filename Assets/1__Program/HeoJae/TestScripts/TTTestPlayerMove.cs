using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTestPlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 300f; // AddForce에 사용할 점프 힘
    public bool isGrounded = true;
    public float groundCheckDistance = 1.1f; // 바닥 체크 거리
    private Rigidbody rb;

    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
    }

    void Update()
    {
        // 플레이어 입력 받기
        float moveX = Input.GetAxis("Horizontal"); // A, D 또는 화살표 왼쪽/오른쪽
        float moveZ = Input.GetAxis("Vertical"); // W, S 또는 화살표 위/아래

        // 방향 설정
        moveDirection = new Vector3(moveX, 0, moveZ) * moveSpeed;

        // 트랜스폼 이동 (Rigidbody가 있기 때문에 물리적 이동을 위해 velocity를 변경)
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        // 바닥에 있는지 확인
        CheckGrounded();

        // 점프 처리
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce); // AddForce를 통해 점프
        }
    }

    // 바닥 감지 함수 (Raycast 사용)
    void CheckGrounded()
    {
        // 플레이어의 위치에서 아래로 Raycast 발사
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}
