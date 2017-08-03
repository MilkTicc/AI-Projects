using UnityEngine;
using System.Collections;

namespace AISandbox {
    public class StateMachine {

        public abstract class State {
            public abstract string Name { get; }
            public virtual void Enter() {}
            public virtual void Update() {}
            public virtual void Exit() {}
        }

        public bool AddState(State state) {
            return false;
        }

        public bool RemoveState( State state ) {
            return false;
        }

        public bool RemoveState( string name ) {
            return false;
        }

        public bool SetActiveState( State state ) {
            return false;
        }

        public bool SetActiveState( string name ) {
            return false;
        }

        public void Update() {
        }
    }
}