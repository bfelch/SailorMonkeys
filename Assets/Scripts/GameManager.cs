using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	
	public static int gridH = 10;
	public static int gridW = 15;
	
	public GameObject bananaIsland;
	public GameObject seaMonster;
	public GameObject finalIsland;
	public GameObject player;
	public GameObject waterPlane;
	
	public GameObject scorePanel;
	public Text statsText;
	public Text scoreText;
	
	private int numBananaIslands = 8;
	private int numSeaMonsters = 5;
	
	private bool[,] grid;

	// Use this for initialization
	void Start () {
		grid = new bool[gridH, gridW];
		
		//BuildGrid();
		//MakeWaterPlane();
		
		SpawnGridItem(player, 1, 1);
		SpawnGridItem(finalIsland, gridW - 2, gridH - 2);
		
		for (int i = 0; i < numBananaIslands; i++){
			RandomizeGridSpawn(bananaIsland);
		}
		
		for (int i = 0; i < numSeaMonsters; i++){
			RandomizeGridSpawn(seaMonster);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void BuildGrid() {
		for (int x = 0; x < gridW; x++) {
			for (int y = 0; y < gridH; y++) {
				GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
				tile.transform.position = new Vector3(10 * x, 0, 10 * y);
				tile.transform.localScale = new Vector3(.95f, 1, .95f);
			}
		}
	}
	
	private void MakeWaterPlane() {
		GameObject water = (GameObject) Instantiate(waterPlane);
		water.transform.position = new Vector3((gridW - 1) * 5, 0, (gridH - 1) * 5);
		water.transform.localScale = new Vector3(gridW, 1, gridH);
	}
	
	private GameObject SpawnGridItem(GameObject item, int gridX, int gridY) {
		grid[gridY, gridX] = true;
		return (GameObject) Instantiate(item, new Vector3(10 * gridX, 0, 10 * gridY), Quaternion.identity);
	}
	
	private GameObject RandomizeGridSpawn(GameObject item) {
		int x;
		int y;
		int tries = 0;
		
		do {
			x = Random.Range(1, gridW - 1);
			y = Random.Range(1, gridH - 1);
			tries++;
		} while (tries < 100 && !NoAdjacentObstacles(x, y));
		
		return SpawnGridItem(item, x, y);
	}
	
	private bool NoAdjacentObstacles(int x, int y) {
		bool noItemAbove = y == gridH - 1 ? true : !grid[y+1, x];
		bool noItemBelow = y == 0 ? true : !grid[y-1, x];
		bool noItemLeft = x == 0 ? true : !grid[y, x-1];
		bool noItemRight = x == gridW - 1 ? true : !grid[y, x+1];
		bool noItemOn = !grid[y, x];
		
		return noItemAbove && noItemBelow && noItemLeft && noItemRight && noItemOn;
	}
	
	public void FinishGame(Player player) {
		scorePanel.SetActive(true);
		statsText.text = "";
		scoreText.text = "";
		
		statsText.text += string.Format("{0:0.00%}\n", player.GetIntegrity());
		statsText.text += player.GetRemainingMonkeys() + "\n";
		statsText.text += player.GetNumClicks();
		
		scoreText.text += "x " + player.GetIntegrityMod().ToString("0.00") + "\n";
		scoreText.text += player.GetMonkeyScore() + "\n";
		scoreText.text += "x " + player.GetClickMod().ToString("0.00") + "\n\n";
		scoreText.text += player.GetFinalScore().ToString("0.00");
	}
	
	public void PlayAgain() {
		Application.LoadLevel("MainGame");
	}
}
