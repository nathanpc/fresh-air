using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the movement of a <see cref="GameObject"/> using the arrow keys
/// in the keyboard.
/// </summary>
/// <inheritdoc/>
public class KeyboardMover : IMover {
	public float MoveBackward() {
		return Input.GetKey(KeyCode.UpArrow) ? 1.0f : 0.0f;
	}

	public float MoveForward() {
		return Input.GetKey(KeyCode.DownArrow) ? 1.0f : 0.0f;
	}

	public float MoveLeft() {
		return Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f;
	}

	public float MoveRight() {
		return Input.GetKey(KeyCode.LeftArrow) ? 1.0f : 0.0f;
	}

	public void OnQuit() {
		// Do nothing.
	}

	public void PollDevice() {
		// Do nothing.
	}
}
