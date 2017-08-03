using UnityEngine;

namespace AISandbox {
    public interface IActor {
        void SetAcc( float x_axis, float y_axis );
        float MaxSpeed { get; }
		Vector2 Velocity { get; set; }
		Vector2 Position { get; set; }
		void SetVelo( float x_axis, float y_axis );
    }
}