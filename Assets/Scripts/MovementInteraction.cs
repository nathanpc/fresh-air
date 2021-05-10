using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the movement of a <see cref="GameObject"/> this is attached to
/// using the specified control interface.
/// </summary>
public class MovementInteraction : MonoBehaviour {
    public int speed = 1;
	private IMover control;

	// Start is called before the first frame update
	void Start() {
        control = new KeyboardMover();
	}

	// Update is called once per frame
	void Update() {
        if (control.MoveForward()) {
            this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        if (control.MoveBackward()) {
            this.transform.Translate(Vector3.back * Time.deltaTime * speed);
        }

        if (control.MoveLeft()) {
            this.transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (control.MoveRight()) {
            this.transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
    }
}
