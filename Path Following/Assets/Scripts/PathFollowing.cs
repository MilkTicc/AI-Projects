using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AISandbox{
	public class PathFollowing : MonoBehaviour {
		private const float SPAWN_RANGE = 20f;
		private const int SPAWN_NUM = 10;
		private int GRIDLENGTH = 32;
		private int GRIDHEIGHT = 20;
		public static bool pathFollowStatus = false;
		public static bool debugging = false;
		public PathFollowerController pfcPrefab;
		public Grid grid;
		public UI ui;
		private PathFollowerController lastDebuggedPFA;
		private Queue<PathFollowerController> _pathfollower = new Queue<PathFollowerController>();
		private int count=0;

		private PathFollowerController CreatePFActor(){
			PathFollowerController newPFActor = Instantiate<PathFollowerController> (pfcPrefab);

			newPFActor.GetComponent<NewActor> ().Position = grid.RCtoPosition (Random.Range(1,GRIDHEIGHT-2), Random.Range(1,GRIDLENGTH-2));
			newPFActor.gameObject.name = "Path Following Actor "+count;
			newPFActor.transform.SetParent( transform);
			newPFActor.gameObject.SetActive (true);
			return newPFActor;
		}

		private void TogglePFActors(){
			if (!pathFollowStatus) {
				for (int i = 0; i < SPAWN_NUM; i++) {
					_pathfollower.Enqueue (CreatePFActor ());
					count += 1;
				}
				GameObject.Find ("Path Following Actor 0").GetComponent<PathFollowerController> ().TheOne = true;
			}
			if (pathFollowStatus) {
				for (int i = 0; i < SPAWN_NUM; i++) {
					Destroy( _pathfollower.Dequeue ().gameObject);
					count -= 1;
				}

			}
			pathFollowStatus = !pathFollowStatus;
			debugging = false;
		}

		private void Debug(bool debug){
			PathFollowerController thePFA = GameObject.Find("Path Following Actor 0").GetComponent<PathFollowerController>(); 
			if(debug)
			{
				thePFA.DrawPath = true;
			}
			if (!debug)
				thePFA.DrawPath = false;
		}

		// Update is called once per frame
		void Update () {
			//Toggle Path Followers
			if (Input.GetKeyDown (KeyCode.Space)) {
				TogglePFActors ();
				ui.Reset ();
			}
			//Toggle Debug and Path Visibility
			if (Input.GetKeyDown (KeyCode.Tab)&&pathFollowStatus) {
				debugging = !debugging;
				Debug (debugging);
			}
				
		}
	}
}