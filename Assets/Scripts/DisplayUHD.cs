using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DisplayUHD : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		(FindObjectOfType (typeof(GameManager)) as GameManager).GetComponent<NetworkManagerHUD>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
