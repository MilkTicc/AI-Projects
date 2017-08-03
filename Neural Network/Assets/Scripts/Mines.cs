using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : MonoBehaviour {

	float flashTime = 0;
	bool flashing = false;
	float cameraHeight;
	float cameraWidth;
	SpriteRenderer _spriteRenderer;

	void Awake(){
		cameraHeight = Camera.main.orthographicSize * 0.9f;
		cameraWidth = Camera.main.orthographicSize * Camera.main.aspect * 0.9f;
	}

	public void Flash(){
		flashing = true;
		transform.position = new Vector3 (Random.Range (-cameraWidth, cameraWidth), Random.Range (-cameraHeight, cameraHeight), 0f);
		
	}
	// Use this for initialization

	void Start () {
		_spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(flashing){
			flashTime += Time.deltaTime;
			_spriteRenderer.color = new Color (1, 1, 0);
			if(flashTime > 0.6f)
			{
				_spriteRenderer.color = new Color (1, 1, 1);
				flashTime = 0;
				flashing = false;
			}
		}
	}
}
