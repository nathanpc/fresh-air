using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple interface to allow us to abstract the controls for the movement
/// interaction.
/// </summary>
public interface IMover {
	/// <summary>
	/// Polls the controller in case its needed.
	/// </summary>
	void PollDevice();

	/// <summary>
	/// Should we move the <see cref="GameObject"/> forward?
	/// </summary>
	/// <returns>How much we should move forward.</returns>
	float MoveForward();

	/// <summary>
	/// Should we move the <see cref="GameObject"/> backwards?
	/// </summary>
	/// <returns>How much we should move backwards.</returns>
	float MoveBackward();

	/// <summary>
	/// Should we move the <see cref="GameObject"/> left?
	/// </summary>
	/// <returns>How much we should move to the left.</returns>
	float MoveLeft();

	/// <summary>
	/// Should we move the <see cref="GameObject"/> right?
	/// </summary>
	/// <returns>How much we should move to the right.</returns>
	float MoveRight();
}
