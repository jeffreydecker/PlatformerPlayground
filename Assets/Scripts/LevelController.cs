using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
	public List<GameObject> platformPrefabs;

	public List<GameObject> activePlatforms;

	public float gapDistance = 2;
	//public float [] gapDistances = {1, 2}; 

	public float viewThreshold = 5;

	Camera cam;
	float viewHeight;
	float viewWidth;

    // Start is called before the first frame update
    void Start() {
		cam = Camera.main;
		viewHeight = 2f * cam.orthographicSize;
		viewWidth = viewHeight * cam.aspect;
	}

    // Update is called once per frame
    void Update() {
		AddPlatforms ();
		PrunePlatforms ();
    }

	void AddPlatforms() {
		PlatformController lastPlatform = activePlatforms [activePlatforms.Count - 1].GetComponent<PlatformController> ();
		Vector2 leftBound = lastPlatform.LeftBound ();
		if (leftBound.x < (cam.transform.position.x + (viewWidth / 2) + viewThreshold)) {
			Vector2 lastPlatformRight = lastPlatform.RightBound ();
			int index = Random.Range (0, platformPrefabs.Count);
			GameObject prefab = platformPrefabs [index];
			PlatformController pc = prefab.GetComponent<PlatformController> ();
			Vector3 position = pc.PlacementOffset ();
			position.x += lastPlatformRight.x + gapDistance;
			position.y += lastPlatformRight.y;
			GameObject instance = Instantiate (prefab, position, Quaternion.identity);
			activePlatforms.Add (instance);
		}
	}

	void PrunePlatforms() {
		for (int i = activePlatforms.Count - 1; i >= 0; i--) {
			GameObject platform = activePlatforms [i];
			PlatformController pc = platform.GetComponent<PlatformController> ();
			Vector2 rightBound = pc.RightBound ();
			if (rightBound.x < (cam.transform.position.x - (viewWidth / 2) - viewThreshold)) {
				activePlatforms.Remove (platform);
				Destroy (platform);
			}
		}
	}
}
