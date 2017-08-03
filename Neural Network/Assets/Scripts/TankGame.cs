using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AISandbox
{
	public class TankGame : MonoBehaviour
	{
		public TankActor tankPrefab;
		public Mines minesPrefab;
		public int mineCount = 50;
		public int tankCount = 20;
		float cameraHeight;
		float cameraWidth;
		int movementAmout = 140;
		public int round = 0;
		public int roundTime = 5;
		int eliteAmout = 2;
		public int highScore;
		public int totalScore;
		float mutationRate = 0.05f;
		float crossOverRate = 0.7f;
		public float MutationRate { get { return mutationRate; } set { mutationRate = value;}}
		public float CrossOverRate { get { return crossOverRate;} set { crossOverRate = value;}}
		public float currentRoundTime = 0f;
		public int currentRoundHS = 0;
		float [] selectionList;
		public Transform tanksFolder;
		public Transform minesFolder;
		List<Mines> allMines = new List<Mines> ();
		List<MovementList> moveLists = new List<MovementList> ();
		//List<float []> weightList = new List<float []> ();
		public List<TankActor> allTanks = new List<TankActor> ();
		public List<int> highScoreList = new List<int>();
		public List<float> averageScoreList = new List<float>();

		Mines CreateMines(int index){
			Mines newMine = Instantiate<Mines> (minesPrefab);
			newMine.gameObject.name = "Mine " + index;
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
			newTank.MoveList = moveLists[newTank.SerialNum];
			//newTank.Mines = allMines;
			//newTank.Weights = weightList [newTank.SerialNum];
			newTank.SetMines (allMines);
			allTanks.Add (newTank);
			return newTank;
		}

		void ResetAllTanks(){
			foreach(TankActor tank in allTanks){
				tank.ResetTank (moveLists[tank.SerialNum], allMines);
				//tank.Mines = mines;
			}
		}

		void NewMoveLists(){
			for (int i = 0; i < tankCount; i++)
			{
				moveLists.Add (new MovementList (movementAmout));
			}
		}

		//void NewWeightLists(){
		//	for (int i = 0; i < tankCount;i++)
		//	{
		//		float [] weights = new float [56];
		//		for (int j = 0; j < weights.Length; j++) {
		//			weights [j] = Random.Range (-1f, 1f);
		//		}
		//		weightList.Add (weights);
		//	}
		//}

		void ResetAllMines(){
			foreach (Mines mine in allMines){
				Destroy (mine.gameObject);
			}
			allMines.Clear ();
			CreateAllMines ();
		}

		void Generic(){
			//for (int i = 0; i < moveLists.Count;i++){
			//	totalScore = 0;
			//	totalScore += moveLists [i].Score;
			//}
			//CalcScore ();
			highScoreList.Add (currentRoundHS);
			averageScoreList.Add ((float)totalScore / tankCount);
			//Debug.Log ("HS: "+highScoreList[0] +" "+highScoreList[highScoreList.Count-1] + "  total: " + totalScore);

			List<MovementList> sortedList = moveLists.OrderByDescending (list => list.Score).ToList();
			//for (int i = 0; i < sortedList.Count; i++) { Debug.Log (sortedList [i].Score); }
			//highScore = sortedList [0].Score;
			List<MovementList> nextGenList = new List<MovementList> ();
			for (int i = 0; i < eliteAmout; i++)
				{
				//sortedList [i].IsElite = true; 
				nextGenList.Add( new MovementList (movementAmout));
				nextGenList[i].EqualTo (sortedList [i]);
				nextGenList [i].IsElite = true;
			}
			for (int i = eliteAmout; i < tankCount; i+=2){
				//sortedList [i].EqualTo (OffSpring (Selection (moveLists), Selection (moveLists)).Key);
				//sortedList [i + 1].EqualTo (OffSpring (Selection (moveLists), Selection (moveLists)).Value);
				//Mutation (sortedList [i]);
				//Mutation (sortedList [i+1]);
				//Debug.Log ("before offspring: " + i);
				nextGenList.AddRange (OffSpring (Selection (sortedList), Selection (sortedList)));
			}
			foreach(MovementList list in nextGenList)
			{
				list.Score = 0;
				if (!list.IsElite)
					Mutation (list);
			}
			moveLists = nextGenList;

		}

		void Mutation(MovementList list){
				float rand = Random.Range (0f, 1f);
				if (rand < mutationRate) {
				//int mutationPoint = Random.Range (10, 120);
				//list.WeightList[Random.Range(0,list.WeightList.Length-1)] = Random.Range (-1f, 1f);
				for (int i = 0; i < movementAmout; i++){
					list.WeightList [i] = Random.Range (-1f, 1f);
				}
					 list.IsMutant = true;
				}
		}

		MovementList Selection(List<MovementList> list){
			//int sum = 0;
			//int index1 = 0;
			MovementList theChosenOne = new MovementList(movementAmout);
			//List<int> scorelist = new List<int> ();
 		//	foreach(MovementList thelist in list){
			//	//scorelist.Add (th elist.Score);
			//	sum += thelist.Score;
			//}
			float rand1 = Random.Range (1f, totalScore-1f);
			//while(rand1 > 0){
			//	rand1 -= scorelist [index1];
			//	index1++;
			//}
			//return list[index1];
			int scoreSoFar = 0;
			for (int i = 0; i < list.Count; i++){
				scoreSoFar+= list [i].Score;
				if (scoreSoFar > rand1) {
					//Debug.Log (i + "  " + list [i].Score);
					theChosenOne.EqualTo (list [i]);
					break;
				}
			}
			return theChosenOne;
		}

		List<MovementList> OffSpring(MovementList list1, MovementList list2){
			int cutoff = Random.Range (1, movementAmout);
			float rand = Random.Range (0f, 1f);
			MovementList tempList1 = new MovementList (movementAmout);
			MovementList tempList2 = new MovementList (movementAmout);
			tempList1.EqualTo (list1);
			tempList2.EqualTo (list2);
			if (rand > crossOverRate)
				return new List<MovementList> { tempList1, tempList2 };
			
			for (int i = 0; i < cutoff;i++)
			{
				//tempList1.Left () [i] = list1.Left () [i];
				//tempList2.Left () [i] = list2.Left () [i];
				//tempList1.Right () [i] = list1.Right () [i];
				//tempList2.Right() [i] = list2.Right ()[i];
				tempList1.WeightList [i] = list1.WeightList [i];
				tempList2.WeightList [i] = list2.WeightList [i];
			}
			for (int i = cutoff; i < movementAmout; i++) {
				//tempList1.Left () [i] = list2.Left () [i];
				//tempList2.Left () [i] = list1.Left () [i];
				//tempList1.Right () [i] = list2.Right () [i];
				//tempList2.Right () [i] = list1.Right () [i];
				tempList1.WeightList [i] = list2.WeightList [i];
				tempList2.WeightList [i] = list1.WeightList [i];
			}
			return new List<MovementList> { tempList1, tempList2};
		}

		void CalcScore(){
			totalScore = 0;
			currentRoundHS = 0;
			foreach(TankActor tank in allTanks){
				totalScore += tank.MoveList.Score;
				if(tank.MoveList.Score > currentRoundHS){
					currentRoundHS = tank.MoveList.Score;
				}
			}
		}

		void CreateAllMines(){
			for (int i = 0; i < mineCount;i ++){
				CreateMines (i);
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
			//NewWeightLists ();
			CreateAllTanks ();

		}


		// Update is called once per frame
		void FixedUpdate ()
		{
			currentRoundTime += Time.fixedDeltaTime;
			CalcScore ();
			if (currentRoundTime > roundTime)
			{
				Generic ();
				ResetAllMines ();
				ResetAllTanks ();

				round++;
				currentRoundTime = 0f;
			}

			if (Input.GetKeyDown (KeyCode.Equals))
				crossOverRate += 0.05f;
			if (Input.GetKeyDown (KeyCode.Minus))
				crossOverRate -= 0.05f;
			if (Input.GetKeyDown (KeyCode.RightBracket))
				mutationRate += 0.01f;
			if (Input.GetKeyDown (KeyCode.LeftBracket))
				mutationRate -= 0.01f;
			
			
		}
	}
}