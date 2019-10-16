using UnityEngine;
using System.Collections;

public class PlatformController: MonoBehaviour, IPlatformDimensions {

	public float width;
	public float verticalOffset;
	public float horizontalOffset;

	[SerializeField]
	private GameObject [] terrains;

	// Use this for initialization
	void Start () {
		RandomizeTerrain ();
	}

	// Update is called once per frame
	void Update () {

	}

	private void RandomizeTerrain () {
		if (this.terrains.Length == 0) {
			return;
		}

		// Get a random number for every terrain + 1. If we land on the + 1
		// no terrain will be made active
		int index = Random.Range (0, terrains.Length + 1);

		for (int i = 0; i < terrains.Length; i++) {
			terrains[i].SetActive (i == index);
		}
	}

	public Vector2 PlacementOffset () {
		return new Vector2 {
			x = (width / 2) - horizontalOffset,
			y = -verticalOffset
		};
	}

	public float HorrizontalOffset () {
		return horizontalOffset;
	}

	public float VerticalOffset () {
		return verticalOffset;
	}

	public float Width () {
		return width;
	}

	public Vector2 LeftBound () {
		return new Vector2 {
			x = gameObject.transform.position.x + horizontalOffset - (width / 2),
			y = gameObject.transform.position.y + verticalOffset
		};
	}

	public Vector2 RightBound () {
		return new Vector2 {
			x = gameObject.transform.position.x + horizontalOffset + (width / 2),
			y = gameObject.transform.position.y + verticalOffset
		};
	}
}
