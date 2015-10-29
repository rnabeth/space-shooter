using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkText : MonoBehaviour
{
	public float interval;
	public int stopInterval;

	private bool coroutineStarted;

	void Start ()
	{
		coroutineStarted = false;
	}

	void Update ()
	{
		var text = GetComponent<Text> ();
		if (text.text != "" && !coroutineStarted) {
			coroutineStarted = true;
			StartCoroutine (Blink ());
		}
	}

	IEnumerator Blink ()
	{
		var currentTime = Time.time;
		var endTime = Time.time + stopInterval;
		var text = GetComponent<Text> ();
		while (currentTime < endTime) {
			yield return new WaitForSeconds (interval);
			text.enabled = !text.isActiveAndEnabled;
			currentTime = Time.time;
		}
		text.enabled = false;
	}
}
