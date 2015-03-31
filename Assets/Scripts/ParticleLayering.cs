using UnityEngine;
using System.Collections;

public class ParticleLayering : MonoBehaviour
{
	public string sortLayerString = "";

	void Start () 
	{
		particleSystem.renderer.sortingLayerName = sortLayerString;
	}
}
