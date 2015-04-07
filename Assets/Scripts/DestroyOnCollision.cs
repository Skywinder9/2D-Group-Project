using UnityEngine;
using System.Collections;
/// <summary>
/// DESTROYS EVERYTHING IN IT'S PATH!!!!!!!!
/// </summary>
public class DestroyOnCollision : MonoBehaviour
{
	/// <summary>
	/// On trigger enter, DEATH!!!
	/// </summary>
	/// <param name="hitObj"></param>
    void OnTriggerEnter2D(Collider2D hitObj)
	{
		DestroyObject(gameObject);
	}
} 
