using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

	public laserScript laser;
	public AudioClip shoot;
	public AudioClip die;
	public AudioClip pain;
	public AudioClip miss;
	public AudioClip ready;

	public int i = 0;

	public StaticController soundController;

	public bool WaitForReady = false;

	public UnitySerialPort listener;

	public List<float> keyframes;
	public List<float> dwells;

	private IEnumerator<float> frameEnumerator;
	private IEnumerator<float> dwellEnumerator;

	private bool playing;
	private bool soundPlayed = false;

	private int hitCounter = 0;

	private float shotLength = 4f;

	private float initialDelay = 0f;

	// Use this for initialization
	void Start () {
		frameEnumerator = keyframes.GetEnumerator();
		frameEnumerator.MoveNext();

		dwellEnumerator = dwells.GetEnumerator();
		dwellEnumerator.MoveNext();

		StartCoroutine("WaitForCrane");
	}

	private IEnumerator WaitForCrane()
	{
		while (listener.IsZero() || listener.NotChanged())
		{
			if (!WaitForReady)
				break;

			yield return new WaitForEndOfFrame();
		}
		initialDelay = Time.unscaledTime;

		Debug.Log("Start");
		GetComponent<AudioSource>().PlayOneShot(ready);
		playing = true;
	}

	// Update is called once per frame
	void Update () {
		float currentDwell = dwellEnumerator.Current < 0 ? 4f : dwellEnumerator.Current;
		if (playing && !soundPlayed && Time.unscaledTime > frameEnumerator.Current + currentDwell - shotLength + initialDelay)
		{
			soundPlayed = true;

			if (dwellEnumerator.Current > -0.5)
			{
				Debug.Log(string.Format("Playing sound {0} at time {1} Keyframe {2} Dwell {3} Shot length {4} Delay {5}", i,  Time.unscaledTime, frameEnumerator.Current, currentDwell, shotLength, initialDelay));
				GetComponent<AudioSource>().PlayOneShot(shoot);
			}
			else
			{
				Debug.Log("Round change sound at time " + Time.unscaledTime);
				GetComponent<AudioSource>().PlayOneShot(ready);
			}
		}
		if (playing && Time.unscaledTime > frameEnumerator.Current + initialDelay)
		{
			Debug.Log("Starting stuff at " + Time.unscaledTime);
			Debug.Log("Shot " + i + " at keyframe time " + (Time.unscaledTime - initialDelay));

			StartCoroutine("Shoot", dwellEnumerator.Current);

			if (frameEnumerator.MoveNext())
			{
				dwellEnumerator.MoveNext();
				soundPlayed = false;
				i++;
			}
			else
			{
				playing = false;
				StartCoroutine("GameEnd");
			}
		}
	}

	IEnumerator Shoot (float dwell)
	{
		if (dwell < 0)
		{
			yield break;
		}

		Debug.Log(string.Format("Preparing to shoot. Time {0}", Time.unscaledTime));
		soundController.cameraMoving = false;
		yield return new WaitForSeconds(dwell);
		soundController.cameraMoving = true;

		if (laser.Shoot())
		{	
			yield return new WaitForSeconds(0.35f);
			Debug.Log("HIT! " + Time.unscaledTime);
			hitCounter++;
			if (hitCounter < 3)
				GetComponent<AudioSource>().PlayOneShot(pain);
			else
			{
				GetComponent<AudioSource>().PlayOneShot(die);
				playing = false;
			}

		}
		else
		{
			yield return new WaitForSeconds(0.35f);
			Debug.Log("BLOCK " + Time.unscaledTime);
			GetComponent<AudioSource>().PlayOneShot(miss);
		}

		Debug.Log(string.Format("Current hits {0}", hitCounter));
		yield return null;
	}

	IEnumerator GameEnd()
	{
		yield return new WaitForSeconds(5f);
		Debug.Log("End of round!");
	}
}
