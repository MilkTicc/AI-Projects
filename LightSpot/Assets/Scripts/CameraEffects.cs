using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraEffects : MonoBehaviour {

	public Material mat;
	public float ChromaticAbberation = 1.0f;
	public float effectStrength = 1f;
	public float effectRange = 0.5f;
	public float resoX;
	public float resoY;
	public PlayerController player;
	public Vector4 playerPos;

	void Awake(){
	resoX = Camera.main.pixelWidth;
	 resoY = Camera.main.pixelHeight;
		//playerPos =  (player.transform.position);
		playerPos = Camera.main.WorldToViewportPoint(player.transform.position);
		//Debug.Log (playerPos);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest){
		mat.SetFloat ("_AberrationOffset", ChromaticAbberation);
		mat.SetFloat ("_EffectStrength", effectStrength);
		mat.SetFloat ("_EffectRange", effectRange);
		mat.SetFloat ("_ResoX", resoX);
		mat.SetFloat ("_ResoY", resoY);
		mat.SetVector ("_PlayerPos", playerPos);
		Graphics.Blit (src, dest, mat);
		
	}

	void Update(){
		playerPos = Camera.main.WorldToViewportPoint (player.transform.position);
		
	}
}
