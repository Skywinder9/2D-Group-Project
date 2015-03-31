using UnityEngine;
using System.Collections;

public class EnemyRespawner : MonoBehaviour
{
	public GameObject spawnEnemy = null;
	float respawnTime = 0.0f;
	
	void OnEnable()
	{
		EnemyControllerScript.enemyDied += scheduleRespawn;
	}
	
	void OnDisable()
	{
		EnemyControllerScript.enemyDied -= scheduleRespawn;
	}
	
	// Note: Even though we don't need the enemyScore, we still need to accept it because the event passes it
	void scheduleRespawn(int enemyScore)
	{
		// Randomly decide if we will respawn or not
		if(Random.Range(0,10) < 5)
			return;

		respawnTime = Time.time + 4.0f;
	}
	
	void Update()
	{
		if(respawnTime > 0.0f)
		{
			if(respawnTime < Time.time)
			{
				respawnTime = 0.0f;
				GameObject newEnemy = Instantiate(spawnEnemy) as GameObject;
				newEnemy.transform.position = transform.position;
                audio.Play();
			}
		}
	}
}
