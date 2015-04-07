using UnityEngine;
using System.Collections;
/// <summary>
/// Crushes enemies or the player if they are under the boss. Attached to Boss GameObject
/// </summary>
public class BossCrushTrigger : MonoBehaviour
{
	/// <summary>
	/// Reference to the Boss's BossEventController script
	/// </summary>
	public BossEventController bossController;

	/// <summary>
	/// Sends the message hitByCrusher to the player or an enemy when the crush trigger comes in contact with them
	/// </summary>
	/// <param name="collidedObject">Collided object.</param>
	void OnTriggerEnter2D( Collider2D collidedObject )
	{
		//This comment out was made so that the boss is constantly looking for the player,
		//or enemy and destroys it.

		//if(bossController.currentEvent != BossEventController.bossEvents.fallingToNode)
		//	return;

		if(collidedObject.tag == "Player" || collidedObject.tag == "Enemy")
			collidedObject.SendMessage("hitByCrusher");
	}
}