using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AISandbox {
    [RequireComponent(typeof (IActor))]
    public class FlockingController : MonoBehaviour {
        public float neighborDistance = 5.0f;
        public float separationWeight = 1.0f;
        public float alignmentWeight = 0.5f;
        public float cohesionWeight = 0.5f;
		public LineRenderer neighborLine1;
		public LineRenderer neighborLine2;
		public LineRenderer neighborCircle;
		public LineRenderer sepLine;
		public LineRenderer aliLine;
		public LineRenderer cohLine;
		public LineRenderer evaLine;
        private IActor _actor;
		public SimpleActor pursuer;
		//private IActor[] allActors;
		private const float NEIGHBORANGLE = 2.5f;
		private const float ACCELERATION = 2.0f;
		private const float EVADEDIS = 10.0f;
		public static bool flockStatus = false;
		public bool flocking = false;
		public bool evading = false;
		private float evadefactor;
		Vector2 steering = Vector2.zero ;
		int size;
		float theta_scale = 0.01f;
		//public bool debuging = false;

        private void Start() {
            _actor = GetComponent<IActor>();
			Flocking.allActors = FindObjectsOfType<OrientedActor> ();

			float sizeValue = (1.0f *  NEIGHBORANGLE) / theta_scale;
			size = (int)sizeValue;
			size++;
			neighborCircle.numPositions =size;
			neighborCircle.gameObject.SetActive(false);
			sepLine.gameObject.SetActive(false);
			aliLine.gameObject.SetActive(false);
			cohLine.gameObject.SetActive(false);
			evaLine.gameObject.SetActive(false);

        }

		private List<IActor> GetNeighborActors(){
			List<IActor> neighbor = new List<IActor>() ;
			foreach (IActor actor in Flocking.allActors) {
				if ((actor.Position  - _actor.Position).magnitude < neighborDistance) {
					neighbor.Add(actor);
				}
				if ((new Vector2 (actor.Position.x,actor.Position.y)  - _actor.Position).magnitude > neighborDistance) {
					neighbor.Remove(actor);
				}
				if (Vector2.Angle ((actor.Position - _actor.Position), _actor.Velocity) > NEIGHBORANGLE && Mathf.PI * 2.0f - NEIGHBORANGLE > Vector2.Angle ((actor.Position - _actor.Position), _actor.Velocity)) {
					neighbor.Remove (actor);
				}
				if (neighbor.Contains(_actor)){
					neighbor.Remove(_actor);
				}
			}
			return neighbor;
		}
	
		private Vector2 Separation(){
			Vector2 separation = Vector2.zero ;
			foreach (IActor actor in GetNeighborActors()) {
				Vector2 posDiff = actor.Position - _actor.Position;
				float T = 1.0f - (posDiff.magnitude / neighborDistance);
				separation += -posDiff * T * T;
			}
			separation = (GetNeighborActors ().Count != 0) ? separation : Vector2.zero;
			return separation;
		}

		private Vector2 Alignment(){
			Vector2 alignment= Vector2.zero ;
			foreach (IActor actor in GetNeighborActors()) {
				alignment += actor.Velocity.normalized;
			}

			alignment = (GetNeighborActors ().Count !=0) ? alignment / GetNeighborActors ().Count - _actor.Velocity.normalized: Vector2.zero ;
			return alignment * 5.0f;
		}

		private Vector2 Cohesion(){
			Vector2 cohesion= Vector2.zero ;
			Vector2 avPos = Vector2.zero ;
			foreach (IActor actor in GetNeighborActors()) {
				avPos += actor.Position;

			}
			avPos = avPos / GetNeighborActors ().Count;
			cohesion = (GetNeighborActors ().Count !=0) ? avPos - _actor.Position : Vector2 .zero;
			return cohesion;
		}



		private Vector2 Evade(){
			Vector2 evade;
			if (evading) {
				Vector2 dis = pursuer.Position - _actor.Position;
				evadefactor = dis.magnitude < EVADEDIS ? Mathf.Sqrt(1.0f - (dis.magnitude / EVADEDIS)): 0f;
				evade = -dis.normalized * 10.0f;
			}
			else
			{
				evadefactor = 0f;
				evade = Vector2.zero;
			}

			return evade;
		}
			
		public void DrawNeighborLine(bool debuging){

			float X = _actor.Velocity.normalized.x * neighborDistance;
			float Y = _actor.Velocity.normalized.y * neighborDistance;
			float cos = Mathf.Cos (NEIGHBORANGLE);
			float sin = Mathf.Sin (NEIGHBORANGLE);

			neighborLine1.transform.rotation = Quaternion.identity;
			neighborLine1.SetPosition (1, new Vector3 (X * cos + Y * sin, Y * cos - X * sin, 0.0f));
			neighborLine2.transform.rotation = Quaternion.identity;
			neighborLine2.SetPosition (1, new Vector3 (X * cos - Y * sin, Y * cos + X * sin, 0.0f));



			Vector3 pos;
			int a = _actor.Velocity.y < 0 ? -1 : 1;
			float theta =  a * Vector2.Angle( Vector2.right, _actor.Velocity ) * Mathf.Deg2Rad - NEIGHBORANGLE;
			for (int i = 0; i < size; i++) {          
				theta += (2.0f* theta_scale);         
				float x = neighborDistance * Mathf.Cos (theta);
				float y = neighborDistance * Mathf.Sin (theta);          
				//x += _actor.Position.x;
				//y += _actor.Position.y;
				pos = new Vector3 (x, y, 0);
				neighborCircle.transform.rotation = Quaternion.identity;
				neighborCircle.SetPosition (i, pos);
			}
			evaLine.gameObject.SetActive (debuging);
			sepLine.gameObject.SetActive (debuging);
			aliLine.gameObject.SetActive (debuging);
			cohLine.gameObject.SetActive (debuging);
			neighborCircle.gameObject.SetActive (debuging);
			neighborLine1.gameObject.SetActive (debuging);
			neighborLine2.gameObject.SetActive (debuging);
		}

		private void DebugPrint()
		{
//			print (Vector2.Angle( _actor.Velocity,Vector2.right ));
//			print (Alignment () * alignmentWeight);
//			print (Cohesion () * cohesionWeight);
			//print(Screen.width);
		}

		private void ReRandom(){
			
				Vector2 randomvelo = new Vector2 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f)).normalized * _actor.MaxSpeed;
				_actor.SetVelocity (randomvelo.x, randomvelo.y);
				steering = Vector2.zero;
		}

		private void DebugLines(){
			sepLine.transform.rotation = Quaternion.identity;
			aliLine.transform.rotation = Quaternion.identity;
			cohLine.transform.rotation = Quaternion.identity;
			evaLine.transform.rotation = Quaternion.identity;
			Vector2 ser = flocking ? Separation () * separationWeight *(1.0f - evadefactor) : Vector2.zero;
			Vector2 ali = flocking ? Alignment () * alignmentWeight *(1.0f - evadefactor): Vector2.zero;
			Vector2 coh = flocking ? Cohesion () * cohesionWeight *(1.0f - evadefactor): Vector2.zero;
			Vector2 eva = evading ? Evade () * evadefactor : Vector2.zero;
			sepLine.SetPosition (1, ser);
			aliLine.SetPosition (1, ali);
			cohLine.SetPosition (1, coh);
			evaLine.SetPosition (1, eva);

		}

        private void Update() {
			DebugLines ();
			evading = Flocking.Evading;
			if (Input.GetKeyDown (KeyCode.Tab) ) {
				if (flocking == true) {
					ReRandom ();
				}
				flocking = !flocking;
				flockStatus = flocking;
				//print (flockStatus );
			}
					
			if(flocking == true)
				steering = (Separation() * separationWeight + Alignment()* alignmentWeight + Cohesion()* cohesionWeight)*(1.0f - evadefactor) + Evade() * (evadefactor);
			
            _actor.SetInput( steering.x, steering.y );
//			if (Input.GetKeyDown ("up")) {
//				DebugPrint ();
//			}

        }
    }
}