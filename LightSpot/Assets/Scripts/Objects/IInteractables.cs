using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractables  {

	GameObject ThisGameObject ();
	void Interact (PlayerController pl);
	void Notify ();
}
