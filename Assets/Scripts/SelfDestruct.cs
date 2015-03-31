using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{
	public float fuseLength = 0.1f;
	private float destructTime = 0.0f;
	
	void Start()
	{
		destructTime = Time.time + fuseLength;
	}
	
	void Update()
	{
		if(destructTime < Time.time)
			Destroy(gameObject);
	}
}
