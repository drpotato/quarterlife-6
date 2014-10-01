using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public GameObject rotor;
	public Camera mainCamera;
    public Camera fpCamera;
    public AudioClip whoosh;
	public GameObject bikeController;
	public CockpitScreen screen;

    public float pitch = 1.03f;
	public float stability = 0.3f;
	public float correctionSpeed = 2.0f;

	public float maxLift = 50.0f;


	private int ringCount;


	Rigidbody rigid;
	float lift = 0.0f;
	float acceleration = 5.0f;
    bool fpsMode = true;

    // Use this for initialization
	void Start () {
		rigid = gameObject.GetComponent<Rigidbody> ();
		ringCount = 0;

        fpCamera.enabled = fpsMode;
	}
	
	void Update() {
	}
	
	void FixedUpdate() {
		Vector3 pitchChange = new Vector3 (40.0f, 0.0f, 0.0f);
		rigid.AddRelativeTorque (pitchChange*Input.GetAxis("Pitch"));

		Vector3 rollChange = new Vector3 (0.0f, 0.0f, 40.0f);
		rigid.AddRelativeTorque (rollChange*Input.GetAxis("Roll"));

		Vector3 yawChange = new Vector3 (0.0f, 140.0f, 0.0f);
		rigid.AddRelativeTorque (yawChange*Input.GetAxis("Yaw"));

		//*******************************//

		if (bikeController.GetComponent<BikeController>().bikePresent) {
			lift = bikeController.GetComponent<BikeController>().speed*20;
		} else {
			lift = ((Input.GetAxis ("Lift") + 1)/2)*maxLift;

			if (Input.GetKey (KeyCode.W)) {
				lift += 0.05f;
				if (lift > maxLift) lift = maxLift;
			} else if (Input.GetKey (KeyCode.S)) {
				lift -= 0.05f;
				if (lift < 0.0f) lift = 0.0f;
			} 
		}

		if (Input.GetKeyDown(KeyCode.Tab)) {
			fpsMode = !fpsMode;
			fpCamera.enabled = fpsMode;
			mainCamera.enabled = !fpsMode;
		}

//        if (lift > 0.0f) screen.StartTimer();
		//********************************//
		rigid.AddForce (rigid.transform.up * lift);
		rotor.transform.Rotate (new Vector3(0.0f, lift, 0.0f));

//		screen.SetSpeed (lift);



//		Vector3 predictedUp = Quaternion.AngleAxis(
//			rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / correctionSpeed,
//			rigidbody.angularVelocity
//			) * transform.up;
//		
//		Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
//		rigidbody.AddTorque(torqueVector * correctionSpeed * correctionSpeed);


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
	}
}
