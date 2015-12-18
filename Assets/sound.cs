using UnityEngine;
using System.Collections;

public class sound : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }
	
	
}
