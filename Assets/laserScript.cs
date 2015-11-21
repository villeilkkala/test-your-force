using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour {
	public AudioClip shot;
	public MeshRenderer saber;
	public Transform startPoint;
	public Transform endPoint;
	LineRenderer laserLine;
	// Use this for initialization
	void Start () {
		laserLine = GetComponent<LineRenderer> ();
		laserLine.SetWidth (50f, 50f);
	}
	
	// Update is called once per frame
	void Update () {
		laserLine.SetPosition (0, startPoint.position);
		laserLine.SetPosition (1, endPoint.position);

	}

	public bool Shoot()
	{
		Debug.Log("Shot!");
		GetComponent<AudioSource>().PlayOneShot(shot);
		laserLine.enabled = true;
		StartCoroutine("StopLaser");
		return !saber.enabled;

	}

	public IEnumerator StopLaser()
	{
		yield return new WaitForSeconds(0.5f);
		laserLine.enabled = false;
	}
}
