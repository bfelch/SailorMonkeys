using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public GameObject monkey;
	public GameObject wave;
	public GameObject boatCamera;
	public GameObject mainCamera;
	
	private float boatIntegrity = 1.0f;
	private int numMonkeys = 10;
	private int numClicks = 0;
	
	private bool canMove = true;
	public static bool canMakeWave = true;
	private new Rigidbody rigidbody;

    void Awake() {
        mainCamera = Camera.main.gameObject;
        rigidbody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {
		
		for (int i = 0; i < numMonkeys; i++) {
			Vector3 offset = new Vector3();
			float maxZ = 1.71f;
			
			offset.x = .53f * (i < 5 ? 1 : -1);
			offset.y = .66f;
			offset.z = ((maxZ * 4) / numMonkeys) * (i % 5) - maxZ;
			
			GameObject newMonkey = (GameObject) Instantiate(monkey, transform.position + offset, Quaternion.Euler(0, Random.Range(0, 360.0f), 0));
			
			newMonkey.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (canMove && canMakeWave && Input.GetMouseButtonDown(0)) {
			DoClick();
		}
		
		/*
		float hAxis = Input.GetAxis("Horizontal");
		float vAxis = Input.GetAxis("Vertical");
	
		transform.position = transform.position + new Vector3(hAxis, 0, vAxis);
		*/
	}
	
	public void SetCanMove(bool canMove) {
		this.canMove = canMove;
		
		if (this.canMove) {
			rigidbody.constraints = RigidbodyConstraints.None; //RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		} else {
			rigidbody.constraints =  RigidbodyConstraints.FreezeAll;
		}
		
		mainCamera.SetActive(this.canMove);
		boatCamera.SetActive(!this.canMove);
	}
	
	public void BoatDamage(float damageTaken) {
		boatIntegrity -= damageTaken;
		Debug.Log(boatIntegrity);
		
		if (boatIntegrity < 0) {
			boatIntegrity = 0.0f;
			FinishGame();
		}
	}
	
	public void LoseMonkey(int monkeysLost) {
		numMonkeys -= monkeysLost;
		
		if (numMonkeys <= 0) {
			FinishGame();
		}
	}
	
	public void DoClick() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		int layerMask = 1 << LayerMask.NameToLayer("WaterPlane");
		
		if (Physics.Raycast(ray, out hit, 200.0f, layerMask)) {
			if (hit.collider.gameObject.tag == "Water") {
				Instantiate(wave, hit.point, Quaternion.identity);
				numClicks++;
				canMakeWave = false;
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "BananaIsland") {
			//Debug.Log("BananaIsland");
			StartCoroutine(BananaIsland(other.gameObject));
		} else if (other.tag == "SeaMonster") {
			//Debug.Log("SeaMonster");
			StartCoroutine(SeaMonster(other.gameObject));
		} else if (other.tag == "FinalIsland") {
			//Debug.Log("FinalIsland");
			StartCoroutine(FinalIsland(other.gameObject));
		}
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "Wave") {
			//Debug.Log("Wave");
			Vector3 direction = transform.position - other.transform.position;
			float force = other.gameObject.GetComponent<Wave>().GetHeight() / 2.0f;
			force = Mathf.Clamp(force, 0, 100.0f);
			
			if (force > 1.2f) {
				BoatDamage(force / 40.0f);
			}
			
			rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
		}
	}
	
	private IEnumerator BananaIsland(GameObject other) {
			SetCanMove(false);
			
			Wave currentWave = FindObjectOfType<Wave>();
			if (currentWave != null) {
				currentWave.SetPaused(true);
			}
			
			boatCamera.transform.rotation = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up);
			
			yield return new WaitForSeconds(1.0f);
			
			BananaIsland island = other.GetComponent<BananaIsland>();
			
			if (island.GetNumBananas() > 1) {
				int lostMonkeys = Random.Range(1, island.GetNumBananas());
				LoseMonkey(lostMonkeys);
				
				for (int i = 0; i < lostMonkeys; i++) {
					yield return new WaitForSeconds(.2f);
					if (transform.childCount > 2) {
						Destroy(transform.GetChild(Random.Range(2, transform.childCount)).gameObject);
						island.MonkeyJumpedShip();
					} else {
						break;
					}
				}
			}
			
			yield return new WaitForSeconds(1.0f);
			
			if (currentWave != null) {
				currentWave.SetPaused(false);
			}
			
			SetCanMove(true);
	}
	
	private IEnumerator SeaMonster(GameObject other) {
			SetCanMove(false);
			
			Wave currentWave = FindObjectOfType<Wave>();
			if (currentWave != null) {
				currentWave.SetPaused(true);
			}
			
			boatCamera.transform.rotation = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up);
			
			yield return new WaitForSeconds(1.0f);
			
			other.GetComponent<SeaMonster>().DoSeaMonster();
			
			int lostMonkeys = Random.Range(1, 4);
			LoseMonkey(lostMonkeys);
			
			for (int i = 0; i < lostMonkeys; i++) {
				yield return new WaitForSeconds(.2f);
				if (transform.childCount > 2) {
					Destroy(transform.GetChild(Random.Range(2, transform.childCount)).gameObject);
				} else {
					break;
				}
			}
			
			yield return new WaitForSeconds(1.0f);
			
			if (currentWave != null) {
				currentWave.SetPaused(false);
			}
			
			SetCanMove(true);
	}
	
	private IEnumerator FinalIsland(GameObject other) {
			SetCanMove(false);
			
			Wave currentWave = FindObjectOfType<Wave>();
			if (currentWave != null) {
				currentWave.SetPaused(true);
			}
			
			boatCamera.transform.rotation = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up);
			
			yield return new WaitForSeconds(3.0f);
			
			FinishGame();
			
			/*
			if (currentWave != null) {
				currentWave.SetPaused(false);
			}
			
			SetCanMove(true);
			*/
	}
	
	private void FinishGame() {
			FindObjectOfType<GameManager>().FinishGame();
	}
	
	public float GetIntegrity() {
		return boatIntegrity;
	}
	
	public float GetRemainingMonkeys() {
		return numMonkeys * 1.0f;
	}
	
	public float GetNumClicks() {
		return numClicks * 1.0f;
	}
	
	public float GetIntegrityMod() {
		return (-.843f * boatIntegrity * boatIntegrity) + (2.033f * boatIntegrity) + .008476f;
	}
	
	public int GetMonkeyScore() {
		return numMonkeys * 100;
	}
	
	public float GetClickMod() {
		return (.0003353f * numClicks * numClicks * numClicks) - (.00992f * numClicks * numClicks) + (.014f * numClicks) + .996f;
	}
	
	public float GetFinalScore() {
		return GetIntegrityMod() * GetMonkeyScore() * GetClickMod();
	}
}
