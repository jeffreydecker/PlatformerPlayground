﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class CharacterController : MonoBehaviour {

	// Componenets
	private Rigidbody2D rb2d;
	private Animator animator;
	[SerializeField]
	private GameObject landParticles;
	[SerializeField]
	private IPlayerDelegate playerDelegate;

	// Size
	[SerializeField]
	private Vector2 dimensions = new Vector2(1f, 1f);

	// Death
	private bool isDead = false;

	// Falling
	[SerializeField]
	private float minHeight = -3f;

	// Grounded
	[SerializeField]
	private GameObject groundedPoint;
	[SerializeField]
	private Vector2 groundedBoxDimens = new Vector2 (0.25f, 0.25f);

	// Hit
	[SerializeField]
	private GameObject rightHitPoint;
	[SerializeField]
	private Vector2 hitPointBoxDimens = new Vector2 (0.6f, 1.0f);

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
		animator = GetComponent<Animator> ();
	}

	void Update () {
		// Anything that should still be done after death goes here

		// Anything that should not be done when dead goes here
		if (isDead) return;

		if (gameObject.transform.position.y <= minHeight) {
			Die ();
			return;
		}

		CheckInput ();
	}

	private void FixedUpdate () {
		// Anything that should still be done after death goes here

		// Anything that should not be done when dead goes here
		if (isDead) return;

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
				animator.SetBool ("jumping", true);
			} else if (enableDoubleJump && canDoubleJump && Input.GetButtonDown ("Jump")) {
				canDoubleJump = false;
				shouldDoubleJump = true;
				animator.Play ("PlayerJump", 0, 0f);
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

	public bool IsDashing () {
		return dashing;
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

	// 
	private bool ShouldDie () {
		Collider2D [] collisions = Physics2D.OverlapBoxAll (rightHitPoint.transform.position, hitPointBoxDimens, 0f);
		foreach (Collider2D col in collisions) {
			if (col.gameObject.tag == "Platform") {
				return true;
			}
		}
		return false;
	}

	//private bool ShouldDie (ContactPoint2D contact) {
	//	Debug.LogFormat ("Contact: {0}, Transform: {1} Die:{2}", contact.point.x, transform.position.x, !Mathf.Approximately (transform.position.x, contact.point.x));
	//	Debug.LogFormat ("Contact: {0}, Transform: {1}", contact.point.y, transform.position.y);

	//	bool centered = Mathf.Approximately (contact.point.x, transform.position.x);
	//	bool top = Mathf.Approximately (contact.point.y - (dimensions.y / 2), transform.position.y);
	//	bool bottom = Mathf.Approximately (contact.point.y + (dimensions.y / 2), transform.position.y);

	//	return !centered && !top && !bottom;
	//}

	private void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Platform") {
			// TODO - We can probably use contact to determine if we hit a wall or not
			//ContactPoint2D contact = collision.GetContact (0);
			if (ShouldDie ()) {
				Die ();
			}
		}

		if (animator.GetBool ("jumping") && IsGrounded ()) {
			animator.SetBool ("jumping", false);
			animator.SetTrigger ("land");
			Instantiate (landParticles, gameObject.transform.position - new Vector3 (0, 0.5f), Quaternion.identity);
		}
	}

	private void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.tag == "Destroyable") {
			if (dashing) {
				DestroyableController dc = collision.gameObject.GetComponent<DestroyableController> ();
				if (dc != null) {
					dc.DashDestroy ();
				}
			} else {
				Die ();
			}
		}
	}

	private void Die () {
		Debug.Log ("Die Please");
		isDead = true;
		gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
		playerDelegate?.OnDeath ();
	}

}