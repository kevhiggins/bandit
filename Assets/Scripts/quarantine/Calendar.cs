using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour {

	public GameObject dayDial;
	public Text dateText;

	[SerializeField]
	private int daysInSeason = 5;

	[SerializeField]
	private int date = 1;
	[SerializeField]
	private string seasonText;
	private int seasonCount = 0;
	private string[] seasonName = {"spring","summer","fall","winter"};

	// Use this for initialization
	void Start () {
		
		seasonText = seasonName[seasonCount];
		dateText.text = (seasonText+" "+date);
		dayDial.GetComponent<Image>().fillAmount = ((float)date / daysInSeason);
	}

	public void AdvanceDay ()
	{
		date++;

		if (date > daysInSeason) 
		{
			seasonCount++;
			if (seasonCount > 3) {
				seasonCount = 0;
			}
			seasonText = seasonName[seasonCount];
			date = 1;
		}

		dateText.text = (seasonText+" "+date);
		dayDial.GetComponent<Image>().fillAmount = ((float)date / daysInSeason);
	}
}
