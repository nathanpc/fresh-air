using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Controls the movement of a <see cref="GameObject"/> this is attached to
/// using the specified control interface.
/// </summary>
public class MovementInteraction : MonoBehaviour {
    public bool useKeyboard = false;
    public int speed = 1;
    public string serialPort;
    private IMover control;
    private PathFollower follower;
    private Thread poolThread;

    // Start is called before the first frame update
    void Start() {
        // Get the object's Path Follower.
        //follower = this.GetComponent<PathFollower>();
        //if (follower == null)
        //    MoreDebug.LogComponentNotFound(this.gameObject, "Path Follower");

        // Determine which method of control to use.
        if (useKeyboard) {
            control = new KeyboardMover();
        } else {
            control = new MicrophoneHubMover(serialPort);
        }

        // Setup and start the serial communication polling thread.
        poolThread = new Thread(new ThreadStart(SerialPoll));
        poolThread.Start();
    }

    // Update is called once per frame
    void Update() {
        // Check if we can start moving on our own.
        //if (!follower.IsAtFinalDestination())
        //    return;

        // Move the object.
        this.transform.Translate(Vector3.forward * Time.deltaTime * control.MoveForward() * speed);
        this.transform.Translate(Vector3.back * Time.deltaTime * control.MoveBackward() * speed);
        this.transform.Translate(Vector3.left * Time.deltaTime * control.MoveLeft() * speed);
        this.transform.Translate(Vector3.right * Time.deltaTime * control.MoveRight() * speed);
    }

    // Application is about to quit
    void OnApplicationQuit() {
        // Clean up.
        control.OnQuit();
    }

    /// <summary>
    /// Polls the device in a separate thread.
    /// </summary>
    public void SerialPoll() {
        // Poll the controller.
        while (true)
            control.PollDevice();
    }
}
