using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple interface to allow us to abstract the controls for the movement
/// interaction.
/// </summary>
public interface IMover {
	/// <summary>
	/// Should we move the <see cref="GameObject"/> forward?
	/// </summary>
	/// <returns><c>True</c> if we should move forward.</returns>
	bool MoveForward();

	/// <summary>
	/// Should we move the <see cref="GameObject"/> backwards?
	/// </summary>
	/// <returns><c>True</c> if we should move backwards.</returns>
	bool MoveBackward();

	/// <summary>
	/// Should we move the <see cref="GameObject"/> left?
	/// </summary>
	/// <returns><c>True</c> if we should move left.</returns>
	bool MoveLeft();

	/// <summary>
	/// Should we move the <see cref="GameObject"/> right?
	/// </summary>
	/// <returns><c>True</c> if we should move right.</returns>
	bool MoveRight();
}
