using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple class to help with debugging, providing some nice utility methods.
/// </summary>
public class MoreDebug {
	/// <summary>
	/// Logs if we have an unassigned inspector reference.
	/// </summary>
	/// <param name="gameObject">Object that we want to check.</param>
	/// <param name="referenceName">Variable name got via <c>nameof(myVar)</c>.</param>
	public static void LogUnassignedReference(GameObject gameObject, string referenceName) {
		Debug.LogError("Unassigned 'Inspector' reference: " + referenceName, gameObject);

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
