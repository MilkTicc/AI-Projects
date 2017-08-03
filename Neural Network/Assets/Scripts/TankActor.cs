using UnityEngine;
using System.Collections.Generic;


namespace AISandbox {
    public class TankActor : MonoBehaviour, IActor {
		#region
		[SerializeField]
		public Vector2 initialVelocity = Vector2.zero;
		public bool wrapScreen = false;
		private bool _DrawVectors = false;
		public bool DrawVectors {
			get {
				return _DrawVectors;
			}
			set {
				_DrawVectors = value;
				_left_steering_line.gameObject.SetActive( _DrawVectors );
				_right_steering_line.gameObject.SetActive (_DrawVectors);
				closest0MineLine.gameObject.SetActive (_DrawVectors);
				closest1MineLine.gameObject.SetActive (_DrawVectors);
				closest2MineLine.gameObject.SetActive( _DrawVectors );
			}
		}
		public LineRenderer _left_steering_line;
		public LineRenderer _right_steering_line;
		
		public SpriteRenderer _sprite;
		public Sprite _eliteSprite;
		public Sprite _normalSprite;
		public Sprite _mutantSprite;
		private bool _screenWrapX = false;
		private bool _screenWrapY = false;
		
		private Vector2 _left_steering = Vector2.zero;
		private Vector2 _right_steering = Vector2.zero;
		private Vector3 _velocity = Vector3.zero;
		private float _orientation = 0.0f;
        private const float MAX_SPEED           = 25.0f;
        private const float STEERING_SPEED      = 12.5f;
        private const float NO_INPUT_DECEL      = -75.0f;
        private const float ROT_SPEED           = 5.0f;
        private const float STEERING_LINE_SCALE = 4.0f;
		public float MaxSpeed { get { return MAX_SPEED; } }
		public Vector3 Velocity { get { return _velocity; } }
		#endregion

		int serialNum;
		//int score = 0;
		NeuroNetWork neuroNet;
		List<Mines> allMines = new List<Mines> ();
		public LineRenderer closest0MineLine;
		public LineRenderer closest1MineLine;
		public LineRenderer closest2MineLine;
		float closest0MineDis;
		float closest0MineAng;
		float closest1MineDis;
		float closest1MineAng;
		float closest2MineDis;
		float closest2MineAng;
		float closestTankDis;
		float closestTankAng;
		float [] neuroInputs;
		//float [] weights;
		MovementList movementList;
		//int movementCount=0;
		//float movementTime = 0;
		//public float[] Weights{ set { weights = value;}}
		public int Score{ get { return movementList.Score;}}
		public MovementList MoveList{ get { return movementList;} set { movementList = value;}}
		public List<Mines> Mines{ set { allMines = value;}}
		public int SerialNum { get { return serialNum; } set { serialNum = value;}}

        private void Start() {
            _velocity = initialVelocity;
			DrawVectors = false;
			//weights = new float [56];
			//for (int i = 0; i < weights.Length; i++){
			//	weights[i] = Random.Range (-1f, 1f);
			//}
			neuroNet = new NeuroNetWork ();
			//Debug.Log (AngToVec(0f));
			//Debug.Log (Mathf.Acos (-1) / Mathf.PI * Mathf.Sign (0))
			neuroInputs = new float [7] ;
			;
        }

		public void SetMines(List<Mines > minelist){
			allMines.Clear ();
			for (int i = 0; i < minelist.Count; i ++){
				allMines.Add (minelist [i]);
			}
		}

		public void ResetTank( MovementList list, List<Mines>mineslist){
			movementList = list;
			//score = 0;
			if (movementList.IsElite)
			{
				_sprite.sprite = _eliteSprite;
				gameObject.name = "Tank "+ serialNum   + "<color=red> <Elite></color>";
				movementList.IsElite = false;
			}
			else if(movementList.IsMutant){
				_sprite.sprite = _mutantSprite;
				gameObject.name = "Tank " + serialNum + " <color=cyan><Mutant></color>";
				movementList.IsMutant = false;
			}
			else{
				_sprite.sprite = _normalSprite;
				gameObject.name = "Tank " + serialNum;
			}

			SetMines (mineslist);
			transform.position = new Vector3 (0, 0, 0);
			_velocity = new Vector3 (0, 0, 0);
			_orientation = 0;
			//score = 0;
			//movementCount = 0;
			//movementTime = 0;
		}

        public void SetInput( float y_axis, float y2_axis ) {
			_left_steering.y = Mathf.Clamp( y2_axis, -1.0f, 1.0f );
			_right_steering.y = Mathf.Clamp( y_axis, -1.0f, 1.0f );
			//_left_steering.y = y2_axis;
			//_right_steering.y = y_axis;
			if (_left_steering.y < 0.0f) {
                _left_steering.y *= 0.5f;
            }
            if( _right_steering.y < 0.0f ) {
                _right_steering.y *= 0.5f;
            }
        }

		float DisToMine(Mines mine){
			return (mine.transform.position - transform.position).magnitude;
		}
		float AngToMine(Mines mine){
			Vector2 v = (mine.transform.position - transform.position).normalized;

			//return v.y / Mathf.Sqrt (v.x * v.x + v.y * v.y);
			//return Mathf.Atan (v.x / v.y) / Mathf.PI;
			return Mathf.Acos (v.y) / Mathf.PI * Mathf.Sign (v.x);
		}

		Vector2 AngToVec(float ang){
				return new Vector2 (Mathf.Sin(ang*Mathf.PI),Mathf.Cos(ang* Mathf.PI));

			//return new Vector2 (Mathf.Tan(Mathf.PI*ang),1f).normalized;
		}

		void GetClosestInfo(){
			int rand = Random.Range (0, allMines.Count);
			Mines [] closestMines =  { allMines [rand], allMines [rand], allMines [rand]};
			foreach(Mines mine in allMines)
			{
				if (DisToMine (mine) < DisToMine (closestMines [0])) {
					closestMines [2] = closestMines [1];
					closestMines [1] = closestMines [0];
					closestMines [0] = mine;
				} else if (DisToMine (mine) < DisToMine (closestMines [1])) {
					closestMines [2] = closestMines [1];
					closestMines [1] = mine;
				} else if (DisToMine (mine) < DisToMine (closestMines [2])) {
					closestMines [2] = mine;
				}
			}
			closest0MineDis = DisToMine (closestMines [0]) ;
			closest1MineDis = DisToMine (closestMines [1]);
			closest2MineDis = DisToMine (closestMines [2]);
			closest0MineAng = AngToMine (closestMines [0]);
			closest1MineAng = AngToMine (closestMines [1]);
			closest2MineAng = AngToMine (closestMines [2]);
			neuroInputs = new float [7] { closest0MineDis/ 10f, closest0MineAng, closest1MineDis/ 10f, closest1MineAng, closest2MineDis/ 10f, closest2MineAng, -1 };
			//neuroInputs = new float [3] { closest0MineDis / 10f, closest0MineAng, -1f };
		}

        Vector3 ScreenWrap() {
            Vector3 position = transform.position;
            if( wrapScreen ) {
                if( _sprite.isVisible ) {
                    _screenWrapX = false;
                    _screenWrapY = false;
                    return position;
                } else {
                    Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
                    if( !_screenWrapX && (viewportPosition.x > 1 || viewportPosition.x < 0) ) {
                        position.x = -position.x;
                        _screenWrapX = true;
                    }
                    if( !_screenWrapY && (viewportPosition.y > 1 || viewportPosition.y < 0) ) {
                        position.y = -position.y;
                        _screenWrapY = true;
                    }
                }
            }
            return position;
        }

		void CollectMines(){
			//List<Mines> minesToBeRemoved = new List<Mines> ();
			foreach (Mines mine in allMines)
			{
				if((transform.position - mine.transform.position).magnitude < 1f)
				{
					mine.Flash ();
					//score++;
					movementList.Score++;
				};
			}
		}

        private void FixedUpdate() {
			//if (movementCount >= 120)
			//	//Destroy (gameObject);
			//	ResetTank (inputList);


			CollectMines ();
            Vector3 position = ScreenWrap();
			//MoveAsListInput (inputList);
			GetClosestInfo ();
			//Debug.Log (neuroInputs[0]); 

			//Vector2 a =  neuroNet.CalcResult (movementList.WeightList, neuroInputs);
			////Debug.Log (a.x +"  "+ a.y);
			//SetInput (a.x, a.y);

			float [] input = neuroNet.NueroOutput (movementList.WeightList, neuroInputs, 3, 6);
			SetInput (input [0], input [1]);

			#region

			float speed = _left_steering.y * STEERING_SPEED + _right_steering.y * STEERING_SPEED;
            float rot = (_right_steering.y - _left_steering.y) * ROT_SPEED;

            _orientation += rot;
            transform.rotation = Quaternion.Euler( 0.0f, 0.0f, _orientation );

            _velocity = transform.up * speed;
            if( _left_steering.y == 0.0f && _right_steering.y == 0.0f && _velocity.sqrMagnitude > 0.0f ) {
                _velocity += Vector3.ClampMagnitude( _velocity.normalized * NO_INPUT_DECEL * Time.fixedDeltaTime, _velocity.magnitude );
            }
            _velocity = Vector3.ClampMagnitude( _velocity, MAX_SPEED );

			position += _velocity * Time.fixedDeltaTime;
            transform.position = position;

            //_left_steering_line.transform.rotation = Quaternion.identity;
            _left_steering_line.SetPosition( 1, _left_steering * STEERING_LINE_SCALE );
            //_right_steering_line.transform.rotation = Quaternion.identity;
            _right_steering_line.SetPosition( 1, _right_steering * STEERING_LINE_SCALE );
			closest0MineLine.transform.rotation = Quaternion.identity;
			closest1MineLine.transform.rotation = Quaternion.identity;
			closest2MineLine.transform.rotation = Quaternion.identity;
			
			closest0MineLine.SetPosition (1, AngToVec (closest0MineAng) * closest0MineDis);
			closest1MineLine.SetPosition (1, AngToVec (closest1MineAng) * closest1MineDis);
			closest2MineLine.SetPosition (1, AngToVec (closest2MineAng) * closest2MineDis);

			if(Input.GetKeyDown(KeyCode.Space)){
				DrawVectors = !_DrawVectors;
			}

			#endregion
        }
    }
}
