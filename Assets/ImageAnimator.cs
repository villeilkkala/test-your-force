using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour {
	public List<Sprite> frames;
	public Image target;

	private IEnumerator<Sprite> frameEnumerator;
	// Use this for initialization
	void Start () {
		frameEnumerator = frames.GetEnumerator();
	}
	
	// Update is called once per frame
	void Update () {
		if (!frameEnumerator.MoveNext())
		{
			frameEnumerator = frames.GetEnumerator();
			frameEnumerator.MoveNext();
		}

		target.sprite = frameEnumerator.Current;

	}
}
