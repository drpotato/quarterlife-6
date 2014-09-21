﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public GameObject rotor;
	public Camera mainCamera;
    public Camera fpCamera;
    public AudioClip whoosh;

    public float pitch = 1.03f;
	public float stability = 0.3f;
	public float correctionSpeed = 2.0f;

	public float maxLift = 20.0f;

	public GUIText winText;
	private int ringCount;


	Rigidbody rigid;
	float lift = 0.0f;
	float acceleration = 5.0f;
    bool fpsMode = false;

    // Use this for initialization
	void Start () {
		rigid = gameObject.GetComponent<Rigidbody> ();
		ringCount = 0;
		winText.text = "";
        fpCamera.enabled = fpsMode;
	}
	
	void Update() {
	}
	
	void FixedUpdate() {
		if (Input.GetKey (KeyCode.UpArrow)) {
			//rigid.AddForce (-rigid.transform.forward * acceleration);
			rigid.AddRelativeTorque (new Vector3 (2.0f, 0.0f, 0.0f));
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			// rigid.AddForce (rigid.transform.forward * acceleration);
			rigid.AddRelativeTorque (new Vector3 (-2.0f, 0.0f, 0.0f));
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			//rigid.AddRelativeTorque (new Vector3 (0.0f, -7.0f, 0.0f));
			rigid.AddRelativeTorque (new Vector3 (0.0f, 0.0f, 1.2f));
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			//rigid.AddRelativeTorque (new Vector3 (0.0f, 7.0f, 0.0f));
			rigid.AddRelativeTorque (new Vector3 (0.0f, 0.0f, -1.2f));
		}
        if (Input.GetKey(KeyCode.A))
        {
            rigid.AddRelativeTorque(new Vector3(0.0f, -7.0f, 0.0f));
            //rigid.AddRelativeTorque(new Vector3(0.0f, 0.0f, 1.0f));
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigid.AddRelativeTorque(new Vector3(0.0f, 7.0f, 0.0f));
            //rigid.AddRelativeTorque(new Vector3(0.0f, 0.0f, -1.0f));
        }
		if (Input.GetKey (KeyCode.W)) {
			lift += 0.05f;
			if (lift > maxLift) lift = maxLift;
		} else if (Input.GetKey (KeyCode.S)) {
			lift -= 0.05f;
			if (lift < 0.0f) lift = 0.0f;
		}
        if (Input.GetKeyDown(KeyCode.Tab)) {
            fpsMode = !fpsMode;
            fpCamera.enabled = fpsMode;
            mainCamera.enabled = !fpsMode;
        }


		rigid.AddRelativeForce (rigid.transform.up * lift);
		rotor.transform.Rotate (new Vector3(0.0f, lift, 0.0f));

		Vector3 predictedUp = Quaternion.AngleAxis(
			rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / correctionSpeed,
			rigidbody.angularVelocity
			) * transform.up;
		
		Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
		rigidbody.AddTorque(torqueVector * correctionSpeed * correctionSpeed);

        if (!audio.isPlaying && lift > .25f)
        {
            audio.clip = whoosh;
            audio.Play();
        }

        if (/*lift % 0.5f < .05f &&*/ lift > .25f)
        {
            audio.pitch = 1 + pitch * (lift / maxLift);
            audio.volume = .25f * (lift / maxLift);
            //audio.PlayOneShot(whoosh);

        }

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Cube") {
			other.gameObject.SetActive (false);
		}
		if (other.gameObject.tag == "Torus") {
			other.gameObject.SetActive (false);
			ringCount++;
		}
		if (ringCount >= 5) {
			winText.text = "Such win. Wow";
		}
	}
}
