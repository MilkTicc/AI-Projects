using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox{
public class CalculateState : IStates {
	private readonly TreasureFinderController tFC;
		IActor _actor;
	public Pathfinding pathFinder;
	public Grid grid;
		public TreasureFind treasureFind;
	public string Name{get{return "Calculating...";}}
	// Use this for initialization
	public CalculateState(TreasureFinderController tfc){
		tFC = tfc;
		_actor = tFC.GetComponent<IActor> ();
		pathFinder = GameObject.Find("Pathfinding").GetComponent<Pathfinding>();
		grid = GameObject.Find ("Grid").GetComponent<Grid> ();

	}

	public void CalculateAvailability(){
			tFC.canGetGreenKey =tFC.haveGreenKey? false: pathFinder.CheckAvailability (tFC, grid.GetNode (grid.GreenKeyPos),true);
			tFC.canGetBlueKey = tFC.haveBlueKey? false: pathFinder.CheckAvailability (tFC, grid.GetNode (grid.BlueKeyPos),true);
			tFC.canGetRedKey =tFC.haveRedKey? false:  pathFinder.CheckAvailability (tFC, grid.GetNode (grid.RedKeyPos),true);
			tFC.canGetGreenDoor = pathFinder.CheckAvailability (tFC, grid.GetNode (grid.GreenDoorPos),false);
			tFC.canGetBlueDoor = pathFinder.CheckAvailability (tFC, grid.GetNode (grid.BlueDoorPos),false);
			tFC.canGetRedDoor = pathFinder.CheckAvailability (tFC, grid.GetNode (grid.RedDoorPos),false);
			tFC.canGetTreasure = pathFinder.CheckAvailability (tFC, grid.GetNode (grid.TreasurePos),true);

	}

	public void CalcGal ()
		{
			if (tFC.haveTreasure)
				ToHappyState ();
			else if (tFC.canGetTreasure)
				SetGoal (grid.GetNode (grid.TreasurePos), "Treasure");
			else if (tFC.canGetGreenKey)
				SetGoal (grid.GetNode (grid.GreenKeyPos), "Green Key");
			else if (tFC.haveGreenKey && tFC.canGetGreenDoor&&!treasureFind.greenDoorOpened) {
				SetGoal (grid.GetNode (grid.GreenDoorPos), "Green Door");
				Debug.Log ("GD:" + tFC.canGetGreenDoor + "GK:" + tFC.canGetGreenKey + " hGK:" + tFC.haveGreenKey);
				}
			else if(tFC.canGetBlueKey)
				SetGoal (grid.GetNode (grid.BlueKeyPos),"Blue Key");
			else if (tFC.haveBlueKey && tFC.canGetBlueDoor&&!treasureFind.blueDoorOpened)
				SetGoal (grid.GetNode (grid.BlueDoorPos),"Blue Door");
			else if(tFC.canGetRedKey)
				SetGoal (grid.GetNode (grid.RedKeyPos),"Red Key");
			else if (tFC.haveRedKey && tFC.canGetRedDoor&&!treasureFind.redDoorOpened)
				SetGoal (grid.GetNode (grid.RedDoorPos),"Red Door");
			
			 
			else{
				ToSadState ();
			}
	}

	public void Enter(){
			CalculateAvailability ();
			_actor.SetVelo (0f, 0f);
			treasureFind = GameObject.Find ("TreasureFind").GetComponent<TreasureFind>();
			}

	public void Exit () {
		
	}

	void ToMoveState (string goalName) { 
		tFC.currentState = tFC.moveState;
		tFC.moveState.Enter ();
			tFC.moveState.SetName (goalName);
	}
	
	void ToHappyState(){
		tFC.currentState = tFC.happyState;
	}
	void ToSadState(){
		tFC.currentState = tFC.sadState;
	}

	private void SetGoal(GridNode goalNode, string goalName){
			tFC.StartNode = grid.PostoNode (tFC._actor.Position);
			tFC.GoalNode = goalNode;

			grid.ResetGrid ();

			ToMoveState (goalName);

	}
	// Update is called once per frame
	public void Update () {
			CalcGal ();
	}
}}
