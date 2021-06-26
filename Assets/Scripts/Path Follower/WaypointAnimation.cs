using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAnimation : WaypointBase {
	public string animationName = "Stop";

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Triggers the animation.
	/// </summary>
	public void TriggerAnimation() {
		anim.SetTrigger(animationName);
	}
}
