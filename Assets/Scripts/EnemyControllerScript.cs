using UnityEngine;
using System.Collections;

/// <summary>
/// Changes the states of the enemies when certain conditions are met
/// </summary>

public class EnemyControllerScript : MonoBehaviour
{
	/// <summary>
	///  States to allow objects to know when an enemy dies
	/// </summary>
	public delegate void enemyEventHandler(int scoreMod);
	/// <summary>
	/// Occurs when the enemy has died
	/// </summary>
		public static event enemyEventHandler enemyDied;
	/// <summary>
	/// Reference to this object's TakeDamageFromPlayerBullet script
	/// </summary>
	public TakeDamageFromPlayerBullet bulletColliderListener = null;
	public float walkingSpeed = 0.45f;
	public GameObject deathFxParticlePrefab = null;

	private bool walkingLeft = true;

	void OnEnable()
	{
		/// <summary>
		/// Subscribe to events from the bullet collider 
		/// </summary>
		bulletColliderListener.hitByBullet += hitByPlayerBullet; 
	}
	
	void OnDisable()
	{
		/// <summary>
		/// Unsubscribe from events
		/// </summary>
		bulletColliderListener.hitByBullet -= hitByPlayerBullet;
	}

	void Start()
	{
		// Randomly default the enemy’s direction
		walkingLeft = (Random.Range(0,2) == 1);
		updateVisualWalkOrientation();

		// Find the round watcher object
		GameObject roundWatcherObject = GameObject.FindGameObjectWithTag("RoundWatcher");
		
		if (roundWatcherObject != null)
		{
			RoundWatcher roundWatcherComponent = roundWatcherObject.GetComponent<RoundWatcher>();
			
			// Increase the enemy speed for each round.
			walkingSpeed = walkingSpeed * roundWatcherComponent.currRound;
		}
	}
	
	void Update()
	{
		// Translate the enemy's position based on the direction that
		// they are currently moving.
		if(walkingLeft)
		{
			transform.Translate(new Vector3(walkingSpeed * Time.deltaTime, 0.0f, 0.0f));
		}
		else
		{
			transform.Translate(new Vector3((walkingSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
		}
	}

	/// <summary>
	/// Have the enemy move in the opposite direction
	/// Occurs when an enemy comes to the edge of a platform or touches another enemy
	/// </summary>
	public void switchDirections()
	{
		// Swap the direction to be the opposite of whatever it 
		// currently is
		walkingLeft = !walkingLeft;
		
		// Update the orientation of the Enemy's material to match the
		// new walking direction
		updateVisualWalkOrientation();
	}

	/// <summary>
	/// Reverse the x-scale of the transform so the enemy appears to be moving in its current direction
	/// </summary>
	void updateVisualWalkOrientation()
	{
		Vector3 localScale = transform.localScale;
		if(walkingLeft)
		{
			if(localScale.x > 0.0f)
			{
				localScale.x = localScale.x * -1.0f;
				transform.localScale  = localScale;
			}
		}
		else
		{
			if(localScale.x < 0.0f)
			{
				localScale.x = localScale.x * -1.0f;
				transform.localScale  = localScale;              
			}
		} 
	}

	/// <summary>
	/// Called when the enemy is hit by the player's bullet
	/// Calls the enemyDied event which adds 25 to the score
	/// </summary>
	public void hitByPlayerBullet()
	{
		// Call the EnemyDied event and give it a score of 25.
		if(enemyDied != null)
			enemyDied(25);

		handleEnemyDeath();
	}

	/// <summary>
	/// Called when the enemy is crushed by the boss
	/// </summary>
	public void hitByCrusher()
	{
		// Enemy was crushed, but don't give any points to the player.
		if(enemyDied != null)
			enemyDied(0);

		handleEnemyDeath();
	}

	/// <summary>
	/// Called when the enemy is destroyed
	/// Instantiates a particle system, sets its position to the enemy's position, then destroys the enemy after 0.1 seconds
	/// </summary>
	void handleEnemyDeath()
	{
		// Create the particle emitter object.
		GameObject deathFxParticle = (GameObject)Instantiate(deathFxParticlePrefab);
		
		// Get the enemy position
		Vector3 enemyPos = transform.position;
		
		// Create a new vector that is in front of the enemy
		Vector3 particlePosition = new Vector3(enemyPos.x,enemyPos.y,enemyPos.z + 1.0f);
		
		// Reposition the particle emitter at this new position
		deathFxParticle.transform.position = particlePosition;

		Destroy(gameObject,0.1f);
	}
}
