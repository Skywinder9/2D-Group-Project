using UnityEngine;
using System.Collections;

public class DeathTriggerScript : MonoBehaviour
{
	void OnTriggerStay2D( Collider2D collidedObject )   //Changed from OnTriggerEnter2D
	{   
		if (collidedObject.gameObject.tag == "Player") {
			collidedObject.SendMessage ("hitDeathTrigger", SendMessageOptions.DontRequireReceiver);
			audio.Play ();
		}
	}    
}
