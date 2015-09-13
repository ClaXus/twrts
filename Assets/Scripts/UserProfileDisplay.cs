using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserProfileDisplay : MonoBehaviour {

	[SerializeField]
	GameObject[] ToDisableInInitiationMode;

	[SerializeField]
	Button InitiationButton;

	[SerializeField]
	Text myMoney;

	[SerializeField]
	Text myPoints;

	private int[] stats;
	private bool[] choices;
	private PlayerInformations pI;

	void Start () {
		InformationLoader iLoader = (FindObjectOfType(typeof(GameManager)) as GameManager).GetComponent<InformationLoader>();
		iLoader.LoadPlayerInformations();
		pI = iLoader.Informations;
		myMoney.text = pI.Money.ToString();
		myPoints.text = pI.Points.ToString();
		stats = pI.Style;
		choices = pI.Choices;
		if (choices==null || !choices [0] ) {
			foreach(GameObject gO in ToDisableInInitiationMode){
				gO.SetActive(false);
			}
			InitiationButton.enabled = true;
			InitiationButton.gameObject.SetActive(true);
		}
	}
}
