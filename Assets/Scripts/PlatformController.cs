using UnityEngine;
using System.Collections;

public class PlatformController: MonoBehaviour, IPlatformDimensions {

	public float width;
	public float verticalOffset;
	public float horizontalOffset;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

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
