using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public class MovementList
	{
		float [] leftMovementList;
		float [] rightMovementList;
		bool isElite;
		bool isMutant;
		int score;
		int length;

		public int Score{ get { return score;}set { score = value;}}
		// Use this for initialization
		public MovementList(int num){
			length = num;
			leftMovementList = new float [num];
			rightMovementList = new float [num];
			for (int i = 0; i < num; i++) {
				rightMovementList [i] = Random.Range (-1f, 1f);
				leftMovementList [i] = Random.Range (-1f, 1f);
				//rightMovementList [i] = -1;
				//leftMovementList [i] = -1;
			}
		}

		public bool IsElite { get { return isElite; } set { isElite = value; } }
		public bool IsMutant{get { return isMutant;} set { isMutant = value;}}

		public float[] Left(){
			return leftMovementList;
		}
		public float [] Right ()
		{
			return rightMovementList;
		}

		public void EqualTo(MovementList list){
			for (int i = 0; i < length; i++){
				leftMovementList [i] = list.Left () [i];
				rightMovementList [i] = list.Right () [i];
			}
		}

		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{

		}
	}
}