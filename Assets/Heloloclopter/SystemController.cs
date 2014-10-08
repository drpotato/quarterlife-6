using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SystemController : MonoBehaviour {
	/*public TextMesh timeText;
	public TextMesh altimeter;
	public TextMesh speedText;
    public TextMesh sequenceText;
    */
	public bool timeEnabled;
	public bool altimeterEnabled;
	public bool speedEnabled;
    public bool displayingSequence;

    public GameObject red;
    public GameObject yellow;
    public GameObject green;
    public GameObject blue;

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
        /*yellow.SetActive(false);
        green.SetActive(false);
        red.SetActive(false);
        blue.SetActive(false);*/
        sequenceLength = 1;
		nextCheckpoint = 0;
        sequence = new List<int>();
        ShuffleList ();
        DisplaySequence ();
        
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
        sequenceLength = 1;
        System.Random rand = new System.Random ();
        for (int i = 0; i < maxLength; i++) {
            sequence.Add(i % 4);//rand.Next (0, 4));
            
        }
        Debug.Log(sequence[0]);
    } 

	int CurrentCheckpoint () {
        //Debug.Log("FAGGOT: " + (checkpointOffset + sequencePosition) % numCheckpoints + " " + checkpointOffset + " " + sequencePosition + " " + numCheckpoints);
		return (checkpointOffset + sequencePosition)%numCheckpoints;
	}

	// Checkpoints
	void OnTriggerEnter(Collider other) {
        //Debug.Log("Collision");
		if (other.gameObject.tag == "Torus") {
            //Debug.Log("Ahh, you need to give me a trigger warning");
            if (CurrentCheckpoint () == other.gameObject.GetComponent<Ring>().checkpointNumber) {
                if (sequence[sequencePosition] == other.gameObject.GetComponent<Ring>().ringNumber) {
                    // Correct Answer
                    score += sequencePosition;
                    sequencePosition++;
                    Debug.Log("Correct");
                    CurrentCheckpoint();
                    Debug.Log(sequencePosition);
                    if (sequencePosition == sequenceLength) {
                        sequencePosition = 0;
                        sequenceLength++;
                        checkpointOffset++;
                        displayingSequence = true;
                        ringDisplayIndex = 0;
                    }
                }
                else {
                    // Incorrect Answer
                    Debug.Log("Incorrect");
                    score--;
                    checkpointOffset++;
                    sequencePosition = 0;
                    ShuffleList();
                    if (currentCheckpoint > 0) {
                        currentCheckpoint = 0;
                    }
                }
                other.gameObject.SetActive(false);
            }
            else {
                Debug.Log("Wrong checkpoint " + CurrentCheckpoint () + " " + other.gameObject.GetComponent<Ring>().checkpointNumber );
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
        if (ringDisplayIndex < sequenceLength && false) { 
            switch (sequence[ringDisplayIndex]){

                case(0):
                    yellow.SetActive(true);
                    green.SetActive(false);
                    red.SetActive(false);
                    blue.SetActive(false);
                    break;
                case(1):
                    yellow.SetActive(false);
                    green.SetActive(true);
                    red.SetActive(false);
                    blue.SetActive(false);
                    break;
                case(2):
                    yellow.SetActive(false);
                    green.SetActive(false);
                    red.SetActive(false);
                    blue.SetActive(true);
                    break;
                case(3):
                    yellow.SetActive(false);
                    green.SetActive(false);
                    red.SetActive(true);
                    blue.SetActive(false);
                    break;
            }

                
        } //sequenceText.text = "" + sequence[ringDisplayIndex];
	}

	void DisplayTime () {
		if (isTiming) {
			//timeText.text = timer.ToString ("F3");
		} else {
			//timeText.text = "Start pedalling\nwhen ready";
		}
	}

	void DisplaySpeed () {
		float speedProportion = speed / maxSpeed;
		int numSpeedMarkers = (int)(speedProportion * 10);

		//speedText.text = "";
		for (int i = 0; i < numSpeedMarkers; ++i) {
			//speedText.text += " ";
		}
		//speedText.text += "I";
	}

	void DisplayAltimeter () {
		height = transform.position.y - 10;

		float heightProportion = height / maxHeight;
		int numHeightMarkers = (int)(heightProportion * 20);
		
		//altimeter.text = "";
		for (int i = 0; i < Mathf.Min (numHeightMarkers, 20); ++i) {
			//altimeter.text += " ";
		}
		//altimeter.text += "I";
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
