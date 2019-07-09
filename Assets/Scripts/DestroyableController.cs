using UnityEngine;
using System.Collections;

public class DestroyableController : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter2D (Collider2D collision) {
		CharacterController cc = collision.gameObject.GetComponent<CharacterController> ();
		if (cc != null && cc.IsDashing ()) {
			// TODO - Destroy this
		}
	}

	private void OnCollisionEnter2D (Collision2D collision) {
		
	}
}
