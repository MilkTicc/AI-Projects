using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AISandbox
{
	public class Rescuer : IJob
	{
		OrientedActor self;
		OrientedActor teamMate;
		IStates currentState;
		RescueState rescueState;
		public IStates CurrentState { get { return currentState; } set { currentState = value; } }

		public Rescuer (OrientedActor actor, OrientedActor teammate)
		{
			self = actor;
			teamMate = teammate;
			rescueState = new RescueState (self, teamMate);
			currentState = rescueState;
			currentState.Enter ();
		}

		public void Captured (OrientedActor capturer)
		{
			currentState = new CapturedState (self, capturer);
		}
		// Use this for initialization
		void Start ()
		{

		}
		public void ToHappy ()
		{
			currentState.ResetPathColor ();
			currentState = new HappyState (self);
		}
		public void ToSad ()
		{
			currentState.ResetPathColor ();
			currentState = new SadState (self);
		}
		// Update is called once per frame
		public void Update ()
		{
			currentState.Update ();
		}
	}
}