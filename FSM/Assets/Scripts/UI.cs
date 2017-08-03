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
	public Text InventoryTest;
	public Text StateTest;
	
	public Image Road;
	public Image Forest;
	public Image Rock;
	public Image Wall;
	public Image GD;
	public Image GK;
	public Image RD;
	public Image RK;
	public Image BK;
	public Image BD;
	public Image TS;
	public Color _orig_color;
	public Color selected_color;
		public TreasureFind treasureFind;
	public Grid grid;
	// Use this for initialization
	void Awake () {
		RoadText.gameObject.SetActive (true);
		RoadText.text = "Road\nPress <b><color=yellow>1</color></b>\nCost:1";
		//RoadText.transform.position = new Vector2 (Screen.width -70f,Screen.height-100f);
		ForestText.text = "Forest\nPress <b><color=yellow>2</color></b>\nCost:2";
		//ForestText.transform.position = new Vector2 (Screen.width -70f,Screen.height-200f);
		RockText.text = "Rock\nPress <b><color=yellow>3</color></b>\nCost:3";
		//RockText.transform.position = new Vector2 (Screen.width -70f,Screen.height-300f);
		WallText.text = "Wall\nPress <b><color=yellow>4</color></b>\nBlocking";
		//WallText.transform.position = new Vector2 (Screen.width -70f,Screen.height-400f);
		InventoryTest.text = "Inventory";
		StateTest.text = "Current State:\n";
		
		
		_orig_color = Road.color;
		selected_color = new Color (1f, 0.9f, 0.1f);
	}
	public void DrawStatusRoad(){
		grid.SetDrawStatus (Grid.DrawStatus.normal);
	}
	public void DrawStatusTree ()	{
		grid.SetDrawStatus (Grid.DrawStatus.forest);
	}
	public void DrawStatusRock (){
		grid.SetDrawStatus (Grid.DrawStatus.rock);
	}	
	public void DrawStatusWall (){
		grid.SetDrawStatus (Grid.DrawStatus.blocked);
	}
	public void DrawStatusGK (){
		grid.SetDrawStatus (Grid.DrawStatus.GK);
	}
	public void DrawStatusGD (){
		grid.SetDrawStatus (Grid.DrawStatus.GD);
	}
	public void DrawStatusRK ()
	{
		grid.SetDrawStatus (Grid.DrawStatus.RK);
	}
	public void DrawStatusRD ()
	{
		grid.SetDrawStatus (Grid.DrawStatus.RD);
	}
	public void DrawStatusBK ()
	{
		grid.SetDrawStatus (Grid.DrawStatus.BK);
	}
	public void DrawStatusBD ()
	{
		grid.SetDrawStatus (Grid.DrawStatus.BD);
	}
	public void DrawStatusTS ()
	{
		grid.SetDrawStatus (Grid.DrawStatus.TS);
	}
	
	void rr(Image r){
				r.color = _orig_color;
			r.transform.localScale= new Vector3 (1.5f, 1.5f, 1.5f);
	}
	
	public void Reset(){
			rr (Road);
			rr (Forest);
			rr (Wall);
			rr (Rock);
			rr (GD);
			rr (GK);
			rr (RD);
			rr (RK);
			rr (BD);
			rr (BK);
			rr (TS);
		grid.ResetGrid ();
	}
	// Update is called once per frame
	void Update () {
		
		if(!TreasureFind.treasureFind)
			ModeText.text = "Press <b><color=yellow>Space</color></b> \nto toggle mode\n<b><color=yellow>Draw Mode</color></b>: <b><color=green>On</color></b>\n"
				+"<b><color=yellow>Treasure Find</color></b>: <b><color=Red>Off</color></b>";
		if(TreasureFind.treasureFind)
			ModeText.text = "Press <b><color=yellow>Space</color></b> \nto toggle mode\n<b><color=yellow>Draw Mode</color></b>: <b><color=Red>Off</color></b>\n"
				+"<b><color=yellow>Treasure Find</color></b>: <b><color=green>On</color></b>";

		if (TreasureFind.debugging)
			DebugText.text = "Press <b><color=yellow>Tab</color></b> \nto toggle Path\n<b><color=yellow>Path Visualization</color></b>: \n<b><color=green>On</color></b>";
		if(!TreasureFind.debugging)
			DebugText.text = "Press <b><color=yellow>Tab</color></b> \nto toggle Path\n<b><color=yellow>Path Visualization</color></b>: \n<b><color=red>Off</color></b>";

		StateTest.text = TreasureFind.treasureFind? "Current State:\n"+"<b><size=180><color=yellow>"+treasureFind.tfc.currentState.Name+"</color></size></b>":"Current State:\n";

	}
}
}