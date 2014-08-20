using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	Rigidbody rigid;
	float acceleration = 5.0f;
	float torqueCorrection = 1f;
	Vector3 cameraPos = new	Vector3(0.0f, 1.0f, 2.0f);
	public Camera mainCamera;
//	public GameObject bomb;
//	public Light light;
	Quaternion balanced;
	
	
	// Use this for initialization
	void Start () {
		rigid = gameObject.GetComponent<Rigidbody> ();
		balanced = new Quaternion();
		balanced.eulerAngles = transform.up;
	}
	
	void Update() {
//		if (Input.GetKey (KeyCode.E)) {
//			GameObject bomb_inst = (GameObject)Instantiate (bomb, transform.position + (new Vector3(0, 3, 0)), transform.rotation);
//			bomb_inst.rigidbody.AddRelativeForce(new Vector3(0, 0, 5000));
//		}
	}
	
	void FixedUpdate() {
		mainCamera.transform.position = transform.position;
		mainCamera.transform.rotation = transform.rotation;
		
//		light.transform.position = transform.position;
		
		if (Input.GetKey (KeyCode.W)) {
			// rigid.AddForce (-rigid.transform.forward * acceleration);
			rigid.AddRelativeTorque (new Vector3 (1.0f, 0.0f, 0.0f));
			
		} else if (Input.GetKey (KeyCode.S)) {
			// rigid.AddForce (rigid.transform.forward * acceleration);
			rigid.AddRelativeTorque (new Vector3 (-1.0f, 0.0f, 0.0f));
		} else if (Input.GetKey (KeyCode.A)) {
			rigid.AddTorque (new Vector3 (0.0f, -5.0f, 0.0f));
		} else if (Input.GetKey (KeyCode.D)) {
			rigid.AddTorque (new Vector3 (0.0f, 5.0f, 0.0f));
		}
		
		if (Input.GetKey (KeyCode.Space)) {
			rigid.AddForce (rigid.transform.up * acceleration*2);
		}
		
		Debug.Log (rigid.transform.rotation.eulerAngles.x);
		
		Vector3 corrected = Quaternion.Lerp(transform.rotation, balanced, 0.02f).eulerAngles;
		corrected.y = transform.rotation.eulerAngles.y;
		transform.eulerAngles = corrected;
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Cube") {
			other.gameObject.SetActive (false);
		}
	}
}
