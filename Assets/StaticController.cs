using UnityEngine;
using System.Collections;
using System;

public class StaticController : MonoBehaviour {

	public int MinimumCutoff = 2000;
	public int MaximumCutoff = 6000;

	public float MaxMagnitude = 10f;
	private float smoothedSpeed = 0f;
	private float rawSpeed = 0f;
	private float simulatedSpeed = 0f;

	public bool cameraMoving = true;

	public Transform TrackedObject;

	private Vector3 lastPosition;
	// Use this for initialization
	void Start () {
	
	}

	private float GetSpeed()
	{
		if (cameraMoving)
			return 0f;

		Vector3 delta = TrackedObject.position - lastPosition;
		lastPosition = TrackedObject.position;

		return Math.Min(delta.magnitude/MaxMagnitude, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		rawSpeed = GetSpeed();

		simulatedSpeed = 0.5f*rawSpeed + rawSpeed*simulatedSpeed + 0.7f * simulatedSpeed;
		smoothedSpeed = (simulatedSpeed*4 + smoothedSpeed*10 + rawSpeed)/15;
		
		int newCutoff = MinimumCutoff + (int)((MaximumCutoff - MinimumCutoff)*smoothedSpeed);

		GetComponent<AudioLowPassFilter>().cutoffFrequency = newCutoff;

	}
}
