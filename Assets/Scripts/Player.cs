using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public GameObject monkey;
	public GameObject boatCamera;
	public GameObject mainCamera;
	
	private float boatIntegrity = 1.0f;
	private int numMonkeys = 10;
	private int numClicks = 0;
	
	private bool canMove;
	private new Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main.gameObject;
		
		rigidbody = GetComponent<Rigidbody>();
		
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
		float hAxis = Input.GetAxis("Horizontal");
		float vAxis = Input.GetAxis("Vertical");
	
		transform.position = transform.position + new Vector3(hAxis, 0, vAxis);
	}
	
	public void SetCanMove(bool canMove) {
		this.canMove = canMove;
		
		if (this.canMove) {
			rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		} else {
			rigidbody.constraints =  RigidbodyConstraints.FreezeAll;
		}
		
		mainCamera.SetActive(this.canMove);
		boatCamera.SetActive(!this.canMove);
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
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "BananaIsland") {
			Debug.Log("BananaIsland");
			StartCoroutine(BananaIsland(other.gameObject));
		} else if (other.tag == "SeaMonster") {
			Debug.Log("SeaMonster");
			StartCoroutine(SeaMonster(other.gameObject));
		} else if (other.tag == "FinalIsland") {
			Debug.Log("FinalIsland");
			StartCoroutine(FinalIsland(other.gameObject));
		} else if (other.tag == "Wave") {
			
		}
	}
	
	private IEnumerator BananaIsland(GameObject other) {
			SetCanMove(false);
			
			boatCamera.transform.rotation = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up);
			
			yield return new WaitForSeconds(1.0f);
			
			int lostMonkeys = Random.Range(1, 4);
			LoseMonkey(lostMonkeys);
			
			for (int i = 0; i < lostMonkeys; i++) {
				yield return new WaitForSeconds(.2f);
				if (transform.childCount > 2) {
					Destroy(transform.GetChild(Random.Range(2, transform.childCount)).gameObject);
					other.GetComponent<BananaIsland>().MonkeyJumpedShip();
				} else {
					break;
				}
			}
			
			yield return new WaitForSeconds(1.0f);
			SetCanMove(true);
	}
	
	private IEnumerator SeaMonster(GameObject other) {
			SetCanMove(false);
			
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
			SetCanMove(true);
	}
	
	private IEnumerator FinalIsland(GameObject other) {
			SetCanMove(false);
			
			boatCamera.transform.rotation = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up);
			
			yield return new WaitForSeconds(3.0f);
			SetCanMove(true);
	}
}
