using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaclespawn : MonoBehaviour {

	public GameObject cubePrefab;
	public Transform cameraPosition;

	void Start () {
		CubeSpawn ();
	}

	void Update () {
		
	}

	void CubeSpawn() {
		transform.position = new Vector3(Random.Range(cameraPosition.position.x,(cameraPosition.position.x + (Random.Range(2,-2)))),transform.position.y,Random.Range(cameraPosition.position.z,(cameraPosition.position.z + (Random.Range(2,-2)))));
		Instantiate(cubePrefab,transform,false);
	}
}
