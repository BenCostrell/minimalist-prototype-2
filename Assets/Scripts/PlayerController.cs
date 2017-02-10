using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int playerNum;
	private float expansionTimeRemaining;
	public float growthRate;
	public float maxExpandTime;
	private float defaultRadius;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		defaultRadius = GetComponent<CircleCollider2D> ().bounds.extents.x;
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput ();
		if (expansionTimeRemaining > 0) {
			Expand ();
			expansionTimeRemaining -= Time.deltaTime;
		} else {
			ResetScale ();
		}
	}

	void ProcessInput(){
		if (Input.GetButtonDown("Shalom_P" + playerNum)){
			if (expansionTimeRemaining > 0) {
				expansionTimeRemaining = 0;
				MoveToEdge ();
				ResetScale ();
			} else {
				expansionTimeRemaining = maxExpandTime;
			}
		}
	}

	void Expand(){
		transform.localScale += growthRate * Vector3.one;
	}

	void ResetScale(){
		transform.localScale = Vector3.one;
	}

	void MoveToEdge(){
		Vector2 newPosition = transform.position;
		if (playerNum == 1) {
			newPosition.x = GetComponent<CircleCollider2D> ().bounds.max.x - defaultRadius;
		} else if (playerNum == 2) {
			newPosition.x = GetComponent<CircleCollider2D> ().bounds.min.x + defaultRadius;
		}
		//rb.MovePosition(newPosition);
		transform.position = newPosition;
	}

}
