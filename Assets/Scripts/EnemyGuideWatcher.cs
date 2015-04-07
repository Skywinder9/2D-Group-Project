using UnityEngine;
using System.Collections;
/// <summary>
/// The script that checks to see if the enemy has hit the edge of the platform or another enemy and if it has, they change direction.
/// </summary>
public class EnemyGuideWatcher : MonoBehaviour
{
	public EnemyControllerScript enemyObject = null;///< the enemy this script is attached to
	public bool Touched = false;///< a bool so that our two enemies do not get caught in each other.

    /// <summary>
    /// checks to see if the enemy has reached the edge of the platform and if it has then it switches directions.
    /// </summary>
    /// <param name="otherObj"></param>
	void OnTriggerExit2D( Collider2D otherObj )
	{
		// If this trigger just left a Platform object, then the enemy 
		// is about to walk off the platform. Tell the enemy that they need to switch directions!


		if (otherObj.tag == "Platform") 
		{
			enemyObject.switchDirections ();
			Touched = false;
		}

	}

    /// <summary>
    /// checks to see if the enemy has touched another enemy and if it has, it switches directions.
    /// </summary>
    /// <param name="otherObj"></param>
	void OnTriggerEnter2D( Collider2D otherObj )
	{
		// If this trigger just left a Platform object, then the enemy 
		// is about to walk off the platform. Tell the enemy that they need to switch directions!
		
		
		if (otherObj.tag == "Enemy" && !Touched) 
		{
			enemyObject.switchDirections ();
			Touched = true;
		}
		
	}
}