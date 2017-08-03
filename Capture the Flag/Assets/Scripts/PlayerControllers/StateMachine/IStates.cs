using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox {
	public interface IStates {

		void ResetPathColor ();
		 void Enter() 	;    
		 void Update()     ;     
		void Exit ();

	}
}