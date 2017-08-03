using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {
	Situation situation;
	public DynamicLight lineOfSight;
	GameObject player;
	
	// Use this for initialization
	void Start () {
		lineOfSight = GetComponentInChildren<DynamicLight> ();
		situation = GameObject.FindGameObjectWithTag ("Situation").GetComponent<Situation> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		//lineOfSight.enabled = false;
		//lineOfSight.LightRadius = 0;
		
		if (lineOfSight)
			lineOfSight.InsideFieldOfViewEvent += playerSpotted;
	}

	void playerSpotted (GameObject [] p)
	{
		foreach (GameObject pl in p) {
			if (pl.GetInstanceID () == player.GetInstanceID ()) {
				//Debug.Log ("Player Spotted!");
				if (!situation.isPlayerSpotted)
					situation.isPlayerSpotted = true;
				situation.MaxSecurity ();
				
				//isPlayerSpotted = true;
			}
		}
	}

	public void Disable(){
		Debug.Log ("Disable");
		lineOfSight.enabled = false;
		//lineOfSight.InsideFieldOfViewEvent -= playerSpotted;
		lineOfSight.LightRadius = 0;
		lineOfSight.RangeAngle = 0;
		lineOfSight.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
