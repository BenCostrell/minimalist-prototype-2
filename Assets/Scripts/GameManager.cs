using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public Vector3 spawnP1;
	public Vector3 spawnP2;
	public bool gameOver;
	public GameObject winScreen;

	// Use this for initialization
	void Start () {
		gameOver = false;
		winScreen.SetActive (false);
		InitializePlayers ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Reset")){
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	void InitializePlayers(){
		GameObject player1 = Instantiate (playerPrefab, spawnP1, Quaternion.identity);
		player1.GetComponent<PlayerController> ().playerNum = 1;

		GameObject player2 = Instantiate (playerPrefab, spawnP2, Quaternion.identity);
		player2.GetComponent<PlayerController> ().playerNum = 2;
	}

	public void EndGame(int playerNum){
		int winningPlayer;
		if (playerNum == 2) {
			winningPlayer = 1;
		} else {
			winningPlayer = 2;
		}
		gameOver = true;
		winScreen.SetActive (true);
		winScreen.GetComponent<Text> ().text = "PLAYER " + winningPlayer + " WINS";
	}
}
