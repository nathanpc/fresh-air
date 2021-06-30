using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSpotlight : WaypointBase {
	public DMXSpotlight spotlight;
	public float red;
	public float green;
	public float blue;
	private int counter = -1;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		// Do nothing if we have disabled the counter.
		if (counter < 0)
			return;

		// Count up.
		counter++;

		// Use a counter to delay each command.
		switch (counter) {
			case 1:
				spotlight.RedDimmer(red);
				break;
			case 5:
				spotlight.BlueDimmer(blue);
				break;
			case 9:
				spotlight.GreenDimmer(green);
				counter = -1;
				break;
		}
	}

	/// <summary>
	/// Send commands to the spotlight in sequence.
	/// </summary>
	/*IEnumerator SequenceCommands() {
		spotlight.RedDimmer(red);

		//Wait for 4 seconds
		yield return new WaitForSeconds(4);

		//Rotate 40 deg
		transform.Rotate(new Vector3(40, 0, 0), Space.World);

		//Wait for 2 seconds
		yield return new WaitForSeconds(2);

		//Rotate 20 deg
		transform.Rotate(new Vector3(20, 0, 0), Space.World);
	}*/

	/// <summary>
	/// Operates the spotlight.
	/// </summary>
	public void Operate() {
		counter = 0;
	}
}
