using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildController : MonoBehaviour 
{
	List<GameObject> children = new List<GameObject>();
	//bool initialized;
	void Start() {
		Setup ();
	}

	[ContextMenu("Setup")]
	void Setup()
	{
		foreach (Transform child in transform) 
		{
			children.Add (child.gameObject);	
		}	
	}

	[ContextMenu("True")]
	void EnableChildren()
	{
		SetActive (true);
	}

	[ContextMenu("False")]
	void DisableChildren()
	{
		SetActive (false);
	}

	public void SetActive(bool activeSelf)
	{
		foreach (var child in children) 
		{
			child.SetActive (activeSelf);	
		}
	}
}
