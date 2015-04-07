using UnityEngine;
using System.Collections;
/// <summary>
/// This class is everything player. Both Josh and I have no idea what half this code does.
/// We just know that it is calling a whole bunch of states to control the player.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerStateListener : MonoBehaviour
{         
	public float playerWalkSpeed = 3f;///< how fast our player can walk
	public float playerJumpForceVertical = 500f;///< the force our player jumps upwards
	public float playerJumpForceHorizontal = 250f;///< the force our player jumps to the side
    public GameObject deathFxParticlePrefab = null;///< our death particle system
	public GameObject playerRespawnPoints = null;///< the different points on the map our player can respawn to
	public GameObject bulletPrefab = null;///< our bullet we fire
    public GameObject resurrectSound;///< our sound that we make when we reurect.

    public GameObject[] lives;///< the array of lives we have
    private int livesLeft;///< counts how many lives we have left

    public Transform bulletSpawnTransform;///< the position our bullet is instantiated at

	private Animator playerAnimator = null;///< the animator that controls our players animation
    private SpriteRenderer spriteRenderer;///< our player sprite
	private PlayerStateController.playerStates previousState = PlayerStateController.playerStates.idle;///< the previous player state
	private PlayerStateController.playerStates currentState = PlayerStateController.playerStates.idle;///< the current player state
    private bool playerHasLanded = true;///< checks to see if the player has landed
    private bool playerDead = false;///< checks to see if the player is dead or not
    private bool invincible = false;///< our temporary invincibillity when we respawn
    private bool God = false;///< a bool to determine whether or not god mode is enabled
    private int invincibleFrames = 80;///< the number of frames we are invincible for after respawning

	/// <summary>
	/// activates OnStateChange
	/// </summary>
	void OnEnable()
    {
		PlayerStateController.onStateChange += onStateChange;
    }
	/// <summary>
	/// turns off OnStateChange
	/// </summary>
    void OnDisable()
    {
		PlayerStateController.onStateChange -= onStateChange;
    }

	/// <summary>
	/// initiallizes our values to their defualt. gets the length of our lives array and establishes livesLefts
	/// </summary>
	void Start()
	{
        livesLeft = lives.Length - 1;
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
       
		// Setup any specific starting values here
		PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.jump] = 1.0f;
		PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.firingWeapon] = 1.0f;
	}

	/// <summary>
	/// passes in a bool from the startButtonController to check to see if god mode should be enabled or not.
	/// </summary>
	/// <param name="set">If set to <c>true</c> set.</param>
    public void setGodMode(bool set)
    {
        God = set;
    }
    /// <summary>
    /// Lates the update.
    /// </summary>
	void LateUpdate()
    {
         onStateCycle();
    }
    
	/// <summary>
	/// checks to see if we are invincable and if we are then it doesn't kill the player.
	/// </summary>
	public void hitDeathTrigger()
	{
        if (!invincible)
        {
            onStateChange(PlayerStateController.playerStates.kill);
        }
	}
	
    ///<summary>
	/// Every cycle of the engine, process the current state.
	/// </summary>
    void onStateCycle()
    {
		// Grab the current localScale of the object so we have 
		// access to it in the following code
		Vector3 localScale = transform.localScale;

		transform.localEulerAngles = Vector3.zero;
		
		switch(currentState)
		{
			case PlayerStateController.playerStates.idle:
			break;
        
			case PlayerStateController.playerStates.left:
				transform.Translate(new Vector3((playerWalkSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
			
				if(localScale.x > 0.0f)
				{
					localScale.x *= -1.0f;
					transform.localScale  = localScale;
				}
			
			break;
             
			case PlayerStateController.playerStates.right:
				transform.Translate(new Vector3(playerWalkSpeed * Time.deltaTime, 0.0f, 0.0f));
			
				if(localScale.x < 0.0f)
				{
					localScale.x *= -1.0f;
					transform.localScale = localScale;              
				}

			break;
             
			case PlayerStateController.playerStates.jump:
			break;
             
			case PlayerStateController.playerStates.landing:
			break;
             
			case PlayerStateController.playerStates.falling:
			break;              

			case PlayerStateController.playerStates.kill:
                if (livesLeft >= 0 && !God)
                {
                    //Call KillPause() once
                    if (!playerDead)
                    {
                        playerDead = true;
                        StartCoroutine(KillPause());
                    }
                }
                //Not in God mode
                if(livesLeft < 0 && !God)
                {
                    Application.LoadLevel(0);
                }
                    //In God mode
                else
                {
                    if (!playerDead)
                    {
                        playerDead = true;
                        StartCoroutine(KillPause());
                    }
                }
                
			break;         

			case PlayerStateController.playerStates.resurrect:
                resurrectSound.audio.Play();
                StartCoroutine(Invincible());
				onStateChange(PlayerStateController.playerStates.idle);
			break;
			
			case PlayerStateController.playerStates.firingWeapon:
			break;
		}
	}
    
    ///<summary> 
	/// onStateChange is called whenever we make a change to the player's state from anywhere within the game's code.
	/// </summary>
	/// <param name="newState">New state.</param>
	public void onStateChange(PlayerStateController.playerStates newState)
	{
		// If the current state and the new state are the same, abort - no need 
		// to change to the state we're already in.
		if(newState == currentState)
			return;
		
		// Verify there are no special conditions that would cause this state to abort
		if(checkIfAbortOnStateCondition(newState))
			return;

         
		// Check if the current state is allowed to transition into this state. If it's not, abort.
		if(!checkForValidStatePair(newState))
			return;
         
		// Having reached here, we now know that this state change is allowed. 
		// So let's perform the necessary actions depending on what the new state is.
		switch(newState)
		{
			case PlayerStateController.playerStates.idle:
				playerAnimator.SetBool("Walking", false);
			break;
         
			case PlayerStateController.playerStates.left:
				playerAnimator.SetBool("Walking", true);
			break;
              
			case PlayerStateController.playerStates.right:
				playerAnimator.SetBool("Walking", true);
			break;
              
			case PlayerStateController.playerStates.jump:                   
				if(playerHasLanded)
				{
					// Use the jumpDirection variable to specify if the player should be jumping left, right or vertical
					float jumpDirection = 0.0f;
					if(currentState == PlayerStateController.playerStates.left)
						jumpDirection = -1.0f;
					else if(currentState == PlayerStateController.playerStates.right)
						jumpDirection = 1.0f;
					else
						jumpDirection = 0.0f;
					             
					// Apply the actual jump force
					rigidbody2D.AddForce(new Vector2(jumpDirection * playerJumpForceHorizontal, playerJumpForceVertical));
									
					playerHasLanded = false;
    				PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.jump] = 0f;
				}
			break;

              
			case PlayerStateController.playerStates.landing:
				playerHasLanded = true;
				PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.jump]= Time.time + 0.1f;
			break;
              
			case PlayerStateController.playerStates.falling:
				PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.jump] = 0.0f;
			break;              
              
			case PlayerStateController.playerStates.kill:
			break;         

			case PlayerStateController.playerStates.resurrect:
                int childIndex = Random.Range(0,playerRespawnPoints.transform.childCount);
                Vector3 respawnPoint = playerRespawnPoints.transform.GetChild(childIndex).position;
				transform.position = respawnPoint;
				transform.rotation = Quaternion.identity;
				rigidbody2D.velocity = Vector2.zero;
			break;
			
			case PlayerStateController.playerStates.firingWeapon:
				// Make the bullet object
				GameObject newBullet = (GameObject)Instantiate(bulletPrefab);
				              
				// Setup the bullet’s starting position
				newBullet.transform.position = bulletSpawnTransform.position;
				
				// Acquire the PlayerBulletController component on the new object so we can specify some data
				PlayerBulletController bullCon = newBullet.GetComponent<PlayerBulletController>();
				
				// Set the player object
				bullCon.playerObject = gameObject;
				              
				// Launch the bullet!
				bullCon.launchBullet();    
				
				//play audio
				audio.Play();
				// With the bullet made, set the state of the player back to the current state
				onStateChange(currentState);
			
				PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.firingWeapon] = Time.time + 0.25f;
			break;
		}
         
		// Store the current state as the previous state
		previousState = currentState;
		
		// And finally, assign the new state to the player object
		currentState = newState;
	}    
    /// <summary>
	/// Compare the desired new state against the current, and see if we are 
	/// allowed to change to the new state. This is a powerful system that ensures 
	/// we only allow the actions to occur that we want to occur.
	/// </summary>
	bool checkForValidStatePair(PlayerStateController.playerStates newState)
	{
		bool returnVal = false;

		// Compare the current against the new desired state.
		switch(currentState)
		{
			case PlayerStateController.playerStates.idle:
				// Any state can take over from idle.
				returnVal = true;
			break;
         
			case PlayerStateController.playerStates.left:
				// Any state can take over from the player moving left.
				returnVal = true;
			break;
              
			case PlayerStateController.playerStates.right:         
				// Any state can take over from the player moving right.
				returnVal = true;              
			break;
              
			case PlayerStateController.playerStates.jump:
				// The only state that can take over from Jump is landing or kill.
				if(
					newState == PlayerStateController.playerStates.landing
					|| newState == PlayerStateController.playerStates.kill
					|| newState == PlayerStateController.playerStates.firingWeapon
				  )
						returnVal = true;
				  else
						returnVal = false;
			break;
              
			case PlayerStateController.playerStates.landing:
				// The only state that can take over from landing is idle, left or right movement.
				if(
					newState == PlayerStateController.playerStates.left
					|| newState == PlayerStateController.playerStates.right
					|| newState == PlayerStateController.playerStates.idle
					|| newState == PlayerStateController.playerStates.firingWeapon
				  )
					returnVal = true;
				else
					returnVal = false;
			break;              
              
			case PlayerStateController.playerStates.falling:    
				// The only states that can take over from falling are landing or kill
				if(
					newState == PlayerStateController.playerStates.landing
					|| newState == PlayerStateController.playerStates.kill
					|| newState == PlayerStateController.playerStates.firingWeapon
				  )
					returnVal = true;
				else
					returnVal = false;
				break;              
              
			case PlayerStateController.playerStates.kill:         
				// The only state that can take over from kill is resurrect
				if(newState == PlayerStateController.playerStates.resurrect)
					returnVal = true;
				else
					returnVal = false;
			break;              
              
			case PlayerStateController.playerStates. resurrect :
				// The only state that can take over from Resurrect is Idle
				if(newState == PlayerStateController.playerStates.idle)
					returnVal = true;
				else
					returnVal = false;                          
			break;
			
			case PlayerStateController.playerStates.firingWeapon:
				returnVal = true;
			break;
		}          
		return returnVal;
	}
	/// <summary>
	/// checkIfAbortOnStateCondition allows us to do additional state verification, to see
	/// if there is any reason this state should not be allowed to begin.
	/// </summary>
	bool checkIfAbortOnStateCondition(PlayerStateController.playerStates newState)
	{
		bool returnVal = false;
		
		switch(newState)
		{
			case PlayerStateController.playerStates.idle:
			break;
			
			case PlayerStateController.playerStates.left:
			break;
			
			case PlayerStateController.playerStates.right:
			break;
			
			case PlayerStateController.playerStates.jump:
				float nextAllowedJumpTime = PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.jump ];
				
				if(nextAllowedJumpTime == 0.0f || nextAllowedJumpTime > Time.time)
					returnVal = true;
			break;
			
			case PlayerStateController.playerStates.landing:
			break;
			
			case PlayerStateController.playerStates.falling:
			break;
			
			case PlayerStateController.playerStates.kill:
			break;
			
			case PlayerStateController.playerStates.resurrect:
			break;
			
			case PlayerStateController.playerStates.firingWeapon:		
				if(PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.firingWeapon] > Time.time)
					returnVal = true;
			
			break;
		}
		
		// Value of true means 'Abort'. Value of false means 'Continue'.
		return returnVal;
	}
	/// <summary>
	/// If hit by the crusher is called we destroy the player.
	/// </summary>
	public void hitByCrusher()
	{
		onStateChange(PlayerStateController.playerStates.kill);
	}

	/// <summary>
	/// pauses our respawn while we play our death FX
	/// </summary>
	/// <returns>The pause.</returns>
    IEnumerator KillPause()
    {
        if (!God)
        {
            Destroy(lives[livesLeft]);
            livesLeft -= 1;
        }
        DeathParticles();
        spriteRenderer.enabled = false;
        rigidbody2D.isKinematic = true;
        yield return new WaitForSeconds(2f);
        onStateChange(PlayerStateController.playerStates.resurrect);
        spriteRenderer.enabled = true;
        rigidbody2D.isKinematic = false;
        playerDead = false;
    }

    /// <summary>
    /// checks to see if we are invincable or not and returns the bool
    /// </summary>
    /// <returns></returns>
    public bool isInvincible()
    {
        return invincible;
    }

    /// <summary>
    /// Makes our player flicker while he is invincible. Then once the frames are over, the invincibility ends.
    /// </summary>
    /// <returns></returns>
    IEnumerator Invincible()
    {
        invincible = true;
        for (int i = 0; i < invincibleFrames; i++)
        {
            if (i % 4 == 0 || i % 4 == 1)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }
            yield return 0;
        }
        invincible = false;
    }

    /// <summary>
    /// generates the deathParticleSystem where we die
    /// </summary>
    void DeathParticles()
    {
        // Create the particle emitter object.
        GameObject deathFxParticle = (GameObject)Instantiate(deathFxParticlePrefab);

        // Get the enemy position
        Vector3 enemyPos = transform.position;

        // Create a new vector that is in front of the enemy
        Vector3 particlePosition = new Vector3(enemyPos.x, enemyPos.y, enemyPos.z + 1.0f);

        // Reposition the particle emitter at this new position
        deathFxParticle.transform.position = particlePosition;

        Destroy(deathFxParticle, 0.1f);
    }
}
