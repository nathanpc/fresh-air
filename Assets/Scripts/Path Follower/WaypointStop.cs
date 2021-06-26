using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be attached to a waypoint <see cref="GameObject"/> in order
/// for a character to stop for some seconds at that position.
/// </summary>
public class WaypointStop : WaypointBase {
	public int stoppedTime = 2;
	public string stoppedAnimation = "Stop";
	private bool stopEnabled = false;
	private float timeLeft;
	private bool finished = false;

	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		if (stopEnabled) {
			// Count down.
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0)
				finished = true;
		}
	}

	/// <summary>
	/// Starts the "stop" timer.
	/// </summary>
	public void StartTimer() {
		// Ignore if we've already started the timer.
		if (stopEnabled)
			return;

		// Setup the timer.
		timeLeft = stoppedTime;
		finished = false;
		stopEnabled = true;

		// Stop animation.
		if (follower.showDebug)
			Debug.Log("Stop Waypoint: Play stop animation");
		anim.SetTrigger("Stop");
		if (follower.showDebug)
			Debug.Log("Stop Waypoint: Play " + stoppedAnimation + " animation");
		anim.SetTrigger(stoppedAnimation);
	}

	/// <summary>
	/// Stops the "stop" timer.
	/// </summary>
	public void StopTimer() {
		stopEnabled = false;
		finished = false;
		timeLeft = stoppedTime;
	}

	/// <summary>
	/// Checks if the timer has finished doing its thing.
	/// </summary>
	/// <returns>Is the time up?</returns>
	public bool TimesUp() {
		return finished;
	}

	/// <inheritdoc/>
	public override string WaypointType() {
		return "Stop";
	}
}
