using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AISandbox
{
	public class TankGame : MonoBehaviour
	{
		public TankActor tankPrefab;
		public Mines minesPrefab;
		int mineCount = 45;
		int tankCount = 20;
		float cameraHeight;
		float cameraWidth;
		int movementAmout = 121;
		public int round = 0;
		int roundTime = 30;
		int eliteAmout = 2;
		public int highScore;
		float mutationRate = 0.05f;
		float crossOverRate = 0.7f;
		public float currentRoundTime = 0f;
		public int currentRoundHS = 0;
		float [] selectionList;
		public Transform tanksFolder;
		public Transform minesFolder;
		bool resetMineInNextRound = false;
		List<Mines> allMines = new List<Mines> ();
		List<MovementList> moveLists = new List<MovementList> ();
		public List<TankActor> allTanks = new List<TankActor> ();
		//float[][] inputList;

		Mines CreateMines(){
			Mines newMine = Instantiate<Mines> (minesPrefab);
			newMine.gameObject.name = "Mine";
			newMine.transform.position = new Vector3 (Random.Range (-cameraWidth, cameraWidth), Random.Range (-cameraHeight, cameraHeight), 0f);
			newMine.transform.SetParent (minesFolder);
			newMine.gameObject.SetActive (true);
			allMines.Add (newMine);
			return newMine;
		}

		TankActor CreateTank(int i){
			TankActor newTank = Instantiate<TankActor> (tankPrefab);
			newTank.gameObject.name = "Tank "+(i+1);
			newTank.transform.parent = tanksFolder;
			newTank.gameObject.SetActive (true);
			newTank.SerialNum = i;
			newTank.InputList = moveLists[newTank.SerialNum];
			//newTank.Mines = allMines;
			newTank.SetMines (allMines);
			allTanks.Add (newTank);
			return newTank;
		}

		void ResetAllTanks(){
			foreach(TankActor tank in allTanks){
				tank.ResetTank (moveLists[tank.SerialNum],allMines);
				//tank.Mines = mines;
			}
		}

		void NewMoveLists(){
			for (int i = 0; i < tankCount;i++)
			{
				moveLists.Add (new MovementList (movementAmout));
			}
		}

		void Generic(){
			//List<int> scoreList = new List<int> ();
			//foreach(MovementList movelist in moveLists){
			//	scoreList.Add (movelist.Score);
			//}
			//scoreList.Sort ();
			List<MovementList> sortedList = moveLists.OrderByDescending (list => list.Score).ToList();
			highScore = sortedList [0].Score;
			for (int i = 0; i < eliteAmout; i++)
				{sortedList [i].IsElite = true; }

			for (int i = eliteAmout; i < sortedList.Count; i+=2){
				sortedList [i].EqualTo (OffSpring (Selection (moveLists), Selection (moveLists)).Key);
				sortedList [i + 1].EqualTo (OffSpring (Selection (moveLists), Selection (moveLists)).Value);
				Mutation (sortedList [i]);
				Mutation (sortedList [i+1]);
			}

			foreach(MovementList list in sortedList)
			{
				list.Score = 0;
			}
			moveLists = sortedList;
		}

		void Mutation(MovementList list){
			float rand = Random.Range (0f, 1f);
			if(rand<mutationRate){
				int mutationPoint = Random.Range (10, 120);
				list.Left () [mutationPoint] = Random.Range (-1f, 1f);
				list.Right () [mutationPoint] = Random.Range (-1f, 1f);
				list.IsMutant = true;
			}

		}

		MovementList Selection(List<MovementList> list){
			int sum = 0;
			int index1 = 0;
			List<int> scorelist = new List<int> ();
 			foreach(MovementList thelist in list){
				scorelist.Add (thelist.Score);
				sum += thelist.Score;
			}
			float rand1 = Random.Range (0f, sum-1f);
			while(rand1 > 0){
				rand1 -= scorelist [index1];
				index1++;
			}
			return list[index1-1];
		}

		KeyValuePair<MovementList,MovementList> OffSpring(MovementList list1, MovementList list2){
			int cutoff = Random.Range (1, movementAmout);
			float rand = Random.Range (0f, 1f);
			if (rand > crossOverRate)
				
				return new KeyValuePair<MovementList, MovementList>(list1,list2);
			
			MovementList tempList1 = new MovementList (movementAmout);
			MovementList tempList2 = new MovementList (movementAmout);
			for (int i = 0; i < cutoff;i++)
			{
				tempList1.Left () [i] = list1.Left () [i];
				tempList2.Left () [i] = list2.Left () [i];
				tempList1.Right () [i] = list1.Right () [i];
				tempList2.Right() [i] = list2.Right ()[i];
			}
			for (int i = cutoff; i < movementAmout; i++) {
				tempList1.Left () [i] = list2.Left () [i];
				tempList2.Left () [i] = list1.Left () [i];
				tempList1.Right () [i] = list2.Right () [i];
				tempList2.Right () [i] = list1.Right () [i];
			}
			return new KeyValuePair<MovementList, MovementList> (tempList1, tempList2);
		}


		void CreateAllMines(){
			for (int i = 0; i < mineCount;i ++){
				CreateMines ();
			}
		}


		void CreateAllTanks(){
			for (int i = 0; i < tankCount; i++){
				CreateTank (i);
			}
		}
		void Awake(){
			cameraHeight = Camera.main.orthographicSize * 0.9f;
			cameraWidth = Camera.main.orthographicSize * Camera.main.aspect * 0.9f;
		}
		void Start ()
		{
			//selectionList = new float [12] {0f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.77f, 0.84f, 0.9f, 0.95f ,1f};
			CreateAllMines ();
			NewMoveLists ();
			CreateAllTanks ();
		}


		// Update is called once per frame
		void FixedUpdate ()
		{

			currentRoundTime += Time.fixedDeltaTime;
			if (currentRoundTime > roundTime)
			{
				Generic ();
				ResetAllTanks ();
				currentRoundHS = 0;
				round++;
				currentRoundTime = 0f;
			}
		}
	}
}