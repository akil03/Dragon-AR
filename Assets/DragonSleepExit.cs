using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSleepExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		positionManager.Instance.AwakeAnimationCalled ();
		//fetchscriptTest.Instance.hit = false;
	}
}
