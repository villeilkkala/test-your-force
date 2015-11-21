using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

	public laserScript laser;
	public AudioClip prepare;
	public AudioClip die;
	public AudioClip pain;
	public AudioClip miss;
	public AudioClip ready;

	public bool WaitForReady = false;

	public UnitySerialPort listener;

	public List<float> keyframes;
	public List<float> dwells;

	private IEnumerator<float> frameEnumerator;
	private IEnumerator<float> dwellEnumerator;

	private bool playing;

	private int hitCounter = 0;

	private float accumulatedDwell = 0f;
	private float dwellValue = 4f;

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

		Debug.Log("Start");
		GetComponent<AudioSource>().PlayOneShot(ready);
		playing = true;
	}

	// Update is called once per frame
	void Update () {
		if (playing && Time.unscaledTime > frameEnumerator.Current + accumulatedDwell + dwellEnumerator.Current - shotLength)
		{
			StartCoroutine("Shoot", dwellEnumerator.Current);

			if (frameEnumerator.MoveNext())
			{
				accumulatedDwell += dwellEnumerator.Current;
				dwellEnumerator.MoveNext();
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
		GetComponent<AudioSource>().PlayOneShot(prepare);
		Debug.Log(string.Format("Preparing to shoot. Time {0}", Time.unscaledTime));
		yield return new WaitForSeconds(dwell);

		if (laser.Shoot())
		{	
			yield return new WaitForSeconds(0.35f);
			Debug.Log("HIT!");
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
