using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class Webcam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		WebCamDevice[] devices = WebCamTexture.devices;
		for( var i = 0 ; i < devices.Length ; i++ )
		    Debug.Log(devices[i].name);

		WebCamTexture texture = new WebCamTexture (devices[0].name);
		texture.Play();

		this.GetComponent<RawImage>().texture = texture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
