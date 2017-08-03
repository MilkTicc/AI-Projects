using UnityEngine;
using System.Collections.Generic;
using System.Collections;


namespace AISandbox {
    [RequireComponent(typeof (IActor))]
    public class PursuitController : MonoBehaviour {
		private const float SPEED_MIN = 10.0f;
		private const float SPEED_MAX = 10.0f;
		private const float DIR = 1.0f;
		private const float MAX_SPEED= 15.0f;
        private IActor _actor;

		private Vector3 steer;
		private const float PURSUEDIS = 15.0f;


	

		private void Awake() {
            _actor = GetComponent<IActor>();

        }

		private List<IActor> GetTargetActors(){
			List<IActor> target = new List<IActor>() ;
			foreach (IActor actor in Flocking.allActors) {
				if ((actor.Position  - _actor.Position).magnitude < PURSUEDIS) {
					target.Add(actor);
				}
				if ((new Vector2 (actor.Position.x,actor.Position.y)  - _actor.Position).magnitude > PURSUEDIS) {
					target.Remove(actor);
				}

			}
			return target;
		}

		private Vector2 Pursue(){
			Vector2 pursue = Vector2.zero ;
			Vector2 targetPos = Vector2.zero;
			Vector2 targetVelo = Vector2.zero;
			foreach (IActor actor in GetTargetActors()) {
				targetPos += actor.Position;
				targetVelo += actor.Velocity;
			}
			Vector2 avtargetPos = targetPos / GetTargetActors ().Count;
			Vector2 avtargetVelo = targetVelo / GetTargetActors ().Count;
			float dis = (avtargetPos - _actor.Position).magnitude;
			float T = dis / MAX_SPEED;
			Vector2 avTarPredPos = avtargetPos + avtargetVelo * T;
			Vector2 dir = (avTarPredPos - _actor.Position).normalized;
			pursue = dir * MAX_SPEED - _actor.Velocity;
			pursue = (GetTargetActors ().Count != 0) ? pursue : Vector2.zero;
			return pursue;
		}

        private void FixedUpdate ()
		{
			steer = Pursue ();
				
			// Pass all parameters to the character control script.
			_actor.SetInput (steer.x, steer.y );
		}

	}
}