using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private float boatIntegrity = 1.0f;
	private int numMonkeys = 10;
	private int numClicks = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void BoatDamage(float damageTaken) {
		boatIntegrity -= damageTaken;
	}
	
	public void LoseMonkey(int monkeysLost) {
		numMonkeys -= monkeysLost;
	}
	
	public void DoClick(float posX, float posY) {
		numClicks++;
	}
}
