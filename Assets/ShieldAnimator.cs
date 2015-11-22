using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShieldAnimator : MonoBehaviour {

	public List<Sprite> images;
	private IEnumerator<Sprite> imageEnumerator;
	public Image target;

	// Use this for initialization
	void Start () {
		imageEnumerator = images.GetEnumerator();
		imageEnumerator.MoveNext();
		target.sprite = imageEnumerator.Current;	
	}

	public void RemoveShield()
	{
		imageEnumerator.MoveNext();
		target.sprite = imageEnumerator.Current;
	}

}
