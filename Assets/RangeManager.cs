using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour {

	public Transform targetlookAt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		print ("Called");
		if(other.gameObject.tag == "Player"){
			other.gameObject.transform.LookAt (targetlookAt);
		}
	}

	void OnCollisionEnter(Collision collision) {
		print ("collusion");
	}
}
