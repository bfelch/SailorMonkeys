using UnityEngine;
using System.Collections;

public class BananaIsland : MonoBehaviour {

	public GameObject bananas;
	public GameObject monkey;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void MonkeyJumpedShip() {
		int numBananas = bananas.transform.childCount;
		int currentNanner = Random.Range(0, numBananas);
		
		Vector3 newPos = bananas.transform.GetChild(currentNanner).position;
		newPos.y = .83f;
		
		Destroy(bananas.transform.GetChild(currentNanner).gameObject);
		
		GameObject newMonkey = (GameObject) Instantiate(monkey, newPos, Quaternion.Euler(0, Random.Range(0, 360.0f), 0));
		newMonkey.transform.parent = transform;
	}
}
