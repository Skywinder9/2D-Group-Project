using UnityEngine;
using System.Collections;
/// <summary>
/// Sorts our particle system to where we want it. In most cases, we bring it to the front of everything.
/// </summary>
public class ParticleLayering : MonoBehaviour
{
	public string sortLayerString = "";///< a simple string object

    /// <summary>
    /// This just puts our particle system to wherever we want it in unity.
    /// </summary>
	void Start () 
	{
		particleSystem.renderer.sortingLayerName = sortLayerString;
	}
}
