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
	private float stunTimeRemaining;
	public float kbToStunRatio;
	private float defaultRadius;
	private float targetEdge;
	private Rigidbody2D rb;
	private int damage;
	public float baseKnockback;
	public float knockbackGrowth;
	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		defaultRadius = GetComponent<CircleCollider2D> ().bounds.extents.x;
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.gameOver) {
			if (stunTimeRemaining > 0) {
				stunTimeRemaining -= Time.deltaTime;
				if (stunTimeRemaining <= 0) {
					ResetToNeutral ();
				}
			} else {
				ProcessInput ();
				if (expanding) {
					Expand ();
				} else if (contracting) {
					Contract ();
				}
			}
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
		rb.velocity = Vector2.zero;
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

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.tag == "Player") {
			if (collider.gameObject.GetComponent<PlayerController> ().expanding) {
				GetHit ();
			}
		} else if (collider.gameObject.tag == "Wall") {
			gameManager.EndGame (playerNum);
		}
	}

	void GetHit(){
		damage += 1;
		float knockbackMagnitude = baseKnockback + (damage * knockbackGrowth);
		stunTimeRemaining = kbToStunRatio * knockbackMagnitude;
		if (playerNum == 1) {
			knockbackMagnitude *= -1;
		}
		rb.velocity =  knockbackMagnitude * Vector2.right;
		GetComponent<SpriteRenderer> ().color = Color.grey;
	}

}
