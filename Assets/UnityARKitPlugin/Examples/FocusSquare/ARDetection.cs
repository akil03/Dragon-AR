using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ARDetection : MonoBehaviour {

	public GameObject focusSquare, focusManager;
	public GameObject SpawnObject;
	bool isSpawned;

	public GameObject Plane;
	//public GameObject cubeSpawner;

	void Start () {
		
	}

	void Update () {
		
	}

	public void Summon(){
		if (!isSpawned && focusSquare.activeSelf) {
			SpawnObject.SetActive (true);
			focusManager.SetActive (false);
			//SpawnObject.transform.position = focusSquare.transform.position;
			Plane.transform.SetParent(null);
			FollowUser.height = focusSquare.transform.position.y;
			isSpawned = true;
		}
	}

	public void ResetAR(){
		SpawnObject.SetActive (false);
		focusManager.SetActive (true);
		isSpawned = false;
	}
}
