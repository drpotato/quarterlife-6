using UnityEngine;
using System.Collections;

public class CockpitScreen : MonoBehaviour {
	public TextMesh screenText;
	public TextMesh altimeter;
	public TextMesh speedText;

	float timer = 0.0f;
	float speed = 0.0f;
	float maxSpeed = 40.0f;

	float height = 0.0f;
	float maxHeight = 40.0f;

	bool isTiming = false;
	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		if (isTiming) {
			timer += Time.deltaTime;
		}

		if (isTiming) {
			screenText.text = timer.ToString ("F3");
		} else {
			screenText.text = "Start pedalling\nwhen ready";
		}

		float speedProportion = speed / maxSpeed;
		int numSpeedMarkers = (int)(speedProportion * 10);

		speedText.text = "";
		for (int i = 0; i < numSpeedMarkers; ++i) {
			speedText.text += " ";
		}
		speedText.text += "I";

		height = transform.position.y - 10;
//		speedText.text = speed.ToString ("F3");

		float heightProportion = height / maxHeight;
		int numHeightMarkers = (int)(heightProportion * 20);
		
		altimeter.text = "";
		for (int i = 0; i < Mathf.Min(numHeightMarkers, 20); ++i) {
			altimeter.text += " ";
		}
		altimeter.text += "I";
	}

	public void SetSpeed (float speed) {
		this.speed = speed;
	}

	public void StartTimer () {
		if (!isTiming) {
			isTiming = true;
		}
	}
}
