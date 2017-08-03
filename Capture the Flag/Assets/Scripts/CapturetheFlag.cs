using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AISandbox
{
	public class CapturetheFlag : MonoBehaviour
	{

		public Grid grid;
		List<GridNode> NeuZone = new List<GridNode> ();
		List<GridNode> CyanZone = new List<GridNode> ();
		List<GridNode> MangettaZone = new List<GridNode>();
		int FOREST_NUM = 30;
		int ROCK_NUM = 15;
		int WALL_NUM = 10;
		public UI ui;
		public GridNode cyanJail;
		GridNode cyanBase;
		public GridNode mangettaJail;
		GridNode mangettaBase;
		public GridNode cyanReturnPoint;
		public GridNode mangettaReturnPoint;
		public GridNode cyanConvoyPoint;
		public GridNode mangettaConvoyPoint;
		public Flag flagPrefab;
		public Flag cyanFlag;
		public Flag mangettaFlag;
		Color cyan = new Color (0, 1, 1);
		Color mangetta = new Color (1, 0,1);
		public OrientedActor actorPrefab;
		public bool gameStart = false;
		public List<GridNode> CGuardZone = new List<GridNode> ();
		public List<GridNode> MGuardZone = new List<GridNode> ();
		Queue<OrientedActor> Players = new Queue<OrientedActor> ();
		int cyanCount = 0;
		int mangCount = 0;
		int playerCount = 0;

		OrientedActor CreatePlayer(bool iscyan, bool isdefender){
			OrientedActor newPlayer = Instantiate<OrientedActor> (actorPrefab);
			newPlayer.CtF = this;
			if (iscyan)
			{
				cyanCount++;
				newPlayer.teamNum = cyanCount;
				newPlayer.teamName = "Cyan Player ";
				newPlayer.position = CyanZone [Random.Range (0, CyanZone.Count - 1)].transform.position;
				newPlayer.currentNode = grid.PostoNode (newPlayer.position);
				newPlayer._Team = OrientedActor.Team.cyan;
				
				if (isdefender)
				{
					newPlayer.SetJob (1);
					newPlayer.gameObject.name = "Cyan Defender";
				}
				else
				{
					newPlayer.gameObject.name = "Cyan Attacker";
					newPlayer.SetJob(2);
				}
			}
			else 
			{
				mangCount++;
				newPlayer.teamNum = mangCount;
				newPlayer.teamName = "Mangetta Player ";
				newPlayer.position = MangettaZone [Random.Range (0, CyanZone.Count - 1)].transform.position;
				newPlayer.currentNode = grid.PostoNode (newPlayer.position);
				newPlayer._Team  =OrientedActor.Team.mangetta;
				
			if (isdefender)
				{
					newPlayer.gameObject.name = "Mangetta Defender";
					newPlayer.SetJob (1);
				}
			else
				{
					newPlayer.gameObject.name = "Mangetta Attacker";
					newPlayer.SetJob (2);
				}
			}

			newPlayer.transform.SetParent (transform);
			newPlayer.gameObject.SetActive (true);
			playerCount++;
			return newPlayer;
		}
		Flag CreateFlags(bool iscyan){
			Flag newflag = Instantiate<Flag> (flagPrefab);
			if(iscyan)
			{
				newflag.transform.position = cyanBase.transform.position;
				newflag.GetComponent<SpriteRenderer> ().color = cyan;
				newflag.gameObject.name = "CyanFlag";
			}
			else{
				newflag.transform.position = mangettaBase.transform.position;
				newflag.GetComponent<SpriteRenderer> ().color = mangetta;
				newflag.gameObject.name = "MangettaFlag";
			}
			newflag.transform.SetParent (transform);
			newflag.gameObject.SetActive (true);
			return newflag;
		}

		void ToggleGame(){
			if (!gameStart){
				gameStart = true;
				cyanFlag = CreateFlags (true);
				mangettaFlag = CreateFlags (false);
				Players.Enqueue (CreatePlayer (true,true));
				Players.Enqueue (CreatePlayer (true, true));
				//Players.Enqueue (CreatePlayer (true, false));
				Players.Enqueue (CreatePlayer (true, false));
				Players.Enqueue (CreatePlayer (false, true));
				Players.Enqueue (CreatePlayer (false, true));
				Players.Enqueue (CreatePlayer (false, false));
				//Players.Enqueue (CreatePlayer (false, false));

				foreach (OrientedActor players in Players){
					foreach(OrientedActor otherplayers in Players)
					{
						if (otherplayers._Team != players._Team)
							players.enemies.Add (otherplayers);
						else
							players.teammates.Add (otherplayers);
					}
					players.teammates.Remove (players);
				}
			}
			else{
				//Destroy (FindObjectsOfType<Flag> () [0]);
				//Destroy (FindObjectsOfType<Flag>()[1]);
				//Destroy (mangettaFlag.gameObject);
				cyanFlag.gameObject.SetActive (false);
				mangettaFlag.gameObject.SetActive (false);
				for (int i = 0; i < playerCount; i++) {
					Destroy (Players.Dequeue ().gameObject);}
				playerCount = 0;
				cyanCount = 0;
				mangCount = 0;
				DrawNeutralZone ();
				DrawCyanZone ();
				DrawMangettaZone ();
				ui.ClearMessage ();
				gameStart = false;
			}
		}

		void DrawNeutralZone(){
			for (int i = 0; i < grid.GRIDHEIGHT; i++)
				NeuZone.Add (grid.GetNode (i,  grid.GRIDLENGTH / 2));

			foreach (GridNode nodes in NeuZone) {
				nodes.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 0.6f);
			}
		}
		void DrawCyanZone(){
			CyanZone.Clear ();
			for (int i = 0; i < grid.GRIDHEIGHT; i++){
				for (int j = 0; j < grid.GRIDLENGTH / 2; j++){
					CyanZone.Add (grid.GetNode (i, j));
					grid.GetNode (i, j).GetComponent<SpriteRenderer> ().color = new Color (0.85f, 1, 1);
				}
			}
		}
		void DrawMangettaZone ()
		{
			MangettaZone.Clear ();
			for (int i = 0; i < grid.GRIDHEIGHT; i++) {
				for (int j = grid.GRIDLENGTH / 2+1; j < grid.GRIDLENGTH; j++) {
					MangettaZone.Add (grid.GetNode (i, j));
					grid.GetNode (i, j).GetComponent<SpriteRenderer> ().color = new Color (1, 0.85f, 1);
				}
			}
		}
		void DrawForestOnZone(List<GridNode> Nodes){
			int x = Random.Range (0, Nodes.Count - 1);
			Nodes [x].SetNodeStatus (GridNode.NodeStatus.forest);
			Nodes.RemoveAt (x);
		}
		void DrawRockOnZone (List<GridNode> Nodes)
		{
			int x = Random.Range (0, Nodes.Count - 1);
			Nodes [x].SetNodeStatus (GridNode.NodeStatus.rock);
			Nodes.RemoveAt (x);
		}
		void DrawWallOnZone (List<GridNode> Nodes)
		{
			int x = Random.Range (0, Nodes.Count - 1);
			Nodes [x].SetNodeStatus (GridNode.NodeStatus.blocked);
			Nodes.RemoveAt (x);
		}

		void DrawJail(){
			CyanZone [33].SetNodeStatus (GridNode.NodeStatus.jail);
			cyanJail = CyanZone [33];
			mangettaConvoyPoint = grid.GetNode (cyanJail.row, 15);
			CyanZone.RemoveAt (33);
			MangettaZone [266].SetNodeStatus (GridNode.NodeStatus.jail);
			mangettaJail = MangettaZone [266];
			cyanConvoyPoint = grid.GetNode (mangettaJail.row, 15);
			MangettaZone.RemoveAt (266);
		}

		void DrawBase(){
			MangettaZone [41].SetNodeStatus (GridNode.NodeStatus.flagbase);
			mangettaBase = MangettaZone [41];
			cyanReturnPoint = grid.GetNode (mangettaBase.row, 15);
			MangettaZone.RemoveAt (41);
			CyanZone [257].SetNodeStatus (GridNode.NodeStatus.flagbase);
			cyanBase = CyanZone [257];
			mangettaReturnPoint = grid.GetNode (cyanBase.row, 15);
			
			CyanZone.RemoveAt (257);
		}

		void SetGuardZone(){
			for (int row = 0; row < grid.GRIDHEIGHT; ++row) {
				for (int col = 0; col < grid.GRIDLENGTH; ++col) {

					if (grid.heuristic (grid.GetNode (row, col), cyanBase) <= 6 && grid.GetNode (row, col)._NodeStatus != GridNode.NodeStatus.blocked)
						CGuardZone.Add (grid.GetNode (row,col));
					if (grid.heuristic (grid.GetNode (row, col), mangettaBase) <= 6&& grid.GetNode (row, col)._NodeStatus != GridNode.NodeStatus.blocked)
						MGuardZone.Add (grid.GetNode (row, col));
				}
			}
		}

		void DrawMap(){
			for (int i = 0; i < FOREST_NUM; i++) {
				DrawForestOnZone (CyanZone);
				DrawForestOnZone (MangettaZone);}
			for (int i = 0; i < ROCK_NUM; i++) {
				DrawRockOnZone (CyanZone);
				DrawRockOnZone (MangettaZone);
			}
			for (int i = 0; i < WALL_NUM; i++) {
				DrawWallOnZone (CyanZone);
				DrawWallOnZone (MangettaZone);
			}
		}
		void Start ()
		{
			DrawNeutralZone ();
			DrawCyanZone ();
			DrawMangettaZone ();
			DrawJail ();
			DrawBase ();
			SetGuardZone ();
			DrawMap ();
		}

		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown (KeyCode.Space))
				ToggleGame ();
			//if (Input.GetKeyDown (KeyCode.Tab))
			//{
			//	Debug.Log (Players.Peek().enemies.Count);
			//}
		}
	}
}