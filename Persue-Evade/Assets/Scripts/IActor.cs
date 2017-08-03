using UnityEngine;



namespace AISandbox {
    public interface IActor {
        void SetInput( float x_axis, float y_axis );
		void SetVelocity (float i_x, float i_y);
		Vector2 GetVelocity ();
	//	float GetMaxSpeed();

    }
}