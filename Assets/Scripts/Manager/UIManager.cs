using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
	public GameObject BalanceBar;
	public GameObject PromptText;
    public Text RemainTimeText;
    public Text RemainDistanceText;

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

    private void Update()
    {
        RemainDistanceText.text = "Goal: " + Mathf.RoundToInt(GameManager.Instance.remainDistance).ToString();

        // Convert remainTime from float to MM:SS format
        int totalSeconds = Mathf.RoundToInt(GameManager.Instance.remainTime); // Round to the nearest second
        int minutes = totalSeconds / 60; // Get the total minutes
        int seconds = totalSeconds % 60; // Get the remaining seconds

        // Format remainTime as MM:SS. The ToString("00") ensures that the value is always displayed with at least two digits, adding leading zeros if necessary.
        RemainTimeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }
}
