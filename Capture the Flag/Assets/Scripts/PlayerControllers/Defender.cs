using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AISandbox
{
	public class Defender : IJob
	{
		OrientedActor self;
		IStates currentState;
		public PatrolState patrolState;
		GridNode prevNode;
		List<GridNode> influencedNodes;
		public IStates CurrentState { get { return currentState; } set { currentState = value; } }
		
		public Defender(OrientedActor actor){
			self = actor;
			prevNode = self.currentNode;
			patrolState = new PatrolState (self, this);
			currentState = patrolState;
			influencedNodes = null;

				

			patrolState.Enter ();
		}
		// Use this for initialization
		void Start ()
		{
		}
		void Influence(){
			if(influencedNodes != null){
				foreach (GridNode nodes in influencedNodes) {
					nodes.CyanInfluence = 0;
					nodes.MangInfluence = 0;
					if (self._Team == OrientedActor.Team.mangetta)
						nodes.GetComponent<SpriteRenderer> ().color += new Color (0, 0.2f, 0);
				}
			}

			influencedNodes = self.grid.GetSurroundNodesByLevel (self.currentNode, 2);
			influencedNodes.AddRange (self.grid.GetSurroundNodesByLevel (self.currentNode, 1));
			if (self._Team==OrientedActor.Team.cyan)
			{
				foreach (GridNode nodes in influencedNodes)
				{
					nodes.CyanInfluence += 5; 
				}
				self.currentNode.CyanInfluence += 50;
			}
			else
			{
				foreach (GridNode nodes in influencedNodes) {
					nodes.MangInfluence += 5;
					nodes.GetComponent<SpriteRenderer>().color += new Color (0, -0.2f, 0);
				}
				self.currentNode.MangInfluence += 50;
				
			}
		}

		public void Captured (OrientedActor capturer)
		{
			currentState = new CapturedState (self, capturer);
		}

		public void ToHappy ()
		{
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

		public void DeInfluence(){
			foreach (GridNode nodes in influencedNodes) {
				nodes.MangInfluence =0;
				nodes.CyanInfluence = 0;
				if(self._Team==OrientedActor.Team.mangetta)
					nodes.GetComponent<SpriteRenderer> ().color += new Color (0, 0.2f, 0);
			}
		}
		// Update is called once per frame
		public void Update ()
		{   if(prevNode != self.currentNode){
				Influence ();

				prevNode = self.currentNode;
			}
			//Influence ();
			currentState.Update ();
		}
	}
}