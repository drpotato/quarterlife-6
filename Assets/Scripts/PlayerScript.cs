﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public GameObject rotor;
	public AudioClip whoosh;
	public GameObject bikeController;
	
    public float pitch = 1.03f;
	public float stability = 0.3f;
	public float correctionSpeed = 2.0f;
    public float yaw = 140.0f;
	public float yawConstant = 0.1f;
    public float pitchH = 40.0f;
	public float pitchHConstant = 0.1f;
    public float roll = 40.0f;
	public float rollConstant = 0.1f;
	public float maxLift = 50.0f;

	private int ringCount;

	Rigidbody rigid;
	float lift = 0.0f;
	float acceleration = 5.0f;
    bool fpsMode = true;
    Collider last;
    bool lastRing = false;

    // Use this for initialization
	void Start () {
		rigid = gameObject.GetComponent<Rigidbody> ();
		ringCount = 0;
	}
	
	//controls the input for the helicopter as well as audio
	void FixedUpdate() {
		Vector3 pitchChange = new Vector3 ((pitchH*lift + 0.1f)/maxLift, 0.0f, 0.0f);
		rigid.AddRelativeTorque (pitchChange*Input.GetAxis("Pitch"));

		Vector3 rollChange = new Vector3 (0.0f, 0.0f, (roll*lift + 0.1f)/maxLift);
		rigid.AddRelativeTorque (rollChange*Input.GetAxis("Roll"));

		Vector3 yawChange = new Vector3 (0.0f, (yaw*lift + 0.1f)/maxLift, 0.0f);
		rigid.AddRelativeTorque (yawChange*Input.GetAxis("Yaw"));

		//*******************************//

		if (bikeController.GetComponent<BikeController>().bikePresent) {
			lift = bikeController.GetComponent<BikeController>().speed*20;
		} else {
			lift = ((Input.GetAxis("Lift") + 1)/2)*maxLift;

			if (Input.GetKey(KeyCode.W)) {
				lift += 0.05f;
				if (lift > maxLift) lift = maxLift;
			} else if (Input.GetKey(KeyCode.S)) {
				lift -= 0.1f;
				if (lift < 0.0f) lift = 0.0f;
			} 
		}

		if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.R)) {
			transform.position = transform.position + Vector3.down * 10;
			Vector3 rotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
			rotation.y = 0.0f;
			transform.rotation = Quaternion.LookRotation(rotation);
		}

		if (Input.GetKeyDown("joystick button 6") || Input.GetKey(KeyCode.Escape)) {
			Application.LoadLevel("Menu");
		}

		//********************************//

		rigid.AddForce (rigid.transform.up * lift * 3.0f);
		rotor.transform.Rotate (new Vector3(0.0f, lift, 0.0f));

        if (!audio.isPlaying && lift > .25f) {
            audio.clip = whoosh;
            audio.Play();
        }

        if (/*lift % 0.5f < .05f &&*/ lift > .25f) {
            audio.pitch = 1 + pitch * (lift / maxLift);
            audio.volume = .25f * (lift / maxLift);
        }

	}

	//logic for what happen when you pass through a ring
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Torus") {
            if (lastRing) {
                last.transform.parent.transform.parent.gameObject.SetActive(true);
                last.gameObject.SetActive(true);
            } else {
                lastRing = true;
            }
			other.gameObject.SetActive(false);
            Debug.Log(other.transform.parent);
            other.transform.parent.transform.parent.gameObject.SetActive(false);
            last = other;
			ringCount++;
		}
	}
}
