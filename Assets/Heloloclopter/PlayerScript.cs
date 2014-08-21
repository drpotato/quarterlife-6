using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public float rotationCorrection = 1f;
	public GameObject rotor;
	public Camera mainCamera;

	public float stability = 0.3f;
	public float correctionSpeed = 2.0f;

	Rigidbody rigid;
	float lift = 0.0f;
	float acceleration = 5.0f;
	float maxLift = 20.0f;

	Quaternion balanced;
	
	
	// Use this for initialization
	void Start () {
		rigid = gameObject.GetComponent<Rigidbody> ();
		balanced = new Quaternion();
		balanced.eulerAngles = transform.up;
	}
	
	void Update() {
	}
	
	void FixedUpdate() {
//		light.transform.position = transform.position;
		
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			// rigid.AddForce (-rigid.transform.forward * acceleration);
			rigid.AddRelativeTorque (new Vector3 (1.0f, 0.0f, 0.0f));
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			// rigid.AddForce (rigid.transform.forward * acceleration);
			rigid.AddRelativeTorque (new Vector3 (-1.0f, 0.0f, 0.0f));
		}
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			rigid.AddRelativeTorque (new Vector3 (0.0f, -7.0f, 0.0f));
			rigid.AddRelativeTorque (new Vector3 (0.0f, 0.0f, 1.0f));
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			rigid.AddRelativeTorque (new Vector3 (0.0f, 7.0f, 0.0f));
			rigid.AddRelativeTorque (new Vector3 (0.0f, 0.0f, -1.0f));
		}
		if (Input.GetKey (KeyCode.Space)) {
			lift += 0.05f;
			if (lift > maxLift) lift = maxLift;
		} else {
			lift -= 0.05f;
			if (lift < 0.0f) lift = 0.0f;
		}

		rigid.AddRelativeForce (rigid.transform.up * lift);
		rotor.transform.Rotate (new Vector3(0.0f, 2.0f*lift, 0.0f));

		Vector3 predictedUp = Quaternion.AngleAxis(
			rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / correctionSpeed,
			rigidbody.angularVelocity
			) * transform.up;
		
		Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
		rigidbody.AddTorque(torqueVector * correctionSpeed * correctionSpeed);

//		(balanced.x, balanced.y, balanced.z, balanced.w);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Cube") {
			other.gameObject.SetActive (false);
		}
	}
}
