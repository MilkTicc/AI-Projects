using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {
	public IActor _actor;
	public Pathfinding pathfinder;
	public Grid grid;
	public GridNode currentNode;
	public List<GridNode> patrolNodes;
	public IStates currentState;
	PatrolState patrolState;
	public DynamicLight lineOfSight;
	GameObject player;
	//bool isPlayerSpotted = false;
	bool isPursueing = false;
	Situation situation;
	public SpriteRenderer mark;

	//public Guard (List<GridNode> pNodes)
	//{
	//	patrolNodes = pNodes;}
	void Awake(){
		_actor = GetComponent<OrientedActor> ();
		//mark = GetComponentInChildren<SpriteRenderer> ();
		lineOfSight = GetComponentInChildren<DynamicLight> ();
		situation = GameObject.FindGameObjectWithTag ("Situation").GetComponent<Situation> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Start () {
		
		currentNode = grid.PostoNode (transform.position);
			if (lineOfSight)
				lineOfSight.InsideFieldOfViewEvent += playerSpotted;
		//Debug.Log (this.name+ ": "+patrolNodes[0]+" "+ patrolNodes[1]);
			if (patrolNodes != null) {
				patrolState = new PatrolState (this, patrolNodes);
				currentState = patrolState;
				currentState.Enter ();
			}
		}

	void ShowMark(){

		mark.transform.position = transform.position  + new Vector3 (0.15f, 0, 0);
		mark.transform.rotation = Quaternion.AngleAxis (0, Vector3.back);
		mark.gameObject.SetActive (true);
	}

	void playerSpotted(GameObject[] p){
		foreach(GameObject pl in p)
		{
			if (pl.GetInstanceID () == player.GetInstanceID () && (transform.position-pl.transform.position).magnitude < 2) {
				if(!situation.isPlayerSpotted)
				situation.isPlayerSpotted = true;
				situation.MaxSecurity ();
			}
		}
	}

	void ToPursueState(NewActor pl){
		PursueState pursueState = new PursueState (this, pl);
		currentState = pursueState;
		currentState.Enter ();
	}

	public void AddPatrolNodes(GridNode node){
		patrolNodes.Add (node);
	}

	void Update(){
		currentNode = grid.PostoNode (transform.position);

		if(situation.isPlayerSpotted && !isPursueing){
			ToPursueState (player.GetComponent<NewActor> ());
			ShowMark ();
			
			isPursueing = true;
		}

		//Debug.Log (currentNode);
		if(currentState!=null)
		currentState.Update ();

		if((transform.position - player.transform.position).magnitude< 1 && isPursueing){
			//Debug.Log ("asd");
			player.GetComponent<PlayerController> ().isCaught = true;
		}
	}
	
}
