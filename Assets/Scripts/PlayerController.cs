using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int playerNum;
	private float expansionTimeRemaining;
	private float contractionTimeRemaining;
	public float maxExpansionSize;
	public float maxExpandTime;
	public float maxContractTime;
	private bool expanding;
	private bool contracting;
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
		} else if (expanding) {
			BeginContraction ();
		} else if (contractionTimeRemaining > 0) {
			Contract ();
			contractionTimeRemaining -= Time.deltaTime;
		} else if (contracting) {
			ResetToNeutral ();
		}
	}

	void ProcessInput(){
		if (Input.GetButtonDown("Shalom_P" + playerNum)){
			if (expanding || contracting) {
				MoveToEdge ();
				ResetToNeutral ();
			} else {
				BeginExpansion ();	
			}
		}
	}

	void Expand(){
		transform.localScale = Vector3.Lerp (Vector3.one, maxExpansionSize * Vector3.one, 1 - expansionTimeRemaining / maxExpandTime);
	}

	void Contract(){
		transform.localScale = Vector3.Lerp (maxExpansionSize * Vector3.one, Vector3.one, 1 - contractionTimeRemaining / maxContractTime);
	}

	void BeginExpansion(){
		expanding = true;
		expansionTimeRemaining = maxExpandTime;
		GetComponent<SpriteRenderer> ().color = Color.red;
	}

	void BeginContraction(){
		expanding = false;
		contracting = true;
		contractionTimeRemaining = maxContractTime;
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
		if (playerNum == 1) {
			newPosition.x = GetComponent<CircleCollider2D> ().bounds.max.x - defaultRadius;
		} else if (playerNum == 2) {
			newPosition.x = GetComponent<CircleCollider2D> ().bounds.min.x + defaultRadius;
		}
		//rb.MovePosition(newPosition);
		transform.position = newPosition;
	}

}
