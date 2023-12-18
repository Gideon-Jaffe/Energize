using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxSpeed = 2;
    [SerializeField] private float acceleration = 50;
    [SerializeField] private float deAcceleration = 100;
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckRight;

    public bool facingRight;
    private float currentSpeed = 0;
    private bool isGrounded = true;
    private Rigidbody2D rb2D;
    public bool onDoor = false;
    private Vector3 jumpDirection = Vector3.up;

    void OnEnable()
    {
        jump.action.started += Jump;
        rb2D = GetComponent<Rigidbody2D>();
        facingRight = false;
    }

    void OnDisable()
    {
        jump.action.started -= Jump;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGrounded();
        float movementInput = movement.action.ReadValue<float>();
        if (facingRight && movementInput < 0) 
        {
            facingRight = false;
            GetComponent<SpriteRenderer>().flipX = false;
        } else if (!facingRight && movementInput > 0)
        {
            facingRight = true;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (movementInput > 0)
        {
            if (currentSpeed < 0)
            {
                currentSpeed += deAcceleration * maxSpeed * Time.deltaTime;
            }
            else
            {
                currentSpeed += acceleration * maxSpeed * Time.deltaTime;
            }
        }
        else if (movementInput < 0)
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= deAcceleration * maxSpeed * Time.deltaTime;
            }
            else
            {
                currentSpeed -= acceleration * maxSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= deAcceleration * maxSpeed * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += deAcceleration * maxSpeed * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, 0);
            }
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        Vector2 vel = rb2D.velocity;
        vel.x = currentSpeed;
        rb2D.velocity = vel;
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        PlayerJump(jumpForce);
    }

    public void PlayerJump(float forceForJump)
    {
        if (isGrounded)
        {
            rb2D.AddForce(jumpDirection * forceForJump, ForceMode2D.Impulse);
        }
    }

    private void CheckGrounded()
    {
        Collider2D[] collidersLeft = Physics2D.OverlapCircleAll(groundCheckLeft.transform.position, 0.1f).Where(collider => !collider.CompareTag("Door")).ToArray();
        Collider2D[] collidersRight = Physics2D.OverlapCircleAll(groundCheckRight.transform.position, 0.1f).Where(collider => !collider.CompareTag("Door")).ToArray();
        isGrounded = collidersLeft.Length > 1 || collidersRight.Length > 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            GameController.Instance.TouchedDoor();
        }
    }

    public void ChangeGravity()
    {
        rb2D.gravityScale = -rb2D.gravityScale;
        jumpDirection = -jumpDirection;
        if (rb2D.gravityScale > 0)
        {
            transform.rotation = new Quaternion {x = 0, y = 0, z = 0, w = 0};
        }
        else
        {
            transform.rotation = new Quaternion {x = 180, y = 0, z = 0, w = 0};
        }
    }
}
