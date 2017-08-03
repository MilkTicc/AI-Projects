using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public interface IJob
	{
		IStates CurrentState { get; set;}
		void Captured (OrientedActor cap);
		void ToHappy ();
		void ToSad ();
		void Update ();
	}
}