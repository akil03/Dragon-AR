using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class charSelection: MonoBehaviour 
{
	public int index;
	public Text numberText;

	public List<string> names;
	public Animator dragonAnimator;

	void Update() 
	{
		numberText.text = index.ToString ();
	}
	void Start()
	{
		
	}
	public void Next()
	{
		
		index = index + 1 != names.Count ? index + 1 : 0; 
		dragonAnimator.Play (names[index]);
	}
	public void Previous()
	{
		index = index - 1 < 0 ? names.Count-1 : index-1;
		dragonAnimator.Play (names[index]);
	}
}