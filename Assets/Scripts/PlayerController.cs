using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int playerNum;
	private float expansionTimeRemaining;
	private float contractionTimeRemaining;
	public float maxExpansionSize;
	public float maxExpandTime;
	public float contractionTimeFactor;
	private float expansionTimeElapsed;
	private float expandedUpTo;
	private bool expanding;
	private bool contracting;
	private float defaultRadius;
	private float targetEdge;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		defaultRadius = GetComponent<CircleCollider2D> ().bounds.extents.x;
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput ();
		if (expanding) {
			Expand ();
		} else if (contracting) {
			Contract ();
		}
	}

	void ProcessInput(){
		if (Input.GetButtonDown("Shalom_P" + playerNum)){
			if (expanding) {
				BeginContraction ();
			} else if (!contracting){
				BeginExpansion ();	
			}
		}
	}

	void Expand(){
		expansionTimeRemaining -= Time.deltaTime;
		transform.localScale = Vector3.Lerp (Vector3.one, maxExpansionSize * Vector3.one, 1 - expansionTimeRemaining / maxExpandTime);
		expansionTimeElapsed = Mathf.Min(maxExpandTime - expansionTimeRemaining, maxExpandTime);
		expandedUpTo = transform.localScale.x;
		if (expansionTimeRemaining <= 0) {
			BeginContraction ();
		}
	}

	void Contract(){
		contractionTimeRemaining -= Time.deltaTime;
		transform.localScale = Vector3.Lerp (expandedUpTo * Vector3.one, Vector3.one, 
			1 - contractionTimeRemaining / (expansionTimeElapsed * contractionTimeFactor));
		MoveToEdge ();
		if (contractionTimeRemaining <= 0) {
			ResetToNeutral ();
		}
	}

	void BeginExpansion(){
		expanding = true;
		expansionTimeRemaining = maxExpandTime;
		GetComponent<SpriteRenderer> ().color = Color.red;
	}

	void BeginContraction(){
		CircleCollider2D collider = GetComponent<CircleCollider2D> ();
		if (playerNum == 1) {
			targetEdge = collider.bounds.max.x;
		} else if (playerNum == 2) {
			targetEdge = collider.bounds.min.x;
		}
		expanding = false;
		expansionTimeRemaining = 0;
		contracting = true;
		contractionTimeRemaining = expansionTimeElapsed * contractionTimeFactor;
		Debug.Log (contractionTimeRemaining);
		GetComponent<SpriteRenderer> ().color = Color.blue;
	}

	void ResetToNeutral(){
		expansionTimeRemaining = 0;
		contractionTimeRemaining = 0;
		expanding = false;
		contracting = false;
		transform.localScale = Vector3.one;
		GetComponent<SpriteRenderer> ().color = Color.white;
	}

	void MoveToEdge(){
		Vector2 newPosition = transform.position;
		CircleCollider2D collider = GetComponent<CircleCollider2D> ();
		if (playerNum == 1) {
			newPosition.x = targetEdge - collider.bounds.extents.x;
		} else if (playerNum == 2) {
			newPosition.x = targetEdge + collider.bounds.extents.x;
		}
		transform.position = newPosition;
	}

}
