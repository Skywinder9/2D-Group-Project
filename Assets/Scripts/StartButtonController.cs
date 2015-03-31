using UnityEngine;
using System.Collections;

public class StartButtonController : MonoBehaviour
{
	public GameObject upSprite;
	public GameObject downSprite;
	public float downTime = 0.1f;
	public GameStates stateManager = null;
    public PlayerStateListener playerState;
	
	private enum buttonStates
	{
		up = 0,
		down
	}
	
	private buttonStates currentState = buttonStates.up;
	private float nextStateTime = 0.0f;
	
	void Start()
	{
		upSprite.SetActive(true);
		downSprite.SetActive(false);
	}
	
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
				// Start the game!
				stateManager.startGame();
                
			}
		}
	}
   
}
