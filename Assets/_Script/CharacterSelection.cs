using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterSelection : MonoBehaviour 
{
	public int index;

	public List<GameObject> objects;
	public List<GameObject> Uidragons;
	private GameObject dragon;

	void Update() 
	{
		
	}

	void Start()
	{
		index = objects.IndexOf(objects.First (a=>a.activeSelf));
	}

	public void Next()
	{
		objects [index].SetActive (false);
		Uidragons [index].SetActive (false);
		index = index + 1 != objects.Count ? index + 1 : 0; 

		Uidragons [index].SetActive (true);
		 objects [index].SetActive (true);
		dragon =objects[index];
		positionManager.Instance.dragon = dragon;
	}

	public void Previous()
	{
		objects [index].SetActive (false);
		Uidragons [index].SetActive (false);
		index = index - 1 < 0 ? objects.Count-1 : index-1;
	//	objects [index].SetActive (true);
		Uidragons [index].SetActive (true);
		objects [index].SetActive (true);
		dragon =objects[index];
		positionManager.Instance.dragon = dragon;
	}
}