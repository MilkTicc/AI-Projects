using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AISandbox
{
	public class Flagger : IJob
	{
		OrientedActor self;
		IStates currentState;
		public AttackState attackState;
		public ReturnState returnState;
		public IStates CurrentState{ get { return currentState;} set { currentState = value; } }

		public Flagger (OrientedActor actor)
		{
			self = actor;
			attackState = new AttackState (self,this);
			returnState = new ReturnState (self,this);
			currentState = attackState;
			attackState.Enter ();

		}

		public void Captured (OrientedActor capturer)
		{
			currentState.ResetPathColor ();

			currentState = new CapturedState (self, capturer);
		}

		public void ToHappy(){
			currentState.ResetPathColor ();
			currentState = new HappyState (self);
			currentState.Enter ();
		}
		public void ToSad ()
		{
			currentState.ResetPathColor ();
			currentState = new SadState (self); 
			currentState.Enter ();
		}
		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		public void Update ()
		{
			currentState.Update ();
		}
	}
}