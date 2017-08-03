using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public Grid grid;
	public Guard Guard1;
	public Guard Guard2;
	public Guard Guard3;
	public PlayerController player;
	//List<Guard> guards;
	public int levelIntel = 3;
	//bool gameOver = false;
	public bool gameWin = false;
	public GameObject gameOverPanel;
	public GameObject gameWinPanel;
	public float scurityLevel = 0;
	bool securityUp;

	//void CreateGuards(){
	//	Guard newGuard = Instantiate<Guard> (GuardPrefab);
	//	newGuard.transform.position = grid.RandomNode.Pos;
	//	newGuard.gameObject.SetActive (true);
	//	guards.Add (newGuard);
	//}
	void Awake(){
		grid.CreateGrid (31,16);
		Guard1.AddPatrolNodes (grid.GetNode (2, 1));
		Guard1.AddPatrolNodes (grid.GetNode (17, 1));
		Guard2.AddPatrolNodes (grid.GetNode (19, 12));
		Guard2.AddPatrolNodes (grid.GetNode (19, 3));
		Guard3.AddPatrolNodes (grid.GetNode (1, 3));
		Guard3.AddPatrolNodes (grid.GetNode (1, 13));
		
	}

	public void MaxSecurity(){
		scurityLevel = 60;
	}

	//public void SetGameOver () { gameOver = true; }
	public void SetGameWin(){gameWin = true;}

	void GameOver(){
		Time.timeScale = 0;
		gameOverPanel.SetActive (true);
	}
	void GameWin(){
		Time.timeScale = 0;
		gameWinPanel.SetActive (true);
		
	}


	void Start ()
	{
		//guards = new List<Guard> ();
		//CreateGuards ();
		//Guard1 = new Guard (new List<GridNode> () { grid.GetNode (2, 1), grid.GetNode (16, 1) });
		//Guard2 = new Guard (new List<GridNode> () { grid.GetNode (19, 3), grid.GetNode (19, 12) });
	}
	
	// Update is called once per frame
	void Update () {
		if(scurityLevel < 60)
			{scurityLevel += Time.deltaTime; }

		if (scurityLevel > 59.9f)
			securityUp = true;

		if (securityUp)
			Guard3.gameObject.SetActive (true);
	

		if(player.isCaught){
			GameOver ();
			if (Input.GetKeyDown (KeyCode.Space)) {
				SceneManager.LoadScene ("Game");
				Time.timeScale=1;
			}
		}
		else if(gameWin){
			GameWin ();
			if (Input.GetKeyDown (KeyCode.Space))
			
			{SceneManager.LoadScene ("Game"); 
				Time.timeScale = 1;
			}
		}
	}
}
