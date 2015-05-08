using UnityEngine;
using System.Collections;

public class SeaMonster : MonoBehaviour {
	
	private enum MoveState {
		None, Up, Down
	};
	
	public GameObject monster;
	//private GameObject boat;
	
	private float startY = -6.4f;
	private float endY = 0;
	private float moveSpeed = .2f;
	
	private MoveState moveState = MoveState.None;

	// Use this for initialization
	void Start () {
		//boat = GameObject.Find("Boat(Clone)");
	}
	
	// Update is called once per frame
	void Update () {
		float posY;
		
		switch (moveState) {
			case MoveState.Up:
			posY = monster.transform.localPosition.y;
			monster.transform.localPosition = new Vector3(0, posY + moveSpeed, 0);
			
			if (monster.transform.localPosition.y > endY) {
				moveState = MoveState.Down;
			}
			break;
			case MoveState.Down:
			posY = monster.transform.localPosition.y;
			monster.transform.localPosition = new Vector3(0, posY - moveSpeed, 0);
			
			if (monster.transform.localPosition.y < startY) {
				moveState = MoveState.None;
			}
			break;
		}
	}
	
	public void DoSeaMonster() {
		moveState = MoveState.Up;
	}
}
