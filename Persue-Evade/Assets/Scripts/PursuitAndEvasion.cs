using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace AISandbox {
    public class PursuitAndEvasion : MonoBehaviour {
        private const float SPAWN_RANGE           = 10.0f;


        public SimpleActor _target_actor;
        public SimpleActor _pursuing_actor;
        public SimpleActor _evading_actor;

		public Vector3 targInitPos;
		public Vector3 purInitPos;

        private void Awake() {
            // Choose a random position for the target actor
            Vector3 position = new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0.0f );
            _target_actor.transform.position = position;
			targInitPos = position;
			//Debug.Log (targInitPos);

            // The pursuing and evading actor start at the same position
            position = new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0.0f );
            _pursuing_actor.transform.position = position;
            _evading_actor.transform.position = position;
			purInitPos = position;

            // Reset the level every 15 seconds
            Invoke( "ResetLevel", 5.0f );
        }
        
        private void ResetLevel() {
            SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
        }
    }
}