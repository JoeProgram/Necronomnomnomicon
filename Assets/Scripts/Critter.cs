using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Critter : MonoBehaviour {

	protected List<Vector3> locations;

	public float speed;
	public float waitTime;

	public int size;
	public int nutrition;

	protected Cth cth;

	private Vector3 chosenLocation;

	void Start(){

		locations = new List<Vector3> ();

		foreach (Transform child in transform) {
			if( child.name == "location" ) locations.Add(child.position);
		}

		ChooseLocation ();

		cth = GameObject.FindWithTag ("cth").GetComponent<Cth> ();
		Debug.Log ("cth is :" + cth);
		cth.Grown += CheckForConsumable;
		CheckForConsumable (cth);
	}

	private void ChooseLocation(){
			
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;

		int index = Random.Range(0,locations.Count);

		if( chosenLocation == locations[index] ) index = (index + 1) % locations.Count;

		chosenLocation = locations[index];

		FaceDirection ();
		TravelToLocation();
	}

	protected void TravelToLocation(){
		iTween.MoveTo (this.gameObject, iTween.Hash ("speed", speed, "position", chosenLocation, "easetype", iTween.EaseType.linear,"oncomplete","Arrived"));
	}

	protected void Arrived(){
		StartCoroutine (ArrivedHelper ());
	}

	protected IEnumerator ArrivedHelper(){
		yield return new WaitForSeconds (waitTime);
		ChooseLocation ();
	}

	private void FaceDirection(){
		Vector2 direction = (chosenLocation - transform.position).normalized;
		transform.localScale = new Vector3(Mathf.Sign(direction.x) * -Mathf.Abs( transform.localScale.x ),transform.localScale.y,transform.localScale.z);
	}

	public bool CanBeConsumed( int hunger ){
		return hunger >= size;
	}

	public int Consumed(){

		GetComponent<Collider2D>().enabled = false;
		GetComponent<Renderer>().enabled = false;
		Destroy (this.gameObject,1.0f);
		iTween.Stop (this.gameObject);
		cth.Grown -= CheckForConsumable;

		return nutrition;
	}	

	void OnTriggerEnter2D( Collider2D other ){
		Debug.Log ("Bug trigger entered");
	}

	protected void CheckForConsumable(Cth cth ){

		if( CanBeConsumed(cth.hunger))  GetComponent<Animator> ().SetTrigger ("ReadyToConsume");
	}
	
}
