using UnityEngine;
using System.Collections;
/// <summary>
/// Player bullet controller. Attached to player bullet GameObjects. Launches the bullet and destroys it after some time has passed
/// </summary>
public class PlayerBulletController : MonoBehaviour
{
	/// <summary>
	/// The player object.
	/// </summary>
     public GameObject playerObject = null; // Will be populated automatically when the bullet is created in PlayerStateListener
     public float bulletSpeed = 15.0f;

	/// <summary>
	/// Determines when the bullet should be destroyed
	/// </summary>
	private float selfDestructTimer = 0.0f;
    
	void Update()
	{
		if(selfDestructTimer > 0.0f)
		{
			if(selfDestructTimer < Time.time)
				Destroy(gameObject);
		}
	}

	/// <summary>
	/// Launchs the bullet by adding a horizontal force in the forward direction
	/// </summary>
     public void launchBullet()
     {
          // The local scale of the player object tells us which direction
          // the player is looking. Rather than programming in extra variables to
          // store where the player is looking, just check what already knows
          // that information... the object scale!
          float mainXScale = playerObject.transform.localScale.x;

		  Vector2 bulletForce;
		
          if(mainXScale < 0.0f)
          {
               // Fire bullet left
               bulletForce = new Vector2(bulletSpeed * -1.0f,0.0f);
          }
          else
          {
               // Fire bullet right
               bulletForce = new Vector2(bulletSpeed,0.0f);
          }
		
		rigidbody2D.velocity = bulletForce;
		
		selfDestructTimer = Time.time + 1.0f;
     }
}
