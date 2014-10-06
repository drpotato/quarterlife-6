using UnityEngine;
using System.Collections;

public class ConstantRotateForward : MonoBehaviour {
	
	public float degreesPerSecond = 0.0f;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * degreesPerSecond * Time.deltaTime);
	}
}
