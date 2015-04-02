using UnityEngine;
using System.Collections;

public class DeathTriggerScript : MonoBehaviour
{
	void OnTriggerEnter2D( Collider2D collidedObject )
	{   
		if (collidedObject.gameObject.tag == "Player") {
			collidedObject.SendMessage ("hitDeathTrigger", SendMessageOptions.DontRequireReceiver);
			audio.Play ();
		}
	}    
}
