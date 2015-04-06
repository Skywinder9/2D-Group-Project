using UnityEngine;
using System.Collections;
/// <summary>
/// a class for our bullet to determine the lifetime of our bullet 
/// so that we don't have bullet clones littering our scene
/// </summary>
public class SelfDestruct : MonoBehaviour
{
	public float fuseLength = 0.1f;///< the amount of time our bullet is alive
	private float destructTime = 0.0f;///< the destruction counter of our bullet
	///<summary>
    /// sets our bullet life to be 0.1 second after we fire it.
    /// </summary>
	void Start()
	{
		destructTime = Time.time + fuseLength;
	}
	
    /// <summary>
    /// checks if the time is up and then destroys the game object.
    /// </summary>
	void Update()
	{
		if(destructTime < Time.time)
			Destroy(gameObject);
	}
}
