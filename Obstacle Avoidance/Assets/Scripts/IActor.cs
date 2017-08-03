using UnityEngine;

namespace AISandbox {
    public interface IActor {
        void SetInput( float x_axis, float y_axis );
		void SetVelocity (float x_axis, float y_axis);
        float MaxSpeed { get; }
        Vector2 Velocity { get; }
		Vector2 Position{ get; }
		void SetPosition(float x_axis, float y_axis);
		float Radius { get;}
    }
}