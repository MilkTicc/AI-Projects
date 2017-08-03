using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : MonoBehaviour {

	float flashTime = 0;
	bool flashing = false;
	SpriteRenderer _spriteRenderer;

	public void Flash(){
		//Debug.Log ("flashing");
		flashing = true;
	}
	// Use this for initialization

	void Start () {
		_spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(flashing){
			flashTime += Time.deltaTime;
			_spriteRenderer.color = new Color (1, 0, 0);
			if(flashTime > 0.6f)
			{
				_spriteRenderer.color = new Color (1, 1, 1);
				flashTime = 0;
				flashing = false;
			}
		}
	}
}
