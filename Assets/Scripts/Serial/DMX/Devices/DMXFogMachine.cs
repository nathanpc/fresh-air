using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMXFogMachine : MonoBehaviour {
	public DMXController controller;
	public int auxChannel = 1;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Turns ON the fog machine.
	/// </summary>
	public void TurnOn() {
		controller.SetAuxOutput(auxChannel, 1);
	}

	/// <summary>
	/// Turns OFF the fog machine.
	/// </summary>
	public void TurnOff() {
		controller.SetAuxOutput(auxChannel, 0);
	}
}
