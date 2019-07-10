using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class CharacterController : MonoBehaviour {

	// Componenets
	private Rigidbody2D rb2d;

	// Grounded
	[SerializeField]
	private GameObject groundedPoint;
	[SerializeField]
	private Vector2 groundedBoxDimens = new Vector2 (0.25f, 0.25f);

	// Move
	private bool shouldMove;

	[SerializeField]
	private float forwardSpeed = 0f;
	[SerializeField]
	private float backwardSpeed = 0f;
	[SerializeField]
	private bool automaticMovement = false;
	[SerializeField]
	private Direction currentDirection = Direction.Right;

	public enum Direction {
		Left = -1,
		Right = 1,
	}

	// Jump
	private bool shouldJump = false;
	private bool shouldDoubleJump = false;

	[SerializeField]
	private bool enableJump = false;
	[SerializeField]
	private float jumpForce = 0f;
	[SerializeField]
	private bool enableDoubleJump = false;
	private bool canDoubleJump = false;
	[SerializeField]
	private float doubleJumpForce = 0f;

	// Pulse
	[SerializeField]
	private bool enablePulse = false;
	[SerializeField]
	private float pulseForce = 0f;

	// Charge/Dash
	private bool shouldDash = false;

	[SerializeField]
	private bool enableDash = false;
	[SerializeField]
	private float dashSpeed = 0f;
	[SerializeField]
	private float dashDuration = 0f;
	private bool dashing = false;
	private float dashTime = 0f;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		CheckInput ();
	}

	private void FixedUpdate () {
		ApplyMovement ();
	}

	void CheckInput () {
		if (!automaticMovement) {
			float axis = Input.GetAxisRaw ("Horizontal");
			if (axis > 0) {
				currentDirection = Direction.Right;
				shouldMove = true;
			} else if (axis < 0) {
				currentDirection = Direction.Left;
				shouldMove = true;
			}
		} else {
			shouldMove = true;
		}

		if (enableJump) {
			// TODO - There is a potential for jumping being done twice so we might want to check velocity but that will negate the fuzzy grounded handling we have
			// TODO - We might want to set can double jump on ground collisions so if the user misses their jump they can still double jump to try and recover
			if (Input.GetButtonDown ("Jump") && IsGrounded ()) {
				shouldJump = true;
				canDoubleJump = true;
			} else if (enableDoubleJump && canDoubleJump && Input.GetButtonDown ("Jump")) {
				canDoubleJump = false;
				shouldDoubleJump = true;
			}
		}

		if (enablePulse) {
			// TODO - Handle Pulse
		}

		if (enableDash) {
			if (!dashing && Input.GetKeyDown (KeyCode.LeftShift)) {
				dashing = true;
				shouldDash = true;
				dashTime = dashDuration;
			}
		}
	}

	private void ApplyMovement () {
		Vector2 movement = rb2d.velocity;

		if (shouldMove) {
			movement.x = (float)currentDirection * forwardSpeed;
			shouldMove = false;
		} else {
			movement.x = 0;
		}

		if (shouldJump) {
			movement.y = jumpForce;
			shouldJump = false;
		}

		if (shouldDoubleJump) {
			movement.y = doubleJumpForce;
			shouldDoubleJump = false;
		}

		if (dashing) {
			if (shouldDash) {
				shouldDash = false;
				dashTime = dashDuration;
				movement.x = (float)currentDirection * dashSpeed;
				gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionY;
			} else {
				dashTime -= Time.fixedDeltaTime;
				if (dashTime <= 0) {
					dashing = false;
					gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
				} else {
					movement.x = (float)currentDirection * dashSpeed;
				}
			}
		}

		rb2d.velocity = movement;
	}

	private bool IsGrounded () {
		if (rb2d.velocity.y <= 0) {
			Collider2D [] collisions = Physics2D.OverlapBoxAll (groundedPoint.transform.position, groundedBoxDimens, 0f);
			foreach (Collider2D col in collisions) {
				if (col.gameObject.tag == "Platform") {
					return true;
				}
			}
		}
		return false;
	}

	public bool IsDashing () {
		return dashing;
	}

	private bool ShouldDie () {
		// Check front collision with ground

		// Check front collision with enemy

		return false;
	}

	private void OnCollisionEnter2D (Collision2D collision) {
		// TODO - Do a death check
	}

	private void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.tag == "Destroyable" && dashing) {
			DestroyableController dc = collision.gameObject.GetComponent<DestroyableController> ();
			if (dc != null) {
				Debug.Log ("Trigger");
				dc.DashDestroy ();
			}
		}

		// TODO - Do a death check
	}

}