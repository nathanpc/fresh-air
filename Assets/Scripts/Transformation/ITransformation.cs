using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstration interface to help implement the character transformations brought
/// by the fog.
/// </summary>
public interface ITransformation {
	/// <summary>
	/// Start the <see cref="GameObject"/> transformation.
	/// </summary>
	void StartTransformation();
}
