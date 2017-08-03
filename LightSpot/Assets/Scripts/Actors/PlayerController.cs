using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


    [RequireComponent(typeof (IActor))]
    public class PlayerController : MonoBehaviour {
        private IActor _actor;
		public int intelPicked =0;
	public bool isCaught = false;
		IInteractables curInteractable = null;
	Text notifyText;

		private void Awake() {
            _actor = GetComponent<IActor>();
		notifyText = GameObject.Find ("Notify").GetComponent<Text> ();
        }

	void OnTriggerEnter2D (Collider2D col)
	{
		//Debug.Log ("asd");
		if (col.tag == "Intel") {
			curInteractable = col.gameObject.GetComponent<Intel> ();
			curInteractable.Notify ();
		}
		if(col.tag == "Switch"){
			curInteractable = col.gameObject.GetComponent<CamSwitch> ();
			//Debug.Log (curInteractable);
			curInteractable.Notify ();
			
		}
		if(col.tag == "Exit"){
			curInteractable = col.gameObject.GetComponent<Exit> ();
			curInteractable.Notify ();
		}
		//curInteractable = col.gameObject.GetComponent<IInteractables> ();
		//if(curInteractable!=null)
		//curInteractable.Notify ();
	}

	void OnTriggerExit2D(Collider2D col){
		if(curInteractable == null){
			return;
		}
		if(col.gameObject.GetInstanceID() == curInteractable.ThisGameObject().GetInstanceID())
		{curInteractable = null;
			notifyText.gameObject.SetActive (false);
			//Debug.Log (curInteractable);
		}
		
	}


        private void Update() {
		//foreach(Intel intel in intels){
		//	if( (transform.position - intel.transform.position).magnitude < 1f){
		//		intelPicked += 1;
		//		intel.PickUp ();
		//		Debug.Log (intelPicked);
		//	}
		//}

			
		if(Input.GetKeyDown(KeyCode.E) && curInteractable != null){
			curInteractable.Interact (this);
			//notifyText.gameObject.SetActive (false);
			
			//intelPicked += 1;
			//Debug.Log (intelPicked);
		}
            float y_axis = Input.GetAxis("Horizontal");
            float y2_axis = Input.GetAxis("Vertical");

			_actor.SetVelo ( y_axis,  y2_axis);
        }
    }
