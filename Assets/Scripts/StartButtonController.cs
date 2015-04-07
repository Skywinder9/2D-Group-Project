using UnityEngine;
using System.Collections;
///<summary>
/// Attached to the Start Button GameObject
/// Starts the game
///</summary>
public class StartButtonController : MonoBehaviour
{
	public GameObject upSprite;///< a place holder for our button when it is not clicked on.
	public GameObject downSprite;///< a place holder for our button when it is clicked on.
	public GameObject GameSound;///< the object that plays our gameplay music
	public GameObject StartSound;///< the object that plays our menu music
	public float downTime = 0.1f;///< float to check the time and make sure that the player clicked on it intentionally
	public GameStates stateManager = null;///< State Manager
    public PlayerStateListener playerState;///< allows us to give control to the player
	
    ///<summary>
    /// our button state down and button state up
    /// </summary>
	private enum buttonStates
	{
		up = 0,
		down
	}
	
	private buttonStates currentState = buttonStates.up;///< initiallizes our button state to up
	private float nextStateTime = 0.0f;///< initiallizes our state time to 0 so that we count down to it.
	
    ///<summary>
    /// sets our scene with the button down of and the button up on.
    /// </summary>
	void Start()
	{
		upSprite.SetActive(true);
		downSprite.SetActive(false);
	}
	
    /// <summary>
    /// gets the click and changes the state
    /// </summary>
	void OnMouseDown()
	{
		if(nextStateTime == 0.0f && currentState == StartButtonController.buttonStates.up)
		{
			nextStateTime = Time.time + downTime;
			upSprite.SetActive(false);
			downSprite.SetActive(true);
			currentState = StartButtonController.buttonStates.down;
		}
	}
	
    /// <summary>
    /// checks the time the button is held down and  if we dilebritely clicked then we start the game
    /// </summary>
	void Update()
	{
		if(nextStateTime > 0.0f)
		{
			if(nextStateTime < Time.time)
			{
                
                // Set the button back to its 'up' state
				nextStateTime = 0.0f;
				upSprite.SetActive(true);
				downSprite.SetActive(false);
				currentState = StartButtonController.buttonStates.up;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    playerState.setGodMode(true);
                }
				StartSound.audio.Stop();
				GameSound.audio.Play();
				// Start the game!
				stateManager.startGame();
                
			}
		}
	}
   
}
