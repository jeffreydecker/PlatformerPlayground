using UnityEngine;
using System.Collections;

public class DestroyableController : MonoBehaviour {

	public void DashDestroy () {
		Debug.Log ("Destroy Me");
		Destroy (gameObject);
	}
}
