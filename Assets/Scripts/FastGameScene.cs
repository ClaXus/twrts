using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class FastGameScene : NetworkBehaviour {

	public enum GameState{
		PlacementState,
		ActionState,
		PauseState
	}

	[SerializeField]
	GameObject[] GameUIDiscoveredOnF;
	
	[SerializeField]
	Button[] buttonsPanelButtons;
	
	[SerializeField]
	Text timerText;
	
	[SerializeField]
	Text messageText;
	
	[SerializeField]
	Button panelButtons;
	
	[SerializeField]
	Camera placementCamera;

	[SerializeField]
	GameObject[] myAllies;

	[SerializeField]
	Transform[] spawnPoints;
	
	[SerializeField]
	public GameObject[] instantiateSpells;

	private float targetTime = 4f;
	private int currentState;
	
	private int TimeToPlace;
	
	private Player currentPlayer;

	private Vector3 spawnPosition;
	// Use this for initialization

	private GameManager gM;

	private int numberOfSpawns=0;
	
	public List<GameObject> InGameEnnemies;

	public List<Camera> InGameCameras;

	void Start () {
		currentState = (int)GameState.PlacementState;
		List<GameObject> InGameEnnemies = new List<GameObject> ();
		List<Camera> InGameCameras = new List<Camera> ();
		hideOrShowGameUiOnF ();
		panelButtons.gameObject.SetActive (false);
		placementCamera.enabled = true;
		gM = (FindObjectOfType(typeof(GameManager)) as GameManager);
		//currentPlayer.OnSetLocalVisibility (false);
	}
	
	// Update is called once per frame
	void Update () {
		if ((int)GameState.PlacementState == currentState) {
			if (Input.GetMouseButtonDown (0)) {
				if(numberOfSpawns==0){
					RaycastHit hit;
					if (Physics.Raycast (placementCamera.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0)), out hit)) {
						spawnPosition = hit.point;
						spawnPosition.y += 1;
						gM.AddPlayerForAGame(spawnPosition);
						numberOfSpawns +=1;
					}
				}
				else if(numberOfSpawns < 4){
					RaycastHit hit;
					if (Physics.Raycast (placementCamera.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0)), out hit)) {
						spawnPosition = hit.point;
						spawnPosition.y += 1;
						//gM.AddPlayerForAGame(spawnPosition);
						if(isLocalPlayer)
							Cmd_SpawnAlly();
						else if (isServer)
						{
							Rpc_SpawnAlly();
						}
						else{
							//SpawnAlly();
						}
					}
				}
			}

			if (targetTime <= 0.0f) {
				numberOfSpawns=0;
				timerEnded();
			} else {
				targetTime -= Time.deltaTime;
				timerText.text = targetTime.ToString ("0.##");
			}
		} else if ((int)GameState.ActionState == currentState) {
			if (Input.GetKeyDown (KeyCode.F)) {
				hideOrShowGameUiOnF ();
			}
		}
	}
	[Command]
	public void Cmd_SpawnAlly(){
		Debug.LogError ("CmdSpawn");
		myAllies[numberOfSpawns-1] = (GameObject) Instantiate(myAllies[numberOfSpawns-1], spawnPosition, spawnPoints[0].rotation);
		//gM.AddAlly(ref myAllies, numberOfSpawns, spawnPosition);
		NetworkServer.Spawn(myAllies[numberOfSpawns-1]);
		gM.AddObservables(myAllies[numberOfSpawns-1].GetComponent<NetworkIdentity>());
		
		numberOfSpawns +=1;
	}

	[ClientRpc]
	public void Rpc_SpawnAlly(){
		Debug.LogError ("RpcSpawn");
		myAllies[numberOfSpawns-1] = (GameObject) Instantiate(myAllies[numberOfSpawns-1], spawnPosition, spawnPoints[0].rotation);
		//gM.AddAlly(ref myAllies, numberOfSpawns, spawnPosition);
		NetworkServer.Spawn(myAllies[numberOfSpawns-1]);
		gM.AddObservables(myAllies[numberOfSpawns-1].GetComponent<NetworkIdentity>());
		
		numberOfSpawns +=1;
		//Cmd_SpawnAlly ();
	}
	[ClientCallback]
	public void SpawnAlly(){
		myAllies[numberOfSpawns-1] = (GameObject) Instantiate(myAllies[numberOfSpawns-1], spawnPosition, spawnPoints[0].rotation);
		//gM.AddAlly(ref myAllies, numberOfSpawns, spawnPosition);
		NetworkServer.Spawn(myAllies[numberOfSpawns-1]);
		gM.AddObservables(myAllies[numberOfSpawns-1].GetComponent<NetworkIdentity>());
		//Cmd_SpawnAlly ();
		numberOfSpawns +=1;
	}
	 public void timerEnded(){
		if (currentPlayer.moverScript.enabled)
			currentPlayer.playerCamera.gameObject.SetActive (true);
		timerText.gameObject.SetActive (false);
		currentState = (int)GameState.ActionState;
		hideOrShowGameUiOnF ();
		panelButtons.gameObject.SetActive (true);
		placementCamera.gameObject.SetActive (false);
		//ManageCameras ();
		//initializeButtons (ref spellButtonsFSG, currentPlayer);
		messageText.text = "Défendez à tout prix le totem !";
	}

	void hideOrShowGameUiOnF(){
		for (int i=0; GameUIDiscoveredOnF.Length>i; i++) {
			if(!GameUIDiscoveredOnF[i].activeSelf)
				GameUIDiscoveredOnF[i].SetActive (true);
			else
				GameUIDiscoveredOnF[i].SetActive (false);
		}
	}

	private Button[] spellButtonsFSG;

	public void initializeButtons(ref Button[] spellButtons, Player p){
		//spellButtonsFSG = spellButtons;
		Debug.LogWarning ("Initialize");
		currentPlayer = p;
		for (int i=0;i<spellButtons.Length && i< buttonsPanelButtons.Length; i++) {
			//Debug.LogWarning ("Initialize Button " + i);
			AddListener(buttonsPanelButtons[i], "init");
			spellButtons[i] = buttonsPanelButtons[i];
		}
	}
	
	void AddListener(Button b, string value){
		//Debug.LogWarning ("Add Listener" + b.ToString());
		b.onClick.AddListener(() => currentPlayer.btnClicked(b));
	}

	public void getInstantiateSpells(Player p){
		p.spellAnimation = instantiateSpells;

	}
}
