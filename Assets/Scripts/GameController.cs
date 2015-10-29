using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	public int bossCountDown;
	
	public GameObject[] asteroidHazards;
	public GameObject enemyHazard;
	public AsteroidSize asteroidSize;

	public Vector3 spawnValues;
	public int hazardCount;
	public int enemyRound;
	public float asteroidSpawnWait;
	public float enemySpawnWait;
	public float startWait;
	public float waveWait;

	public Text hiScoreText;
	public Text scoreText;
	public Text restartText;
	public Text gameOverText;
	public Text prepareForBossText;

	private bool gameOver;
	private bool restart;
	private int score;
	private int enemyWaveDue;

	void Start ()
	{
		gameOver = false;
		restart = false;

		restartText.text = "";
		gameOverText.text = "";
		prepareForBossText.text = "";

		score = 0;
		enemyWaveDue = 1;

		UpdateScore ();
		UpdatePlayerHiScore ();

		StartCoroutine (SpawnWaves ());
	}

	void Update ()
	{
		//DEBUG
		//PlayerPrefs.DeleteKey ("HiScore");

		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}

		if (gameOver) {
			if (PlayerPrefs.HasKey ("HiScore")) {
				var hiScore = PlayerPrefs.GetInt ("HiScore");
				if (score > hiScore) {
					PlayerPrefs.SetInt ("HiScore", score);
				}
			} else {
				PlayerPrefs.SetInt ("HiScore", score);
			}
			UpdatePlayerHiScore ();
		}
	}

	void SpawnAsteroid ()
	{
		Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
		Quaternion spawnRotation = Quaternion.identity; // This quaternion corresponds to "no rotation" - the object is perfectly aligned with the world or parent axes

		GameObject asteroidHazard = asteroidHazards [Random.Range (0, asteroidHazards.Length)];
		asteroidHazard.transform.localScale = new Vector3 (Random.Range (asteroidSize.min, asteroidSize.max), 1, Random.Range (asteroidSize.min, asteroidSize.max));
		Instantiate (asteroidHazard, spawnPosition, spawnRotation);
	}

	void SpawnEnemy ()
	{
		Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
		Quaternion spawnRotation = Quaternion.identity; // This quaternion corresponds to "no rotation" - the object is perfectly aligned with the world or parent axes

		enemyHazard.transform.localScale = new Vector3 (1, 1, 1);
		Instantiate (enemyHazard, spawnPosition, spawnRotation);
	}

	IEnumerator ShowRestart ()
	{
		yield return new WaitForSeconds (2);
		restartText.text = "Press 'R' for Restart";
		restart = true;
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);

		bool keepSpawning = true;
		while (keepSpawning) {
			for (int i = 0; i < hazardCount; i++) {
				if (enemyWaveDue == enemyRound) {
					SpawnEnemy ();
				} else {
					SpawnAsteroid ();
				}

				yield return new WaitForSeconds (enemyWaveDue == enemyRound ? enemySpawnWait : asteroidSpawnWait);

				if (!restart && gameOver) {
					StartCoroutine (ShowRestart ());
					keepSpawning = false;
				}
			}

			if (keepSpawning) {
				bossCountDown--;
				yield return new WaitForSeconds (waveWait);

				if (bossCountDown == 0) {
					yield return new WaitForSeconds (asteroidSpawnWait);
					PrepareForBoss ();
					yield return new WaitForSeconds (6);
					SpawnBoss ();
					break;
				}

				if (enemyWaveDue == enemyRound) {
					enemyWaveDue = 0;
				}
				enemyWaveDue++;
			}
		}
	}

	void SpawnBoss ()
	{
		Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
		Quaternion spawnRotation = Quaternion.identity; // This quaternion corresponds to "no rotation" - the object is perfectly aligned with the world or parent axes
		
		enemyHazard.transform.localScale = new Vector3 (1, 1, 1);
		Instantiate (enemyHazard, spawnPosition, spawnRotation);
	}

	void PrepareForBoss ()
	{
		prepareForBossText.text = "Prepare for Boss!";
		GameObject gameObject = GameObject.FindWithTag ("Prepare Boss");
		gameObject.GetComponent<AudioSource> ().Play ();
		StartCoroutine (StopPrepareForBossAudio ());
	}

	IEnumerator StopPrepareForBossAudio ()
	{
		yield return new WaitForSeconds (4);
		GameObject gameObject = GameObject.FindWithTag ("Prepare Boss");
		gameObject.GetComponent<AudioSource> ().Stop ();
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}

	void UpdatePlayerHiScore ()
	{
		hiScoreText.text = "Hi-Score: " + PlayerPrefs.GetInt ("HiScore");
	}

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}

[System.Serializable]
public class AsteroidSize
{
	public float min;
	public float max;
}
