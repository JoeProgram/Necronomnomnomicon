using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ConsumeZone : MonoBehaviour {

	protected List<Critter> targetList;
	public Cth cth;
	public LayerMask mask;

	public Transform consumePoint; // had to leave this object off of cth, as it was affecting his physics.

	void Start(){
		targetList = new List<Critter> ();
	}

	void Update(){
		transform.position = consumePoint.transform.position;
	}

	void OnTriggerEnter2D( Collider2D other ){


		Debug.Log ("other: " + other);

		if( other.CompareTag("critter")){
			targetList.Add ( other.GetComponent<Critter>() );
		}
	}

	void OnTriggerExit2D( Collider2D other ){
		if( other.CompareTag("critter") && targetList.Contains(other.GetComponent<Critter>())){
			targetList.Remove( other.GetComponent<Critter>() );
		}
	}

	public int Consume(){

		int amountConsumed = 0;

		List<Critter> eaten = new List<Critter> ();

		foreach (Critter critter in targetList) {
			if( critter.CanBeConsumed(cth.hunger) ){
				amountConsumed += critter.Consumed();
				eaten.Add ( critter );
			}
		}

		targetList = targetList.Except (eaten).ToList ();

		return amountConsumed;
	}

}
