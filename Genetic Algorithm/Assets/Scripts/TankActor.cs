using UnityEngine;
using System.Collections.Generic;

namespace AISandbox {
    public class TankActor : MonoBehaviour, IActor {
        private const float MAX_SPEED           = 25.0f;
        private const float STEERING_SPEED      = 12.5f;
        private const float NO_INPUT_DECEL      = -75.0f;
        private const float ROT_SPEED           = 5.0f;
        private const float STEERING_LINE_SCALE = 4.0f;
		int serialNum;
		//float [] [] inputList;
		MovementList inputList;
		//int score = 0;
		List<Mines> mines = new List<Mines> ();
		int movementCount=0;
		float movementTime = 0;
        public Vector2 initialVelocity = Vector2.zero;
        public bool wrapScreen = false;

        [SerializeField]
        private bool _DrawVectors = true;
        public bool DrawVectors {
            get {
                return _DrawVectors;
            }
            set {
                _DrawVectors = value;
                _left_steering_line.gameObject.SetActive( _DrawVectors );
                _right_steering_line.gameObject.SetActive( _DrawVectors );
            }
        }
		public MovementList InputList{ get { return inputList;} set { inputList = value;}}
		public List<Mines> Mines{ set { mines = value;}}
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
		public float MaxSpeed { get { return MAX_SPEED; } }
		public Vector3 Velocity { get { return _velocity; } }
		public int SerialNum { get { return serialNum; } set { serialNum = value;}}

        private void Start() {
            _velocity = initialVelocity;
            DrawVectors = _DrawVectors;
        }

		public void MoveAsListInput(MovementList list){
			//int count = 0;
			//float time = 0;
			movementTime += Time.deltaTime;
			SetInput (list.Left()[movementCount],list.Right()[movementCount]);
			if(movementTime >= 0.25f){
				movementTime = 0;
				movementCount++;
			}

		}

		public void SetMines(List<Mines > minelist){
			mines.Clear ();
			for (int i = 0; i < minelist.Count; i ++){
				mines.Add (minelist [i]);
			}
		}

		public void ResetTank(MovementList list, List<Mines> mineslist){
			inputList = list;
			SetMines (mineslist);
			//score = 0;
			if (inputList.IsElite)
			{
				_sprite.sprite = _eliteSprite;
				gameObject.name = "Tank "+ serialNum   + " <Elite>";
				inputList.IsElite = false;
			}
			else if(inputList.IsMutant){
				_sprite.sprite = _mutantSprite;
				gameObject.name = "Tank " + serialNum + " <Mutant>";
				inputList.IsMutant = false;
			}
			else{
				_sprite.sprite = _normalSprite;
				gameObject.name = "Tank " + serialNum;
			}

			transform.position = new Vector3 (0, 0, 0);
			_velocity = new Vector3 (0, 0, 0);
			_orientation = 0;

			movementCount = 0;
			movementTime = 0;
		}

        public void SetInput( float y_axis, float y2_axis ) {
			_left_steering.y = Mathf.Clamp( y2_axis, -1.0f, 1.0f );
			_right_steering.y = Mathf.Clamp( y_axis, -1.0f, 1.0f );
			//_left_steering.y = y2_axis;
			//_right_steering.y = y_axis;



			// Can only go half as fast backward
			if (_left_steering.y < 0.0f) {
                _left_steering.y *= 0.5f;
            }
            if( _right_steering.y < 0.0f ) {
                _right_steering.y *= 0.5f;
            }
        }

        private Vector3 ScreenWrap() {
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
			List<Mines> minesToBeRemoved = new List<Mines> ();
			foreach (Mines mine in mines)
			{
				if((transform.position - mine.transform.position).magnitude < 1f)
				{
					//mines.Remove (mine)
					mine.Flash ();
					minesToBeRemoved.Add (mine);
					//score++;
					inputList.Score++;
					//Debug.Log (score)
				};
			}
			foreach(Mines mine in minesToBeRemoved){
				if (mines.Contains (mine))
					mines.Remove (mine); 
			}
			//minesToBeRemoved.Clear ();
		}

        private void FixedUpdate() {
			//if (movementCount >= 120)
			//	//Destroy (gameObject);
			//	ResetTank (inputList);

			CollectMines ();
            Vector3 position = ScreenWrap();
			MoveAsListInput (inputList);

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
        }
    }
}
