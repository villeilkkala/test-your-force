using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

	public laserScript laser;
	public AudioClip prepare;

	public List<float> keyframes;

	private IEnumerator<float> frameEnumerator;
	private float lastFrame;
	private bool playing;

	// Use this for initialization
	void Start () {
		lastFrame = 0f;
		frameEnumerator = keyframes.GetEnumerator();
		frameEnumerator.MoveNext();
		playing = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (playing && Time.unscaledTime - lastFrame > frameEnumerator.Current)
		{
			StartCoroutine("Shoot");

			if (frameEnumerator.MoveNext())
			{
				lastFrame = Time.unscaledTime;
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
		Debug.Log("Preparing to shoot");
		yield return new WaitForSeconds(4.0f);

		laser.Shoot();
		yield return null;
	}

	IEnumerator GameEnd()
	{
		yield return new WaitForSeconds(2f);
		Debug.Log("End of round!");
	}
}
