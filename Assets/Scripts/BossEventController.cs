using UnityEngine;
using System.Collections;
using System.Collections.Generic; // This is needed to support list objects

/// <summary>
/// the controller for the boss states
/// </summary>
public class BossEventController : MonoBehaviour
{
	public delegate void bossEventHandler(int scoreMod);///< an event that gives points for killing the boss.
		public static event bossEventHandler bossDied;

	public delegate void bossAttack();///< an event for the boss dropping to the node
		public static event bossAttack crushPlayer;
	
	public GameObject inActiveNode = null;///< the node the boss goes to 
	public GameObject dropToStartNode = null;///< a starting node
	public GameObject dropFXSpawnPoint = null;///< the point where our drop effects spawn
	public List<GameObject> dropNodeList = new List<GameObject>();///< the list of points where our boss can possibly drop
	public GameObject bossDeathFX = null;///< our death particle system
	public GameObject bossDropFX = null;///< our drop particle system
    public GameObject warningAlarm = null;///< our warning alarm
    public GameObject gameMusic = null;///< our game music 
	public TakeDamageFromPlayerBullet bulletColliderListener = null;///< a refrence to our bullet collider script

	private PlayerStateController playerState;///< a refrence to our player state script

	public float moveSpeed = 0.1f;///< how fast the boss moves
	public float eventWaitDelay = 3f; ///< Amount of time to wait between each event
	
	public int enemiesToStartBattle = 10;///< number of enemies we need to kill to bring out the boss.
	
    ///<summary>
    /// our boss states
    /// </summary>
	public enum bossEvents
	{
		inactive = 0,
		fallingToNode,
		waitingToJump,
		waitingToFall,
		jumpingOffPlatform
	}


    public bossEvents currentEvent = bossEvents.inactive;///< Current event to cycle on each Update() pass


    private GameObject targetNode = null;///< The node object that the boss will be falling towards.


    private float timeForNextEvent = 0.0f;///< Amount of time to wait until jumping or falling again.


    private Vector3 targetPosition = Vector3.zero;///< Target position used for when jumping off a platform.


    public int health = 20;///< Current health of the boss 


    private int startHealth = 20;///< Health to start the boss at whenever the battle begins


    private bool isDead = false;///< Used to determine if the boss has been defeated


    private int enemiesLeftToKill = 0;///< How many enemies left to kill before the boss is spawned
	
	///<summary>
    /// Use this for initialization
    /// </summary> 
	void OnEnable()
	{
        bulletColliderListener.hitByBullet += hitByPlayerBullet;  
		EnemyControllerScript.enemyDied += enemyDied;
	}

	void OnDisable()
	{
		bulletColliderListener.hitByBullet -= hitByPlayerBullet;    
		EnemyControllerScript.enemyDied -= enemyDied;
	}
         
	void Start()
	{
		transform.position = inActiveNode.transform.position;
		enemiesLeftToKill = enemiesToStartBattle;
	}
    /// <summary>
    /// switches events
    /// </summary>
	void Update()
	{
		switch(currentEvent)
		{
			case bossEvents.inactive:
				// Not doing anything, so nothing to do.
			break;
              
			case bossEvents.fallingToNode:
				if(transform.position.y > targetNode.transform.position.y)
				{
					// Movespeed here is negtive, so the object moves downwards
					transform.Translate(new Vector3(0f, -moveSpeed * Time.deltaTime, 0f));
					if(transform.position.y < targetNode.transform.position.y)
					{
						Vector3 targetPos = targetNode.transform.position;
						transform.position = targetPos;
					}
				}
				else
				{
					// Create the 'Hit Ground' smoke FX
					createDropFX();
                   
					timeForNextEvent = 0.0f;
					currentEvent = bossEvents.waitingToJump;
				}
               break;
              
				case bossEvents.waitingToFall:
                    // Boss is waiting to fall to another node
                    if(timeForNextEvent == 0.0f)
                    {
                         timeForNextEvent = Time.time + eventWaitDelay;
                    }
					else if(timeForNextEvent < Time.time)
                    {
                         // Need to find a new node!
                         targetNode = dropNodeList[ Random.Range(0,dropNodeList.Count) ];
                   
                         // Set the boss position to the sky position of this node
                         transform.position = getSkyPositionOfNode(targetNode);
                   
                         // Set the event state
                         currentEvent = bossEvents.fallingToNode;
                         timeForNextEvent = 0.0f;
                         warningAlarm.audio.Play();
                    }
               break;
              
               case bossEvents.waitingToJump:
                    // Boss is on a platform and is just waiting to jump off of it
                    if(timeForNextEvent == 0.0f)
                    {
                         timeForNextEvent = Time.time + eventWaitDelay;
                    }
					else if(timeForNextEvent < Time.time)
                    {                   
                         // Build the target position based on the current node
                         targetPosition = getSkyPositionOfNode(targetNode);
                   
                         // Set our event state
                         currentEvent = bossEvents.jumpingOffPlatform;
                         timeForNextEvent = 0.0f;
                   
                         // Also set the target node to null so we know to find a random one when it's time to fall to one again
                         targetNode = null;
                    }
               break;
              
               case bossEvents.jumpingOffPlatform:
                    if(transform.position.y < targetPosition.y)
                    {
						// Movespeed is positive here, causing the object to move upwards
						transform.Translate(new Vector3(0f, moveSpeed * Time.deltaTime, 0f));

                         if(transform.position.y > targetPosition.y)                              
							transform.position = targetPosition;                    
					}
                    else
                    {
                         timeForNextEvent = 0.0f;
                         currentEvent = bossEvents.waitingToFall;
                    }              
               break;
          }
     }
     /// <summary>
     /// this method starts the boss battle after we've killed enough enemies
     /// </summary>
     public void beginBossBattle()
     {
         //Play music
         gameMusic.audio.Pause();
         audio.Play();

          // Set the first falling node and have the boss fall towards it
          targetNode = dropToStartNode;
          currentEvent = bossEvents.fallingToNode;
          warningAlarm.audio.Play();

          // Reset various control variables used to track the boss battle
          timeForNextEvent = 0.0f;
          health = startHealth;
          isDead = false;
     }
    /// <summary>
    /// just getting the position from where we drop to
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
     Vector3 getSkyPositionOfNode(GameObject node)
     {
          Vector3 targetPosition = targetNode.transform.position;
          targetPosition.y += 9f;    
         
          return targetPosition;
     }
     /// <summary>
     /// this takes the damage from the bullet every time it is hit.
     /// </summary>
     void hitByPlayerBullet()
     {
          health -= 1;         

          // If the boss is out of health – kill ‘em!
          if(health <= 0)
               killBoss();
     }
     /// <summary>
     /// just generates the drop effects
     /// </summary>
     void createDropFX()
     {
          GameObject dropFxParticle = (GameObject)Instantiate(bossDropFX);
          dropFxParticle.transform.position = dropFXSpawnPoint.transform.position;
     }
     /// <summary>
     /// controls the death particles as well as stopping the boss battle song and resuming the regular music
     /// </summary>
     void killBoss()
     {
		if(isDead)
			return;
         
		isDead = true;
         
		GameObject deathFxParticle = (GameObject)Instantiate(bossDeathFX);
		audio.Stop ();
		gameMusic.audio.Play ();

		// Reposition the particle emitter at the same position as dropFXSpawnPoint
		deathFxParticle.transform.position = dropFXSpawnPoint.transform.position;
              
		// Call the bossDied event and give it a score of 1000
		if(bossDied != null)
			bossDied(1000);
         
		transform.position = inActiveNode.transform.position;
         
		currentEvent = BossEventController.bossEvents.inactive;
		timeForNextEvent = 0.0f;
		enemiesLeftToKill = enemiesToStartBattle;
     }
    /// <summary>
    /// checks to see if we've killed enough enemies to bring out the boss
    /// </summary>
    /// <param name="enemyScore"></param>
	void enemyDied(int enemyScore)
	{
		if(currentEvent == bossEvents.inactive)
		{
			enemiesLeftToKill -= 1;
			Debug.Log("--- Enemies left to start boss battle: " + enemiesLeftToKill);
			if(enemiesLeftToKill <= 0)
				// internal dialogue: "OH MY GOD THEY KILLED KENNY!"
                beginBossBattle();
		}
	}
    /// <summary>
    /// tells the player to die.
    /// </summary>
	public void playerHitByCrusher()
	{
		//if(currentEvent == bossEvents.fallingToNode)
		//{
			if(crushPlayer != null)
				crushPlayer();
		//}
	}
}
