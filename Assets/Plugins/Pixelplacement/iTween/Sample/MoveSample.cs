using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{
	public Transform finalPosition;

	void Start(){
		//iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
		iTween.MoveTo(gameObject, finalPosition.position, 2);
	}
}

