using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public class SadState : IStates
	{
		OrientedActor self;

		public SadState (OrientedActor actor)
		{
			self = actor;
		}
		// Use this for initialization
		public void Enter ()
		{
			self.SetVelocity (0, 0);
			self._renderer.color = new Color (0, 0, 0);
		}
		public void ResetPathColor ()
		{ }
		public void Exit () { }
		// Update is called once per frame
		public void Update ()
		{

		}
	}
}