using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

namespace AISandbox{
	public class TreasureFind : MonoBehaviour {
		public Grid grid;
		public UI ui;
		public TreasureFinderController tfcPrefab;
		public static bool treasureFind=false;
		public static bool debugging = false;
		public TreasureFinderController tfc;
		public SpriteRenderer _redkey;
		public SpriteRenderer _bluekey;
		public SpriteRenderer _greenkey;
		public SpriteRenderer _reddoor;
		public SpriteRenderer _bluedoor;
		public SpriteRenderer _greendoor;
		public SpriteRenderer _treasure;
		public Sprite _opendoor;
		public Sprite _closeddoor;
		
		public GridNode redDoorNode, greenDoorNode, blueDoorNode;
		public static List<GridNode> ShutDoor = new List<GridNode>();
		[HideInInspector]
		public bool greenDoorOpened=false, blueDoorOpened=false, redDoorOpened=false;
//		private GridNode RedKey;
//		private GridNode GreenKey;
//		private GridNode BlueKey;


		private TreasureFinderController CreateTFActor ()
		{
			TreasureFinderController newTFActor = Instantiate<TreasureFinderController> (tfcPrefab);

			newTFActor.GetComponent<NewActor> ().Position = grid.RCtoPosition (grid.tFCSpawnPos);
			newTFActor.gameObject.name = "Treasure Finder ";
			newTFActor.transform.SetParent (transform);
			newTFActor.gameObject.SetActive (true);
			newTFActor.haveRedKey = false;
			newTFActor.haveGreenKey = false;
			newTFActor.haveBlueKey = false;
			newTFActor.haveTreasure = false;
			return newTFActor;
		}

		void ToggleTreasureFind(){
			if (!treasureFind)
				tfc = CreateTFActor ();

			if (treasureFind) {
				Destroy (tfc.gameObject);
				DrawObjects ();
				CloseDoors ();
			}
			treasureFind = !treasureFind;
			debugging = false;
		}

		void CloseDoors(){
			greenDoorOpened = false;
			_greendoor.sprite = _closeddoor;
			blueDoorOpened = false;
			_bluedoor.sprite = _closeddoor;
			redDoorOpened = false;
			_reddoor.sprite = _closeddoor;
			redDoorNode = grid.GetNode ((int)grid.RedDoorPos.x, (int)grid.RedDoorPos.y);
			blueDoorNode = grid.GetNode ((int)grid.BlueDoorPos.x, (int)grid.BlueDoorPos.y);
			greenDoorNode = grid.GetNode ((int)grid.GreenDoorPos.x, (int)grid.GreenDoorPos.y);
			ShutDoor.Add (redDoorNode);
			ShutDoor.Add (greenDoorNode);
			ShutDoor.Add (blueDoorNode);

	}
		
		private void DrawObjects(){
			_redkey.transform.position = grid.RCtoPosition (grid.RedKeyPos);
			_redkey.gameObject.SetActive (true);
			_bluekey.transform.position = grid.RCtoPosition (grid.BlueKeyPos);
			_bluekey.gameObject.SetActive (true);
			_greenkey.transform.position = grid.RCtoPosition (grid.GreenKeyPos);
			_greenkey.gameObject.SetActive (true);
			_reddoor.transform.position = grid.RCtoPosition (grid.RedDoorPos);
			_reddoor.gameObject.SetActive (true);
			_bluedoor.transform.position = grid.RCtoPosition (grid.BlueDoorPos);
			_bluedoor.gameObject.SetActive (true);
			_greendoor.transform.position = grid.RCtoPosition (grid.GreenDoorPos);
			_greendoor.gameObject.SetActive (true);
			_treasure.transform.position = grid.RCtoPosition (grid.TreasurePos);
			_treasure.gameObject.SetActive (true);	
		}
		public void DrawGK(Vector2 pos){
			grid.GreenKeyPos = pos;
			_greenkey.transform.position = grid.RCtoPosition (grid.GreenKeyPos);
		}
		public void DrawGD (Vector2 pos)
		{
			ShutDoor.Remove (greenDoorNode);
			greenDoorNode = grid.GetNode ((int)grid.GreenDoorPos.x, (int)grid.GreenDoorPos.y);
			ShutDoor.Add (greenDoorNode);
			grid.GreenDoorPos = pos;
			_greendoor.transform.position = grid.RCtoPosition (grid.GreenDoorPos);
		}
		public void DrawRK (Vector2 pos)
		{
			grid.RedKeyPos = pos;
			_redkey.transform.position = grid.RCtoPosition (grid.RedKeyPos);
		}
		public void DrawRD (Vector2 pos)
		{
			ShutDoor.Remove (redDoorNode);
			redDoorNode = grid.GetNode ((int)grid.RedDoorPos.x, (int)grid.RedDoorPos.y);
			ShutDoor.Add (redDoorNode);
			grid.RedDoorPos = pos;
			_reddoor.transform.position = grid.RCtoPosition (grid.RedDoorPos);
		}
		public void DrawBK (Vector2 pos)
		{
			grid.BlueKeyPos = pos;
			_bluekey.transform.position = grid.RCtoPosition (grid.BlueKeyPos);
		}
		public void DrawBD (Vector2 pos)
		{
			ShutDoor.Remove (blueDoorNode);
			blueDoorNode = grid.GetNode ((int)grid.BlueDoorPos.x, (int)grid.BlueDoorPos.y);
			ShutDoor.Add (blueDoorNode);
			grid.BlueDoorPos = pos;
			_bluedoor.transform.position = grid.RCtoPosition (grid.BlueDoorPos);
		}
		public void DrawTS (Vector2 pos)
		{
			grid.TreasurePos = pos;
			_treasure.transform.position = grid.RCtoPosition (grid.TreasurePos);
		}
		// Use this for initialization
		void Start () {
			grid.ResetGrid ();
			DrawObjects ();
			CloseDoors ();
			
				
 		}
		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown (KeyCode.Space)) {
				ToggleTreasureFind ();
				ui.Reset ();
			}
			if(Input.GetKeyDown(KeyCode.Tab)&&treasureFind){
					debugging = !debugging;

			}

		}
	}
}