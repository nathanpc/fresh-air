using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the movement of a <see cref="GameObject"/> using the arrow keys
/// in the keyboard.
/// </summary>
/// <inheritdoc/>
public class KeyboardMover : IMover {
	public bool MoveBackward() {
		return Input.GetKey(KeyCode.DownArrow);
	}

	public bool MoveForward() {
		return Input.GetKey(KeyCode.UpArrow);
	}

	public bool MoveLeft() {
		return Input.GetKey(KeyCode.LeftArrow);
	}

	public bool MoveRight() {
		return Input.GetKey(KeyCode.RightArrow);
	}
}
