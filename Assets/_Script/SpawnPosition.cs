using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour {


	public GameObject target;
	ChildController controller;

	// Use this for initialization
	void Start () {
		transform.SetParent (null);
		controller = GetComponent<ChildController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void PickupObjects() {
		if (positionManager.Instance.UI == true) {
			controller.SetActive (false);
		} else {
			controller.SetActive (true);
		}
	}
}
