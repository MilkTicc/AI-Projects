using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AISandbox
{
	public class Graph : MonoBehaviour
	{

		public TankGame game;
		public LineRenderer xAxis;
		public LineRenderer yAxis;
		public LineRenderer averageLine;
		public LineRenderer highLine;
		Vector2 graphOrigin = new Vector2 (-10, -13);
		Vector2 xLength = new Vector2 (35, 0);
		Vector2 yLength = new Vector2(0,25);
		Vector2 bound = new Vector2 (32, 22);
		public List<int> highScoreList;
		public List<float> averageScoreList;
		int xAxisValue;
		int yAxisValue;
		public Text caption;
		public Text info;

		void Start(){
			highScoreList = game.highScoreList	;
			averageScoreList = game.averageScoreList;
			xAxis.SetPosition (0, graphOrigin);
			xAxis.SetPosition (1, graphOrigin + xLength);
			yAxis.SetPosition (0, graphOrigin);
			yAxis.SetPosition (1, graphOrigin + yLength);
			caption.text = "<color=orange><b>HighScores</b></color>\n\n\n\n\n\n\n<color=green><b>Average</b></color>";
			caption.transform.position = new Vector2 (Screen.width * 0.91f, Screen.height * 0.5f);
			info.text = "Not Enough Imformation. \nWait till the thrid generation to show the graph.";
			info.transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
			
		}

		int MaxOfList(List<int> list){
			int MaxOfList = 0;
			for (int i = 0; i < list.Count; i++){
				if (list [i] > MaxOfList)
					MaxOfList = list [i];
			}
			return MaxOfList;
		}

		void Update(){
			xAxisValue = game.round;
			yAxisValue = MaxOfList (highScoreList);
			if(averageScoreList.Count ==2){
				info.gameObject.SetActive (false);
			}
			if (averageScoreList.Count > 1) {
				//Debug.Log ("HS:  " + highScoreList.Count + "   " + highScoreList [highScoreList.Count - 1]);
				//Debug.Log ("aver:  " + averageScoreList.Count + "   " + averageScoreList [averageScoreList.Count - 1]);
				highLine.numPositions = highScoreList.Count;
				averageLine.numPositions = averageScoreList.Count;
				highLine.SetPosition (0, graphOrigin + new Vector2 (0, ((float)highScoreList [0] / yAxisValue) * bound.y));
				averageLine.SetPosition (0, graphOrigin + new Vector2(0, (averageScoreList[0] / yAxisValue) * bound.y));
					//highLine.SetPosition (1, graphOrigin + new Vector2 (0.5f*bound.x, ((float)highScoreList [highScoreList.Count - 1] / yAxisValue) * bound.y));
				for (int i = 1; i < highScoreList.Count;i++){
					highLine.SetPosition (i, graphOrigin + new Vector2 ((i/(float)(highScoreList.Count-1)) * bound.x, ((float)highScoreList [i] / yAxisValue) * bound.y));
					averageLine.SetPosition (i, graphOrigin + new Vector2 ((i / (float)(highScoreList.Count - 1)) * bound.x, (averageScoreList [i] / yAxisValue) * bound.y));

				}
			//highLine.SetPosition (2, graphOrigin + new Vector2( bound.x, ((float)highScoreList[highScoreList.Count-1]/yAxisValue)* bound.y));
			}

		}
	}
}