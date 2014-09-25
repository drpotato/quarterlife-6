using UnityEngine;
using System.Collections;

public class ConstantRotateUp : MonoBehaviour {

	public float degreesPerSecond = 0.0f;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * degreesPerSecond * Time.deltaTime);
	}
}
