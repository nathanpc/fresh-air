using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAudio : WaypointBase {
	public AudioSource audioSource;
	public bool play;

	// Start is called before the first frame update
	void Start() {
		// Check if we have an Audio Source to use.
		if (audioSource == null) {
			audioSource = gameObject.GetComponent<AudioSource>();
		}
	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Plays the associated audio source.
	/// </summary>
	public void Play() {
		audioSource.Play();
	}

	/// <summary>
	/// Stops the associated 
	/// </summary>
	public void Stop() {
		audioSource.Stop();
	}

	/// <inheritdoc/>
	public override string WaypointType() {
		return "Audio";
	}
}
