using UnityEngine;
using System.Collections;
/// <summary>
/// Player state controller. Changes the player's current state under certain conditions.
/// </summary>
public class PlayerStateController : MonoBehaviour 
{
	/// <summary>
	/// All player states
	/// </summary>
	public enum playerStates
	{
		idle = 0,
		left,
		right,
		jump,
		landing,
		falling,
		kill,
		resurrect,
		firingWeapon,
		_stateCount
	}
		
	/// <summary>
	/// Delay for changing between states, such as landing from jumping and jumping
	/// </summary>
	public static float[] stateDelayTimer = new float[(int)playerStates._stateCount];		//Delay for changing between states, such as landing from jumping and jumping

	/// <summary>
	/// Delegate handler for changing player state
	/// </summary>
	public delegate void playerStateHandler(PlayerStateController.playerStates newState);	//Delegate handler for changing player state
	/// <summary>
	/// Function for changing player state
	/// </summary>
	public static event playerStateHandler onStateChange;									//Function for changing player state
	
	void LateUpdate () 
	{
		if(!GameStates.gameActive)
			return;

		// Detect the current input of the Horizontal axis, then broadcast a state update for the player as appropriate
		float horizontal = Input.GetAxis("Horizontal");
		if(horizontal != 0.0f)
		{
			if(horizontal < 0.0f)
			{
				if(onStateChange != null)
					onStateChange(PlayerStateController.playerStates.left);
			}
			else
			{
				if(onStateChange != null)
					onStateChange(PlayerStateController.playerStates.right);
			}
		}
		else
		{
			if(onStateChange != null)
				onStateChange(PlayerStateController.playerStates.idle);
		}

		float jump = Input.GetAxis("Jump");
		if (jump > 0.0f) {
			if (onStateChange != null)
				onStateChange (PlayerStateController.playerStates.jump);
		}
		
		float firing = Input.GetAxis("Fire1");
		if(firing > 0.0f)
		{
			if(onStateChange != null)
				onStateChange(PlayerStateController.playerStates.firingWeapon);
		}
	}
}
