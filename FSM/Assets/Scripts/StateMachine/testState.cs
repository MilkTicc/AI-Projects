using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox{
public class TestState : IStates {
	private readonly TreasureFinderController tFC;
	public string Name{get{return this.Name;}}
	// Use this for initialization
	public TestState(TreasureFinderController tfc){
		tFC = tfc;
		Debug.Log ("test start");

	}
	public void Enter(){
				
			}

	public void Exit () {
		
	}
	
	// Update is called once per frame
	public void Update () {
			//Debug.Log ("test start");	

	}
}}
