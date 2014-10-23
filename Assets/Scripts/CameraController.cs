using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	Vector3 offset = new Vector3(0.0f, 2.0f, 2.0f);
	public Transform player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()  {
		transform.position = player.position + offset;
		transform.rotation = player.rotation;
	}

}
