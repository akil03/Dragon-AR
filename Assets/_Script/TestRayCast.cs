using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRayCast : MonoBehaviour {
	public GameObject cube;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/3, Screen.height/4, 0));
		if (Physics.Raycast(ray, out hit)) {
			Vector3 objectHit = hit.point;
			if(hit.transform.gameObject!=cube)
				cube.transform.position = objectHit;
		}
	}
}
