using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    private float timer = 0.0f;
    private int mode = 1;
    enum Options { Start, Mode, Exit };
    private Options currentSelection = Options.Start;


    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) == true) {
            Application.Quit();
        }
    }

    void FixedUpdate() {
        if (timer <= 0) {
            if (Input.GetAxis("Pitch") < -0.4) {
                if (currentSelection != Options.Exit) {
                    currentSelection++;
                    selectionChange();
                    timer = .5f;
                }
            } else if (Input.GetAxis("Pitch") > 0.4) {
                if (currentSelection != Options.Start) {
                    currentSelection--;
                    selectionChange();
                    timer = .5f;
                }
            }
        } else {
            timer -= Time.deltaTime;
        }

        if (Input.GetKeyDown("joystick button 0")) {
            performAction();
        }

        if (Input.GetKeyDown("joystick button 1"))
        {
            Application.Quit();
        }
/*
        if (Input.GetKey(KeyCode.UpArrow)) {
            if (currentSelection != Options.Exit) {
                currentSelection++;
                selectionChange();
            }
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            if (currentSelection != Options.Start) {
                currentSelection--;
                selectionChange();
            }
        }

        if (Input.GetKey(KeyCode.Space)) {
            performAction();
        }*/
    }

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
