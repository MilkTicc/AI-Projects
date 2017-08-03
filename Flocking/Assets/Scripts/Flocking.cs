using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AISandbox {
    public class Flocking : MonoBehaviour {
        private const float SPAWN_RANGE           = 10.0f;
		private const int SPAWN_NUM = 20;
        public FlockingController _flockingActorPrefab;
		public SimpleActor _pursuitActorPrefab;
        public Slider _neighborDistanceSlider;
        public Slider _separationSlider;
        public Slider _alignmentSlider;
        public Slider _cohesionSlider;
		public GameObject FlockStatus;
		public GameObject DebugStatus;
		public GameObject EvadeStatus;
		public static IActor[] allActors;
		public static bool Evading;
        private Queue<FlockingController> _flock = new Queue<FlockingController>();
		private int count= 0;
		private bool debuging = false;
		private FlockingController lastactor;
		//private List<FlockingController> allActors;

        private FlockingController CreateFlockingActor() {
            FlockingController newActor = Instantiate<FlockingController>(_flockingActorPrefab);
            newActor.gameObject.name = "Flocking Actor";
            newActor.transform.position = new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0.0f );
            newActor.GetComponent<OrientedActor>().initialVelocity = Random.onUnitSphere * Random.Range( 5.0f, newActor.GetComponent<IActor>().MaxSpeed );
            newActor.gameObject.SetActive(true);
			newActor.GetComponent<FlockingController> ().flocking = FlockingController.flockStatus;
			//newActor.GetComponent<FlockingController> ().pursuer = FindObjectOfType<SimpleActor> ();
			count += 1;
		//	allActors.Add (newActor);
            return newActor;
		}

		private void SpawnPursuer(){
			if (!Evading) {

				SimpleActor pursuer = Instantiate<SimpleActor> (_pursuitActorPrefab);
				pursuer.gameObject.name = "Pursue Actor";
				pursuer.transform.position = new Vector3 (Random.Range (-SPAWN_RANGE, SPAWN_RANGE), Random.Range (-SPAWN_RANGE, SPAWN_RANGE), 0.0f);
				float speed = 15.0f;
				Vector2 tarVelo = new Vector2 (Random.Range (-1f, 1f), Random.Range (-1f, 1f)).normalized * speed;
				pursuer.initialVelocity = new Vector2 (tarVelo.x, tarVelo.y);
				pursuer.gameObject.SetActive (true);

				foreach (FlockingController actor in _flock) {
					actor.pursuer = pursuer;
				}
			}

			if (Evading ) {
				SimpleActor pursuer = FindObjectOfType<SimpleActor> ();
				Destroy (pursuer.gameObject);
			}
		}

		private void DeleteFlockingActor(){
//			int ran = Random.Range (0, count - 1);
			FlockingController randomactor = _flock.ToArray ()[0];
			Destroy(randomactor.gameObject);
			_flock.Dequeue ();
			count -= 1;
		}

        private void Start() {
			
            _neighborDistanceSlider.value = _flockingActorPrefab.neighborDistance;
            _separationSlider.value = _flockingActorPrefab.separationWeight;
            _alignmentSlider.value = _flockingActorPrefab.alignmentWeight;
            _cohesionSlider.value = _flockingActorPrefab.cohesionWeight;

            _neighborDistanceSlider.onValueChanged.AddListener( OnNeighborDistanceSliderValueChanged );
            _separationSlider.onValueChanged.AddListener( OnSeparationSliderValueChanged );
            _alignmentSlider.onValueChanged.AddListener( OnAlignmentSliderValueChanged );
            _cohesionSlider.onValueChanged.AddListener( OnCohesionSliderValueChanged );

			for( int i=0; i<SPAWN_NUM; ++i ) {
                _flock.Enqueue( CreateFlockingActor() );
            }
        }

        private void OnNeighborDistanceSliderValueChanged(float neighborDistance) {
            foreach( FlockingController actor in _flock ) {
                actor.neighborDistance = neighborDistance;
            }
        }

        private void OnSeparationSliderValueChanged(float separationWeight) {
            foreach( FlockingController actor in _flock ) {
                actor.separationWeight = separationWeight;
            }
        }

        private void OnAlignmentSliderValueChanged(float alignmentWeight) {
            foreach( FlockingController actor in _flock ) {
                actor.alignmentWeight = alignmentWeight;
            }
        }

        private void OnCohesionSliderValueChanged(float cohesionWeight) {
            foreach( FlockingController actor in _flock ) {
                actor.cohesionWeight = cohesionWeight;
            }
        }


		private bool GetFlockStatus(){
			return  FlockingController.flockStatus ;
		}
			

		private void ShowDebug(bool debug){
			FlockingController randomactor = _flock.ToArray() [Random.Range (0, count - 1)];

			if (debug) {
				randomactor.DrawNeighborLine (true);
				lastactor = randomactor;
			}
			if (!debug )
				lastactor .DrawNeighborLine (false);
		}

		private void TextControl(){
			if (GetFlockStatus())
				FlockStatus.GetComponent<Text>().text = "Flocking: <b><color=green>True</color></b>";
			if (!GetFlockStatus())
				FlockStatus.GetComponent<Text>().text = "Flocking: <b><color=red>False</color></b>";
			if(!debuging)
				DebugStatus.GetComponent<Text>().text = "Debuging: <b><color=red>False</color></b>";
			if(debuging)
				DebugStatus.GetComponent<Text>().text = "Debuging: <b><color=green>True</color></b>";
			if(!Evading)
				EvadeStatus.GetComponent<Text>().text = "Pursuing: <b><color=red>False</color></b>\nCurrent Count: " + count  ;
			if(Evading)
				EvadeStatus.GetComponent<Text>().text = "Pursuing: <b><color=green>True</color></b>\nCurrent Count: " + count  ;

			GameObject[] Texts = GameObject.FindGameObjectsWithTag ("Text");
			foreach (GameObject text in Texts) {
				text.transform.position = new Vector2 (Screen.width - 162, text.transform.position.y);
			}

		}

		private void Update(){

			allActors = FindObjectsOfType<OrientedActor> ();

			if (Input.GetKeyDown (KeyCode.Equals)) {
				_flock.Enqueue( CreateFlockingActor() );
			}
			if (Input.GetKeyDown (KeyCode.Minus)) {
				DeleteFlockingActor ();
			}

			if (Input.GetKeyDown ("space")) {
				SpawnPursuer ();
				Evading  = !Evading;
			}

			if (Input.GetKeyDown (KeyCode.LeftShift) ) {
				debuging = !debuging;
				ShowDebug(debuging);


			}
			TextControl ();

		}

    }
}