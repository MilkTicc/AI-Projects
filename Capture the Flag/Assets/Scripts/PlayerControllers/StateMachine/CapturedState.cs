using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public class CapturedState : IStates
	{
		readonly OrientedActor self;
		GridNode goalNode;
		GridNode tarNode;
		CapturetheFlag CtF;
		Flagger flagger;
		OrientedActor capturer;

		public CapturedState (OrientedActor actor , OrientedActor cap)
		{
			self = actor;
			self.captured = true;
			CtF = self.CtF;
			capturer = cap;
			if (cap != null) { 
				if (self._Team == OrientedActor.Team.cyan)
					{if (cap._Team != self._Team)
						self.PrintMessage ("Status: I am Captured!");
					else
						self.PrintMessage ("Status: I am being Rescued!");
				}
				self.jailed = false; }
			else {
				if (self._Team == OrientedActor.Team.cyan)
					{self.PrintMessage ("Status: I am in Jail"); }
				self.jailed = true;
				self.CallforHelp ();
			}
				
		}

		public void ResetPathColor ()
		{ }


		public void Enter ()
		{
			
		}
		public void Exit () { }
		public void Update ()
		{
			if (capturer != null)
				self.position = capturer.transform.position + new Vector3 (-0.5f, -0.5f, 0);
			else
				self.position = (self._Team == OrientedActor.Team.cyan) ? CtF.mangettaJail.transform.position : CtF.cyanJail.transform.position;
		}
	}
}