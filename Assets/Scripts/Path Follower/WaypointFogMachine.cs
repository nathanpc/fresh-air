using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFogMachine : WaypointBase {
	public DMXFogMachine fogMachine;
	public bool turnOn;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Operates the fog machine.
	/// </summary>
	public void Operate() {
		if (turnOn) {
			fogMachine.TurnOn();
		} else {
			fogMachine.TurnOff();
		}
	}
}
