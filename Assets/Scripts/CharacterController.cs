using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    // Componenets

    public Rigidbody2D rb2d;

    // Move

    public float forwardSpeed = 0f;
    bool movingForward = false;
    public float backwardSpeed = 0f;
    bool movingBackward = false;
    public bool automaticMovement = false;
    public Direction currentDirection = Direction.Right;

    public enum Direction {
        Left = -1,
        Right = 1,
    }

    // Jump

    public bool enableJump = false;
    bool canJump = true;
    public float jumpForce = 0f;
    public bool enableDoubleJump = false;
    bool canDoubleJump = false;
    public float doubleJumpForce = 0f;

    // Pulse

    public bool enablePulse = false;
    public float pulseForce = 0f;

    // Charge/Dash

    public bool enableDash = false;
    public float dashSpeed = 0f;
    public float dashDuration = 0f;
    bool dashing = false;
    float dashTime = 0f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        CheckInput();
    }

    private void FixedUpdate() {

    }

    void CheckInput() {

        Vector2 movement = rb2d.velocity;

        if (!automaticMovement) {
            float axis = Input.GetAxis("Horizontal");
            movement.x = axis * forwardSpeed;
            if (axis > 0) {
                currentDirection = Direction.Right;
            } else if (axis < 0) {
                currentDirection = Direction.Left;
            }
        } else {
            movement.x = (float) currentDirection * forwardSpeed;
        }

        if (enableJump) {
            if (Input.GetButtonDown("Jump")) {
                movement.y = jumpForce;
                Debug.Log("Get Axis Hor: " + Input.GetAxis("Vertical"));
            }

            if (enableDoubleJump && canDoubleJump) {

            }

        }

        if (enablePulse) {
            // TODO - Handle Pulse
        }

        if (enableDash) {
            if (dashing) {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0) {
                    dashing = false;
					gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
				} else {
                    movement.x = (float) currentDirection * dashSpeed;
                }
            } else if (Input.GetKeyDown(KeyCode.LeftShift)) {
                dashing = true;
                dashTime = dashDuration;
                movement.x = (float) currentDirection * dashSpeed;
				gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionY;
            }
        }

        rb2d.velocity = movement;
    }
}