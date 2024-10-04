using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTestPlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 300f; // AddForce�� ����� ���� ��
    public bool isGrounded = true;
    public float groundCheckDistance = 1.1f; // �ٴ� üũ �Ÿ�
    private Rigidbody rb;

    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
    }

    void Update()
    {
        // �÷��̾� �Է� �ޱ�
        float moveX = Input.GetAxis("Horizontal"); // A, D �Ǵ� ȭ��ǥ ����/������
        float moveZ = Input.GetAxis("Vertical"); // W, S �Ǵ� ȭ��ǥ ��/�Ʒ�

        // ���� ����
        moveDirection = new Vector3(moveX, 0, moveZ) * moveSpeed;

        // Ʈ������ �̵� (Rigidbody�� �ֱ� ������ ������ �̵��� ���� velocity�� ����)
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        // �ٴڿ� �ִ��� Ȯ��
        CheckGrounded();

        // ���� ó��
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce); // AddForce�� ���� ����
        }
    }

    // �ٴ� ���� �Լ� (Raycast ���)
    void CheckGrounded()
    {
        // �÷��̾��� ��ġ���� �Ʒ��� Raycast �߻�
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}
