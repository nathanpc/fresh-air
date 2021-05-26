using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles time in our game. This way we can speed up or down the animation for
/// debugging purposes.
/// </summary>
public class TimeWarp : MonoBehaviour {
	public float gameSpeed = 1.0f;

	// Start is called before the first frame update
	void Start() {
		// Adjust the time scale.
		Time.timeScale = gameSpeed;
		Time.fixedDeltaTime *= Time.timeScale;
	}
}
