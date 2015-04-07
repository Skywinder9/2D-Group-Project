using UnityEngine;
using System.Collections;
/// <summary>
/// this class is a simple change state script that will destroy the player if he hits the death trigger,
/// or will change his state if he jumps from platform to platform
/// </summary>
public class PlayerColliderListener : MonoBehaviour
{
	public PlayerStateListener targetStateListener = null;///< our player state listener
    
    ///<summary>
    /// Checks to see if the player has hit a platform or a death trigger. If it has, it changes to the respective state
    /// </summary>
	void OnTriggerEnter2D( Collider2D collidedObject )
    {
		switch(collidedObject.tag)
        {
			case "Platform":
				// When the player lands on a platform, toggle the Landing state.
				targetStateListener.onStateChange(PlayerStateController.playerStates.landing);
			break;

			case "DeathTrigger":
				// Player hit the death trigger - kill 'em!
                if (!targetStateListener.isInvincible())
                {
                    targetStateListener.onStateChange(PlayerStateController.playerStates.kill);
                }
			break;
		}
	}
	
    /// <summary>
    /// checks to see if we are leaving the platform and if we are then it changes our state to falling so we can't jump.
    /// </summary>
    /// <param name="collidedObject"></param>
	void OnTriggerExit2D( Collider2D collidedObject)
	{
		switch(collidedObject.tag)
		{
			case "Platform":
				// When the player leaves a platform, set the state as falling. If the player actually
				//     is not falling, this will get verified by the PlayerStateListener.
				targetStateListener.onStateChange(PlayerStateController.playerStates.falling);
			break;
		}         
	}

}
