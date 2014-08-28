using UnityEngine;
using System.Collections;

public class TerrainScript : MonoBehaviour {

    public AudioClip bang;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision){
        //if (other.gameObject.tag == "Heloloclopter"){
            audio.clip = bang;
            audio.Play();
        //}
        
    }

}
