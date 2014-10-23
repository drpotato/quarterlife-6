using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    private float timer = 0.0f;
    private int mode = 1;
    enum Options { Start, Mode, Exit };
    private Options currentSelection = Options.Start;

    //controls which buttons do what
    void FixedUpdate() {
        if (timer <= 0) {
            if (Input.GetAxis("Pitch") < -0.4 || Input.GetKey(KeyCode.DownArrow)) {
                if (currentSelection != Options.Exit) {
                    currentSelection++;
                }
            } else if (Input.GetAxis("Pitch") > 0.4 || Input.GetKey(KeyCode.UpArrow)) {
                if (currentSelection != Options.Start) {
                    currentSelection--;
                }
            }
            selectionChange();

			if (Input.GetKeyDown("joystick button 0") || Input.GetKey(KeyCode.Return)) {
				performAction();
			}
			timer = .5f;
        } else {
            timer -= Time.deltaTime;
        }

        /*if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }*/
    }

    //visually shows which menu option is currently selected
    void selectionChange() {
        if (currentSelection == Options.Start) {
            GameObject.FindWithTag("Start_Button").GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
            GameObject.FindWithTag("Mode_Button").GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
            GameObject.FindWithTag("Exit_Button").GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
        } else if (currentSelection == Options.Mode) {
            GameObject.FindWithTag("Start_Button").GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
            GameObject.FindWithTag("Mode_Button").GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
            GameObject.FindWithTag("Exit_Button").GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
        } else {
            GameObject.FindWithTag("Start_Button").GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
            GameObject.FindWithTag("Mode_Button").GetComponent<TextMesh>().fontStyle = FontStyle.Normal;
            GameObject.FindWithTag("Exit_Button").GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
        }
    }

    //loads the selected mode, changes the mode, and quits the application
    void performAction() {
        if (currentSelection == Options.Start) {
            Application.LoadLevel(mode);
        } else if (currentSelection == Options.Mode) {
            if (mode == 1) {
                GameObject.FindWithTag("Mode_Button").GetComponent<TextMesh>().text = "Mode: Cognitive";
                mode = 2;
            } else {
                GameObject.FindWithTag("Mode_Button").GetComponent<TextMesh>().text = "Mode: Regular";
                mode = 1;
            }
        } else if (currentSelection == Options.Exit) {
            Application.Quit();
        }
    }
}
