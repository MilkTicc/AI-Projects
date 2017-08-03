using UnityEngine;


    public interface IActor {
        void SetAcc( float x_axis, float y_axis );
        float MaxSpeed { get; }
		Vector2 Velocity { get; }
		Vector2 Position { get;}
		void SetVelo( float x_axis, float y_axis );
    }
