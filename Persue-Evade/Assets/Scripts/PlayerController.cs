using UnityEngine;
using System.Collections;

namespace AISandbox {
    [RequireComponent(typeof (IActor))]
    public class PlayerController : MonoBehaviour {
        private IActor _actor;

		private void Awake() {
            _actor = GetComponent<IActor>();
        }

        private void FixedUpdate() {
            // Read the inputs.
            float x_axis = Input.GetAxis("Horizontal");
            float y_axis = Input.GetAxis("Vertical");

            // Pass all parameters to the character control script.
            _actor.SetInput( x_axis, y_axis );
        }
    }
}