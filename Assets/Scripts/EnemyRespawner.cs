using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to Vortex GameObjects
/// </summary>

public class EnemyRespawner : MonoBehaviour
{
	/// <summary>
	/// Prefab for enemy GameObject
	/// </summary>
	public GameObject spawnEnemy = null;
	/// <summary>
	/// Counter for the current number of enemies
	/// </summary>
    private int enemyCount;
	/// <summary>
	/// The maximum number of enemies allowed on each platform
	/// </summary>
    private int maxEnemies = 2;
	/// <summary>
	/// Determines when to respawn enemies
	/// </summary>
	float respawnTime = 0.0f;

    void Start(){
        enemyCount = 0;
    }
	
	void OnEnable()
	{
		EnemyControllerScript.enemyDied += scheduleRespawn;
	}
	
	void OnDisable()
	{
		EnemyControllerScript.enemyDied -= scheduleRespawn;
	}
	
	// Note: Even though we don't need the enemyScore, we still need to accept it because the event passes it
	/// <summary>
	/// Sets the respawn time for the vortex. Delegated to EnemyControllerScript.enemyDied
	/// </summary>
	void scheduleRespawn(int enemyScore)
	{
		// Randomly decide if we will respawn or not
		if(Random.Range(0,10) < 5)
			return;

		respawnTime = Time.time + 4.0f;
	}

	/// <summary>
	/// Increments when an enemy touches the vortex's platform, decrements when an enemy is removed from the platform
	/// </summary>
    public void AddEnemy(int add)
    {
        enemyCount += add;
    }
	
	void Update()
	{
		if(respawnTime > 0.0f)
		{
			if(respawnTime < Time.time)
			{
				respawnTime = 0.0f;
                if (enemyCount < maxEnemies)
                {
                    GameObject newEnemy = Instantiate(spawnEnemy) as GameObject;
                    newEnemy.transform.position = transform.position;
                }
                audio.Play();
			}
		}
	}
}
