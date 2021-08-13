using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseMovement : MonoBehaviour
{
    // т.к. логика движения изменилась мы выставили меньшее и более стандартное значение
    public float Speed = 5f;

    public float JumpForce = 300f;

    //что бы эта переменная работала добавьте тэг "Ground" на вашу поверхность земли
    private bool _isGrounded;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        //обратите внимание что все действия с физикой 
        //желательно делать в FixedUpdate, а не в Update
        JumpLogic();

        // в даном случае допустимо использовать это здесь, но можно и в Update.
        // но раз уж вызываем здесь, то 
        // двигать будем используя множитель fixedDeltaTimе 
        MovementLogic();

        RotateGoose();
    }

    private void MovementLogic()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // что бы скорость была стабильной в любом случае
        // и учитывая что мы вызываем из FixedUpdate мы умножаем на fixedDeltaTimе
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
                // Обратите внимание что я делаю на основе Vector3.up а не на основе transform.up
                // если наш персонаж это шар -- его up может быть в том числе и вниз и влево и вправо. 
                // Но нам нужен скачек только вверх! Потому и Vector3.up
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
