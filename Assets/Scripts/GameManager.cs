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

    public GameObject introPanel;
    public GameObject hudPanel;
	public GameObject scorePanel;
    public Text hudText;
	public Text statsText;
	public Text scoreText;

    private Player playerInstance;
	
	private int numBananaIslands = 12;
	private int numSeaMonsters = 7;
	
	private bool[,] grid;

    private static bool showIntro;

	// Use this for initialization
	void Start () {
		showIntro = true;
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

        playerInstance = FindObjectOfType<Player>();

        playerInstance.SetCanMove(false);

        if (!showIntro) {
            StartGame();
        }
	}
	
	// Update is called once per frame
	void Update () {
        hudText.text = "";
        hudText.text += "Monkeys: " + playerInstance.GetRemainingMonkeys() + "\n";
        hudText.text += "Clicks: " + playerInstance.GetNumClicks() + "\n";
        hudText.text += "Boat: " + string.Format("{0:0.00%}", playerInstance.GetIntegrity());
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
			x = Random.Range(0, gridW);
			y = Random.Range(0, gridH);
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
	
	public void FinishGame() {
        hudPanel.SetActive(false);
		scorePanel.SetActive(true);
		statsText.text = "";
		scoreText.text = "";
		
		statsText.text += playerInstance.GetRemainingMonkeys() + "\n";
        statsText.text += playerInstance.GetNumClicks() + "\n";
        statsText.text += string.Format("{0:0.00%}", playerInstance.GetIntegrity());
		
		scoreText.text += playerInstance.GetMonkeyScore() + "\n";
        scoreText.text += "x " + playerInstance.GetClickMod().ToString("0.00") + "\n";
        scoreText.text += "x " + playerInstance.GetIntegrityMod().ToString("0.00") + "\n\n";
		scoreText.text += playerInstance.GetFinalScore().ToString("0.00");
	}

    public void StartGame() {
        introPanel.SetActive(false);
        hudPanel.SetActive(true);

        FindObjectOfType<Player>().SetCanMove(true);
        showIntro = false;
    }
	
	public void PlayAgain() {
		Application.LoadLevel("MainGame");
	}
	
	public void QuitGame() {
		Application.Quit();
	}
}
