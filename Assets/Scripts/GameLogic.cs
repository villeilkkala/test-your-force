using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

	public laserScript laser;
	public AudioClip prepare;

	public List<float> keyframes;

	private IEnumerator<float> frameEnumerator;
	private bool playing;

	private int hitCounter = 0;

	private float accumulatedDwell = 0f;
	private float dwellValue = 4f;

	// Use this for initialization
	void Start () {
		frameEnumerator = keyframes.GetEnumerator();
		frameEnumerator.MoveNext();
		playing = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (playing && Time.unscaledTime > frameEnumerator.Current + accumulatedDwell)
		{
			StartCoroutine("Shoot");

			if (frameEnumerator.MoveNext())
			{
				accumulatedDwell += dwellValue;
			}
			else
			{
				playing = false;
				StartCoroutine("GameEnd");
			}
		}
	}

	IEnumerator Shoot ()
	{
		GetComponent<AudioSource>().PlayOneShot(prepare);
		Debug.Log(string.Format("Preparing to shoot. Time {0}", Time.unscaledTime));
		yield return new WaitForSeconds(4.0f);

		if (laser.Shoot())
		{	
			Debug.Log("HIT!");
			hitCounter++;
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
