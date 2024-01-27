using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
	public GameObject BalanceBar;
	public GameObject PromptText;

	private Text promptTextComponent; // To hold the Text component of the PromptText GameObject

	// Start is called before the first frame update
	void Start()
	{
		// Initialize the promptTextComponent by getting the Text component from PromptText GameObject
		promptTextComponent = PromptText.GetComponent<Text>();

		// Initially, the game is not started, so BalanceBar should be hidden and PromptText should be shown
		BalanceBar.SetActive(false);
		PromptText.SetActive(true);
	}

	public void OnGameStart()
	{
		// When the game starts, show the BalanceBar and hide the PromptText
		BalanceBar.SetActive(true);
		PromptText.SetActive(false);
	}

	public void OnGamePause()
	{
		// When the game is paused, hide the BalanceBar and show the PromptText with a pause message
		BalanceBar.SetActive(false);
		PromptText.SetActive(true);
		SetPromptText("Press Space to Continue");
	}

	public void OnGameContinue()
	{
		// When the game continues, show the BalanceBar and hide the PromptText
		BalanceBar.SetActive(true);
		PromptText.SetActive(false);
	}

	// Utility method to change the content of the PromptText
	public void SetPromptText(string newText)
	{
		if (promptTextComponent != null)
		{
			promptTextComponent.text = newText;
		}
	}
}
