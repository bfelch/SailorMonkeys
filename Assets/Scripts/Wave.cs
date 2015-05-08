using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

	private float height = 0;
	private float radius = 0;
	
	private bool grows = true;
	private bool gamePaused = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!gamePaused) {
			radius += Time.deltaTime * 15.0f;
				
			if (grows) {
				height = radius * 5.0f;
				
				if (radius > 1.0f) {
					grows = false;
				}
			} else {
				height = (.001428f * radius * radius) - (.163f * radius) + 4.729f;
				
				if (radius > 60.0f) {
					Destroy(gameObject);
					Player.canMakeWave = true;
				}
			}
			
			transform.localScale = new Vector3(radius, height, radius);
		}
	}
	
	public float GetHeight() {
		return height;
	}
	
	public void SetPaused(bool paused) {
		gamePaused = paused;
	}
}
