using UnityEngine;
using System.Collections;

/// <summary>
/// Game states. Displays the title screen and the HUD screen
/// </summary>

public class GameStates : MonoBehaviour 
{
	/// <summary>
	/// Contains all HUD GameObjects to be drawn
	/// </summary>
	public GameObject hudContainer;
	/// <summary>
	/// Contains all title GameObjects to be drawn
	/// </summary>
	public GameObject titleContainer;
	/// <summary>
	/// True if the player is playing the game, false if the title screen is active
	/// </summary>
	public static bool gameActive = false;

	/// <summary>
	/// States for title screen and HUD screen
	/// </summary>
	public enum displayStates
	{
		titleScreen = 0,
		hudScreen
	}

	void Start()
	{
		changeDisplayState(displayStates.titleScreen);
	}

	/// <summary>
	/// Change the current display state to a new state
	/// </summary>
	public void changeDisplayState(displayStates newState)
	{
		hudContainer.SetActive(false);
		titleContainer.SetActive(false);

		switch(newState)
		{
			case displayStates.titleScreen:
				gameActive = false;
				titleContainer.SetActive(true);
			break;

			case displayStates.hudScreen:
				gameActive = true;
				hudContainer.SetActive(true);
			break;
		}
	}

	/// <summary>
	/// Sets the display state from title screen to HUD screen and starts the game
	/// </summary>
	public void startGame()
	{
		changeDisplayState(displayStates.hudScreen);
	}
}
