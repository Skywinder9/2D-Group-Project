using UnityEngine;
using System.Collections;
/// <summary>
/// A specific script that only tells the player to die.
/// </summary>
public class DeathTriggerScript : MonoBehaviour
{
    /// <summary>
    /// Checks to see if the attached object has collided with the player and tells the player to die while laughing about it.
    /// </summary>
    /// <param name="collidedObject"></param>
	void OnTriggerStay2D( Collider2D collidedObject )   //Changed from OnTriggerEnter2D
	{   
		if (collidedObject.gameObject.tag == "Player") {
			collidedObject.SendMessage ("hitDeathTrigger", SendMessageOptions.DontRequireReceiver);
			audio.Play ();
		}
	}    
}
