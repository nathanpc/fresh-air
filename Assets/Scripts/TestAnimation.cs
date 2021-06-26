using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour {
	private Animator anim;
    private bool fastSpin = false;

    // Start is called before the first frame update
    void Start() {
		anim = gameObject.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (anim != null)
                anim.SetTrigger("Run");
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            if (anim != null)
                anim.SetTrigger("Stop");
        }
    }
}
