using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private GameController gameController;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// Called when the Collider other enters the trigger
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Enemy") {
			return;
		}

		if (explosion != null) {
			Instantiate (explosion, transform.position, transform.rotation);
		}

		if (other.tag == "Player") {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver ();
		}

		gameController.AddScore (scoreValue);
		
		//Debug.Log (other.name);
		// Destroy marks the object to be destroyed. They are all destroyed at the end of the frame.
		Destroy (other.gameObject);
		Destroy (gameObject);
	}
}
