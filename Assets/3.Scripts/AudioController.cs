﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(gameObject);
        gameObject.GetComponent<AudioSource>().Play();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
