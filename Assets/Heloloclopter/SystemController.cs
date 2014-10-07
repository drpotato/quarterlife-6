using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SystemController : MonoBehaviour {
	public TextMesh timeText;
	public TextMesh altimeter;
	public TextMesh speedText;
    public TextMesh sequenceText;

	public bool timeEnabled;
	public bool altimeterEnabled;
	public bool speedEnabled;
    public bool displayingSequence;
    

	float timer = 0.0f;
	float speed = 0.0f;
	float maxSpeed = 40.0f;

	float height = 0.0f;
	float maxHeight = 40.0f;

	bool isTiming = false;


	// Ring logic
	int ringDisplayIndex;
	float timePerRing = 1.0f;
	float currentRingTimer;

	List<int> sequence;
	
	int nextCheckpoint;
	int sequenceLength;
	int score;
	int incorrectAnswers;
	int maxLength = 20;
    int checkpointOffset = 0;
    int sequencePosition = 0;
    int numCheckpoints = 37;
    int currentCheckpoint = 0;

	// Use this for initialization
	void Start () {
		sequenceLength = 1;
		nextCheckpoint = 0;
        ShuffleList ();
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isTiming) {
			timer += Time.deltaTime;
		}

		if (displayingSequence) {
			DisplaySequence ();
		} else {
			if (timeEnabled) DisplayAltimeter ();
			if (altimeterEnabled) DisplayAltimeter ();
			if (speedEnabled) DisplayAltimeter ();
		}
	}

    void ShuffleList () {
        sequence.Clear();
        System.Random rand = new System.Random ();
        for (int i = 0; i < maxLength; i++) {
            sequence.Add(rand.Next (0, 4));
        }
    } 

	int CurrentCheckpoint () {
		return (checkpointOffset + sequencePosition)%numCheckpoints;
	}

	// Checkpoints
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Ring") {

            if (CurrentCheckpoint () == other.gameObject.GetComponent<Ring>().checkpointNumber) {
                if (sequence[sequencePosition] == other.gameObject.GetComponent<Ring>().ringNumber) {
                    // Correct Answer
                    score += sequencePosition;
                    sequencePosition++;

                    if (sequencePosition == sequenceLength) {
                        sequencePosition = 0;
                        sequenceLength++;
                        displayingSequence = true;
                        ringDisplayIndex = 0;
                    }
                }
                else {
                    // Incorrect Answer
                    score--;

                    if (currentCheckpoint > 0) {
                        currentCheckpoint = 0;
                    }
                }
                other.gameObject.SetActive(false);
            }
            else {
                checkpointOffset = other.gameObject.GetComponent<Ring>().checkpointNumber;
                ShuffleList();
                sequencePosition = 0;

            }
		}
	}


	// Display functions
	void DisplaySequence () {
		currentRingTimer += Time.deltaTime;
		if (currentRingTimer > timePerRing) {
			ringDisplayIndex++;
			currentRingTimer = 0.0f;
		}

		sequenceText.text = "" + sequence[ringDisplayIndex];
	}

	void DisplayTime () {
		if (isTiming) {
			timeText.text = timer.ToString ("F3");
		} else {
			timeText.text = "Start pedalling\nwhen ready";
		}
	}

	void DisplaySpeed () {
		float speedProportion = speed / maxSpeed;
		int numSpeedMarkers = (int)(speedProportion * 10);

		speedText.text = "";
		for (int i = 0; i < numSpeedMarkers; ++i) {
			speedText.text += " ";
		}
		speedText.text += "I";
	}

	void DisplayAltimeter () {
		height = transform.position.y - 10;

		float heightProportion = height / maxHeight;
		int numHeightMarkers = (int)(heightProportion * 20);
		
		altimeter.text = "";
		for (int i = 0; i < Mathf.Min (numHeightMarkers, 20); ++i) {
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
