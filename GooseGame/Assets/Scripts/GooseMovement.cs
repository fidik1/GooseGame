using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseMovement : MonoBehaviour
{
    // �.�. ������ �������� ���������� �� ��������� ������� � ����� ����������� ��������
    public float Speed = 5f;

    public float JumpForce = 300f;

    //��� �� ��� ���������� �������� �������� ��� "Ground" �� ���� ����������� �����
    private bool _isGrounded;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        //�������� �������� ��� ��� �������� � ������� 
        //���������� ������ � FixedUpdate, � �� � Update
        JumpLogic();

        // � ����� ������ ��������� ������������ ��� �����, �� ����� � � Update.
        // �� ��� �� �������� �����, �� 
        // ������� ����� ��������� ��������� fixedDeltaTim� 
        MovementLogic();

        RotateGoose();
    }

    private void MovementLogic()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // ��� �� �������� ���� ���������� � ����� ������
        // � �������� ��� �� �������� �� FixedUpdate �� �������� �� fixedDeltaTim�
        transform.Translate(movement * Speed * Time.fixedDeltaTime);
    }

    private void RotateGoose()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal") * Speed, 0f, Input.GetAxis("Vertical") * Speed);
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);

        Vector3 lookDirection = moveDirection + transform.position;

        transform.LookAt(new Vector3(lookDirection.x, transform.position.y, lookDirection.z));
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (_isGrounded)
            {
                // �������� �������� ��� � ����� �� ������ Vector3.up � �� �� ������ transform.up
                // ���� ��� �������� ��� ��� -- ��� up ����� ���� � ��� ����� � ���� � ����� � ������. 
                // �� ��� ����� ������ ������ �����! ������ � Vector3.up
                _rb.AddForce(Vector3.up * JumpForce);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGrounded = value;
        }
    }
}
