using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AISandbox{
public class UI : MonoBehaviour {
	public Text RoadText, ForestText, RockText, WallText, ModeText, DebugText;
	public Text MessageBoard;
		public Text messagePrefab;
	public Queue<Text> Messages;
	int currentMessageIndex = 0;
	public GameObject drawMode;
	public Image Road, Forest, Rock, Wall;
	public Color _orig_color;
	public Color selected_color;
	public Grid grid;
	public CapturetheFlag ctf;
	// Use this for initialization
	void Awake () {
		RoadText.gameObject.SetActive (true);
		RoadText.text = "Road\nPress <b><color=yellow>1</color></b>\nCost:1";
		ForestText.text = "Forest\nPress <b><color=yellow>2</color></b>\nCost:2";
		RockText.text = "Rock\nPress <b><color=yellow>3</color></b>\nCost:3";
		WallText.text = "Wall\nPress <b><color=yellow>4</color></b>\nBlocking";
		//StateTest.text = "Current State:\n";
		
		_orig_color = Road.color;
		selected_color = new Color (1f, 0.9f, 0.1f);
			//for (int i = 0; i < Messages.Length;i++)
			//{
			//	Messages [i].text = "";
			//}
			Messages = new Queue<Text>();
	}
	//public void DrawStatusRoad(){
	//	grid.SetDrawStatus (Grid.DrawStatus.normal);
	////}
	//public void DrawStatusTree ()	{
	//	grid.SetDrawStatus (Grid.DrawStatus.forest);
	//}
	//public void DrawStatusRock (){
	//	grid.SetDrawStatus (Grid.DrawStatus.rock);
	//}	
	//public void DrawStatusWall (){
	//	grid.SetDrawStatus (Grid.DrawStatus.blocked);
	//}
	//public void DrawStatusGK (){
	//	grid.SetDrawStatus (Grid.DrawStatus.GK);
	//}
	//public void DrawStatusGD (){
	//	grid.SetDrawStatus (Grid.DrawStatus.GD);
	//}
	//public void DrawStatusRK ()
	//{
	//	grid.SetDrawStatus (Grid.DrawStatus.RK);
	//}
	//public void DrawStatusRD ()
	//{
	//	grid.SetDrawStatus (Grid.DrawStatus.RD);
	//}
	//public void DrawStatusBK ()
	//{
	//	grid.SetDrawStatus (Grid.DrawStatus.BK);
	//}
	//public void DrawStatusBD ()
	//{
	//	grid.SetDrawStatus (Grid.DrawStatus.BD);
	//}
	//public void DrawStatusTS ()
	//{
	//	grid.SetDrawStatus (Grid.DrawStatus.TS);
	//}
	
	void rr(Image r){
		r.color = _orig_color;
		r.transform.localScale= new Vector3 (1.5f, 1.5f, 1.5f);
	}
	
	

	 Text CreateMessage(string message, Color color){
		Text newText = Instantiate<Text> (messagePrefab);
		newText.text = message;
		newText.color = color;
		newText.name = "Message" + (currentMessageIndex + 1);
		newText.transform.SetParent (MessageBoard.transform);
		//newText.transform.position = MessageBoard.transform.position + new Vector3(0,-2-1.2f*(currentMessageIndex) ,0);
		//newText.rectTransform.position = new Vector3(0,0,0);
		newText.gameObject.SetActive (true);
		//if (currentMessageIndex < 14)
			currentMessageIndex++;
		return newText;
	}

	public void AddMessage(string message, Color color){
				Messages.Enqueue (CreateMessage (message, color));
			if (currentMessageIndex >= 17)
				Destroy( Messages.Dequeue ());

			for (int i = 0; i < Messages.ToArray().Length ; i++) {
				Messages.ToArray () [i].transform.position = MessageBoard.transform.position + new Vector3 (-1, -2 - 1.8f * (i), 0);
			}
	}

		public void ClearMessage(){
			currentMessageIndex = 0;
			while (Messages.Count != 0)
				Destroy(Messages.Dequeue ().gameObject);
		}

	public void Reset(){
		rr (Road);
		rr (Forest);
		rr (Wall);
		rr (Rock);
		grid.ResetGrid ();
	}
	// Update is called once per frame
	void Update () {
			MessageBoard.gameObject.SetActive (ctf.gameStart);
			drawMode.gameObject.SetActive (!ctf.gameStart);

		if(!ctf.gameStart)
			ModeText.text = "Press <b><color=yellow>Space</color></b> \nto toggle mode\n<b><color=yellow>Draw Mode</color></b>: <b><color=green>On</color></b>\n"
				+"<b><color=yellow>Capture the Flag</color></b>: <b><color=Red>Off</color></b>";
		if (ctf.gameStart) {
			
			ModeText.text = "Press <b><color=yellow>Space</color></b> \nto toggle mode\n<b><color=yellow>Draw Mode</color></b>: <b><color=Red>Off</color></b>\n"
					+ "<b><color=yellow>Capture the Flag</color></b>: <b><color=green>On</color></b>";
		}

		//if (TreasureFind.debugging)
		//	DebugText.text = "Press <b><color=yellow>Tab</color></b> \nto toggle Path\n<b><color=yellow>Path Visualization</color></b>: \n<b><color=green>On</color></b>";
		//if(!TreasureFind.debugging)
		//	DebugText.text = "Press <b><color=yellow>Tab</color></b> \nto toggle Path\n<b><color=yellow>Path Visualization</color></b>: \n<b><color=red>Off</color></b>";

	}
}
}