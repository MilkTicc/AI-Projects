using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public class HappyState : IStates
	{
		OrientedActor self;

		public HappyState(OrientedActor actor)
		{
			self=actor;
		}
		// Use this for initialization
		public void Enter ()
		{
			self.SetVelocity (0, 0);
		}
		public void ResetPathColor ()
		{ }
		public void Exit (){}
		// Update is called once per frame
		public void Update ()
		{

		}
	}
}