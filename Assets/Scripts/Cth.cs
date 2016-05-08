using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void GrowEventHandler(Cth cth);

public class Cth : MonoBehaviour {

	public float maxSpeed;
	public float moveForce;
	public float tentacleForce;

	public LayerMask jumpableLayers;

	public float stuckThreshold; // how slow it should be moving before the stuck rotate kicks in
	public float stuckRotateSpeed; // how fast it should rotate when trying to move against a wall

	public float forcePosition = 0.1f;

	private Transform tentaclePoint;
	public ConsumeZone consumeZone;

	public AudioClip sfxJump;
	public AudioClip sfxFailedJump;
	public AudioClip sfxLevitating;
	public AudioClip sfxConsume;
	public AudioClip sfxFailedConsume;
	public AudioClip sfxComplete;

	private float originalGravityScale;
	private float originalLinearDrag;
	public float levitatingDrag;
	private bool isLevitating = false;

	public int hunger = 0;
	public float scalePerHunger = 0.1f;
	public float forcePerHunger = 25;

	public LayerMask critterMask;

	public List<Transform> consumePoints;

	public event GrowEventHandler Grown;

	public int targetHunger;
	public SpriteRenderer complete;
	public SpriteRenderer blackout;

	// Use this for initialization
	void Start () {

		iTween.ValueTo(this.gameObject, iTween.Hash ("from", 1.0f, "to", 0.0f, "time", 3, "easetype", "linear", "onupdate","SetBlackoutAlpha"));

		
		originalLinearDrag = GetComponent<Rigidbody2D>().drag;
		originalGravityScale = GetComponent<Rigidbody2D>().gravityScale;
		tentaclePoint = transform.FindChild("tentacle_point").transform;

	}
	 

	void Update(){
		if( Input.GetButtonDown("Jump") ){
				
			GetComponent<Animator>().SetTrigger( "Jump" );
			if( Physics2D.Linecast(transform.position, tentaclePoint.position, jumpableLayers)){
				GetComponent<Rigidbody2D>().AddForce( (transform.position - tentaclePoint.position).normalized * (tentacleForce + hunger * forcePerHunger));
				AudioSource.PlayClipAtPoint( sfxJump, Vector3.zero );
			} else {
				AudioSource.PlayClipAtPoint( sfxFailedJump, Vector3.zero );
			}
		}

		if (Input.GetButtonDown ("Levitate")) {
			GetComponent<Rigidbody2D>().gravityScale = 0;
			GetComponent<Rigidbody2D>().drag = levitatingDrag;
			isLevitating = true;
			GetComponent<AudioSource>().clip = sfxLevitating;
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().Play();
		} else if(Input.GetButtonUp ("Levitate")){
			GetComponent<Rigidbody2D>().gravityScale = originalGravityScale;
			GetComponent<Rigidbody2D>().drag = originalLinearDrag;
			isLevitating = false;
			GetComponent<AudioSource>().Stop ();
		}

		if( Input.GetButtonDown("Consume")){
			int amount = Consume();
			if( amount > 0 ){
				Grow (amount);
				AudioSource.PlayClipAtPoint( sfxConsume, Vector3.zero );
			} else {
				AudioSource.PlayClipAtPoint( sfxFailedConsume, Vector3.zero );
			}
		}

		if (Input.GetKeyDown (KeyCode.R)) Application.LoadLevel (Application.loadedLevel);
	}


	// Update is called once per frame
	void FixedUpdate () {
	
		float horizontal = Input.GetAxis("Horizontal");

		if( !isLevitating){

			if(horizontal * GetComponent<Rigidbody2D>().velocity.x < maxSpeed) GetComponent<Rigidbody2D>().AddForceAtPosition(Vector2.right * horizontal * moveForce,(Vector2)transform.position + (Vector2.up * forcePosition) );
			if( GetComponent<Rigidbody2D>().velocity.x > maxSpeed ) GetComponent<Rigidbody2D>().velocity = new Vector2( Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		}

		// if its against a wall, allow the player to rotate into a good jumping position
		if (GetComponent<Rigidbody2D>().velocity.magnitude <= stuckThreshold || isLevitating) {
			transform.Rotate (Vector3.forward * -horizontal * stuckRotateSpeed);
		}

	}

	public int Consume(){


		int amountConsumed = 0;

		List<Critter> inRange = new List<Critter> ();
		List<Critter> eaten = new List<Critter> ();

		for (int i = 0; i < consumePoints.Count - 1; i++) {
		RaycastHit2D hitInfo = Physics2D.Linecast( consumePoints[i].position, consumePoints[i+1].position, critterMask );
			if( hitInfo.collider != null && !inRange.Contains(hitInfo.collider.GetComponent<Critter>())){
				inRange.Add( hitInfo.collider.GetComponent<Critter>());
			}
		}

		foreach (Critter critter in inRange) {
			if( critter.CanBeConsumed(hunger) ){
				amountConsumed += critter.Consumed();
				eaten.Add ( critter );
			}
		}

		return amountConsumed;
	}

	public void Grow( int consumed ){

		hunger += consumed;
		iTween.ScaleTo (gameObject, Vector3.one + Vector3.one * hunger * scalePerHunger, 0.5f);
		if(Grown != null ) Grown(this);

		if (hunger >= targetHunger) {
			StartCoroutine(FinishLevel());
		}

	}

	public IEnumerator FinishLevel(){

		iTween.ScaleTo (complete.gameObject, iTween.Hash ("scale",Vector3.one,"time", 1, "easetype", iTween.EaseType.easeOutBack));
		AudioSource.PlayClipAtPoint (sfxComplete, Vector3.zero);
		yield return new WaitForSeconds (3);
		iTween.ValueTo(this.gameObject, iTween.Hash ("from", 0.0f, "to", 1.0f, "time", 3, "easetype", "linear", "onupdate","SetBlackoutAlpha"));
		yield return new WaitForSeconds (4);



		Application.LoadLevel ((Application.loadedLevel + 1) % Application.levelCount );

	}

	protected void SetBlackoutAlpha( float val ){
		Color c = blackout.GetComponent<SpriteRenderer> ().color;
		blackout.GetComponent<SpriteRenderer> ().color = new Color (c.r, c.g, c.b, val);
	}
	
}
