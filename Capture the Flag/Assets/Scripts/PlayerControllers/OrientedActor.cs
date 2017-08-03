using UnityEngine;
using System.Collections.Generic;

namespace AISandbox
{
	public class OrientedActor : MonoBehaviour, IActor
	{
		private const float MAX_SPEED = 6.0f;
		private const float STEERING_ACCEL = 60.0f;
		private const float STEERING_LINE_SCALE = 3.0f;
		private const float COLOR_RANGE = 1.0f;
		public Vector2 initialVelocity = Vector2.zero;
		public bool wrapScreen = false;
		public Vector2 position;
		public IJob currentJob;
		public LineRenderer _steering_line;
		public Grid grid;
		public CapturetheFlag CtF;
		public Pathfinding pathFind;
		public SpriteRenderer _renderer;
		public GridNode currentNode;
		public enum Team { cyan, mangetta };
		public Flag teamFlag;
		public bool captured = false;
		public bool jailed = false;
		public bool escorting = false;
		public UI ui;
		Color cyan = new Color (0, 1, 1);
		Color mangetta = new Color (1, 0, 1);
		public Color teamColor;
		Team team;
		public Defender defender;
		Flagger flagger;
		public int teamNum;
		public string teamName;
		string jobName;
		public List<OrientedActor> enemies;
		public List<OrientedActor> teammates;
		Vector2 _steering = Vector2.zero;
		Vector2 _acceleration = Vector2.zero;
		Vector2 _velocity = Vector2.zero;

		void Awake ()
		{
			_renderer = GetComponent<SpriteRenderer> ();
		}

		void Start ()
		{
			_velocity = initialVelocity;
			JoinTeam ();
		}
		public void SetJob (int job)
		{
			if (job == 1) {
				jobName = "Defender";
				defender = new Defender (this);
				currentJob = defender;
			}
			else
				{
				jobName = "Attacker";
				flagger = new Flagger (this);
				currentJob = flagger;
			}
		}

		//public void SetTeam (Team s) { team = s;}
		public Team _Team{get{return team;}
	set{team = value;}}

		public void JoinTeam(){
			switch(team){
			case Team.cyan:
				teamColor =cyan;
				teamFlag = CtF.cyanFlag;
				break;
			case Team.mangetta:
				teamName = "Mangetta Player ";
				teamColor = mangetta;
				teamFlag = CtF.mangettaFlag;
				break;
			}
			_renderer.color = teamColor;
		}

        public void SetAcc( float x_axis, float y_axis ) {
            _steering = Vector2.ClampMagnitude( new Vector2(x_axis, y_axis), 50.0f );
            _acceleration = _steering;
        }

		public void SetVelocity( float x_axis, float y_axis ) {
			_velocity = Vector2.ClampMagnitude( new Vector2(x_axis, y_axis), MAX_SPEED );

		}

		public void PrintMessage(string message){
			Debug.Log (teamName);
			Debug.Log (jobName);
			ui.AddMessage (teamName+teamNum+" - "+jobName+ "\n"+message, cyan);
		}

		public void CallforHelp(){
			List<OrientedActor> availableTeammate = new List<OrientedActor> ();
			foreach(OrientedActor teammate in teammates){
				if (!teammate.captured&&!teammate.escorting)
					availableTeammate.Add (teammate);
			}
			if (availableTeammate.Count != 0)
			{
				availableTeammate [0].defender.DeInfluence ();
				availableTeammate [0].currentJob.CurrentState.ResetPathColor ();
				availableTeammate [0].currentJob = new Rescuer (availableTeammate [0], this);
				availableTeammate [0].jobName = "Rescuer";
			}
		}

        public float MaxSpeed {
			get { return MAX_SPEED; }}

        public Vector2 Velocity {
			get { return _velocity; }}
        
		public Vector2 Position {
			get { return position; }}
		
		


        private void Update() {
			currentJob.Update ();
            _velocity += _acceleration * Time.deltaTime;
			//_velocity = _velocity.normalized * MAX_SPEED;
            position += (_velocity * Time.deltaTime);
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.Normalize(_velocity));
			currentNode = grid.PostoNode (transform.position);
            //_steering_line.transform.rotation = Quaternion.identity;
            //_steering_line.SetPosition( 1, _steering * STEERING_LINE_SCALE );
            //_steering_line.gameObject.SetActive(true);
            _steering = Vector2.zero;
            _acceleration = Vector2.zero;
        }
    }
}