using UnityEngine;
using System.Collections;
/// <summary>
/// this class is made simply to apply to enemies and boss to apply damage when shot.
/// </summary>
public class TakeDamageFromPlayerBullet : MonoBehaviour
{
	public delegate void hitByPlayerBullet();///< a public function
	public event hitByPlayerBullet hitByBullet;///< an event that calls the above function
	
    /// <summary>
    /// This just checks if the player has shot the object and calls the function to apply damage to the object.
    /// </summary>
    /// <param name="collidedObject"></param>
	void OnTriggerEnter2D( Collider2D collidedObject )
	{   
		if(collidedObject.tag == "Player Bullet")
		{
			if(hitByBullet != null)
				hitByBullet();
		}
	}    
}