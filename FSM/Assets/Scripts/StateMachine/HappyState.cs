using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox{
public class HappyState : IStates {
	readonly TreasureFinderController tFC;
		const float COLOR_RANGE = 1.0f;
	public string Name{get{return "Found the Treasure!\nSo Happy!";}}
	// Use this for initialization
	public HappyState(TreasureFinderController tfc){
		tFC = tfc;
			
	}
	public void Enter(){
				
			}

	public void Exit () {
		
	}
	
	// Update is called once per frame
	public void Update () {
		tFC.GetComponent<SpriteRenderer>().color = new Color (Random.Range(0, COLOR_RANGE), Random.Range(0, COLOR_RANGE),Random.Range(0, COLOR_RANGE), 1.0f);
	}
}}

