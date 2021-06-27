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
		// Do nothing if there isn't an animator.
		if (anim == null)
			return;

		// Trigger the animation.
		if (follower.showDebug)
			Debug.Log("Animation Waypoint: Play " + animationName + " animation");
		anim.SetTrigger(animationName);
	}

	/// <inheritdoc/>
	public override string WaypointType() {
		return "Animation";
	}
}
