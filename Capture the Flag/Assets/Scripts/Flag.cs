using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AISandbox
{
	public class Flag : MonoBehaviour
	{

		Vector2 position;
		OrientedActor capturer;
		public bool captured;
		// Use this for initialization
		void Start ()
		{

		}

		public void FlagCapture(OrientedActor actor){
			captured = true;
			capturer = actor;
		}

		public void FlagSave(){
			captured = false;
			capturer = null;
		}



		// Update is called once per frame
		void Update ()
		{
			if (captured && capturer != null)
				transform.position = capturer.transform.position + new Vector3 (0.3f, 0.7f,0);
		}
	}
}