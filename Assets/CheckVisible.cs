using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckVisible : MonoBehaviour {

	public bool Visible = false;

	// Use this for initialization
	void Start () {
		
	}

	void Update () {
		if (GetComponent<MeshRenderer> ().isVisible) { 
			Visible = true;
		} else {
			Visible = false;
		}
	}
}

