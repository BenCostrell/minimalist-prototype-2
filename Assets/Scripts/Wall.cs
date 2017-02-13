using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	public static float movementRate = 0.005f;
	public int forwardDirection;
	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.gameOver) {
			transform.position += movementRate * forwardDirection * Vector3.right;
		}
	}
}
