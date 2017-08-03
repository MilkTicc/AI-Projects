using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStates {

	string Name { get; }	 	

	 void Enter() 	;    
	 void Update()     ;     
	void Exit ();

}
