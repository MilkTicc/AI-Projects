using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AISandbox{
public class UI : MonoBehaviour {
	public Text RoadText;
	public Text ForestText;
	public Text RockText;
	public Text WallText;
	public Text ModeText;
	public Text DebugText;
	public SpriteRenderer Road;
	public SpriteRenderer Forest;
	public SpriteRenderer Rock;
	public SpriteRenderer Wall;
	public Color _orig_color;
	public Color selected_color;
		public Grid grid;
	// Use this for initialization
	void Awake () {
		RoadText.gameObject.SetActive (true);
		RoadText.text = "Road\nPress <b><color=yellow>1</color></b>\nCost:1";
		RoadText.transform.position = new Vector2 (Screen.width -70f,Screen.height-100f);
		ForestText.text = "Forest\nPress <b><color=yellow>2</color></b>\nCost:2";
		ForestText.transform.position = new Vector2 (Screen.width -70f,Screen.height-200f);
		RockText.text = "Rock\nPress <b><color=yellow>3</color></b>\nCost:3";
		RockText.transform.position = new Vector2 (Screen.width -70f,Screen.height-300f);
		WallText.text = "Wall\nPress <b><color=yellow>4</color></b>\nBlocking";
		WallText.transform.position = new Vector2 (Screen.width -70f,Screen.height-400f);

		ModeText.transform.position = new Vector2 (Screen.width -70f,Screen.height-480f);
		DebugText.transform.position = new Vector2 (Screen.width -70f,Screen.height-550f);

		_orig_color = Road.color;
		selected_color = new Color (1f, 0.9f, 0.1f);
	}
	public void Reset(){
		Road.color = _orig_color;
		Forest.color = _orig_color;
		Wall.color = _orig_color;
		Rock.color = _orig_color;
		Road.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		Forest.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		Wall.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		Rock.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		grid.ResetGrid (true);
	}
	// Update is called once per frame
	void Update () {
		
		if(!PathFollowing.pathFollowStatus)
				ModeText.text = "Press <b><color=yellow>Space</color></b> \nto toggle mode\n<b><color=yellow>Draw Mode</color></b>: <b><color=green>On</color></b>\n"
					+"<b><color=yellow>Path Follow</color></b>: <b><color=Red>Off</color></b>";
		if(PathFollowing.pathFollowStatus)
			ModeText.text = "Press <b><color=yellow>Space</color></b> \nto toggle mode\n<b><color=yellow>Draw Mode</color></b>: <b><color=Red>Off</color></b>\n"
					+"<b><color=yellow>Path Follow</color></b>: <b><color=green>On</color></b>";

		if (PathFollowing.debugging)
				DebugText.text = "Press <b><color=yellow>Tab</color></b> \nto toggle Path\n<b><color=yellow>Path Visualization</color></b>: \n<b><color=green>On</color></b>";
		if(!PathFollowing.debugging)
				DebugText.text = "Press <b><color=yellow>Tab</color></b> \nto toggle Path\n<b><color=yellow>Path Visualization</color></b>: \n<b><color=red>Off</color></b>";
	}
}
}