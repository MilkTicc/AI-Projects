using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox{
public class SadState : IStates {
	readonly TreasureFinderController tFC;
	public string Name{get{return "Can't get the treasure. \nSad...";}}
	// Use this for initialization
	public SadState(TreasureFinderController tfc){
		tFC = tfc;
			
	}
	public void Enter(){
				
			}

	public void Exit () {
		
	}
	
	// Update is called once per frame
	public void Update () {
		Debug.Log (" I am so sad.");	
		tFC.GetComponent<SpriteRenderer>().color = new Color (0,0,0, 1.0f);
	}
}}

