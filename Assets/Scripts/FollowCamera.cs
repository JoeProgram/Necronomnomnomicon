using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	public Transform target;
	protected Vector3 targetDistance;

	// Use this for initialization
	void Start () {
		targetDistance = target.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.transform.position - targetDistance;
	}
}
