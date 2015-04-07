using UnityEngine;
using System.Collections;

/// <summary>
/// Counts the number of enemies currently on a platform. Attached to Platform GameObjects
/// </summary>

public class EnemyCounter : MonoBehaviour {
	/// <summary>
	/// Vortex above the given platform
	/// </summary>
	public GameObject vortex;
	/// <summary>
	/// Reference to Vortex's EnemyRespawner script
	/// </summary>
    private EnemyRespawner enemyRespawner;

    void Start()
    {
        enemyRespawner = vortex.GetComponent<EnemyRespawner>();
    }


	void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Enemy")
        {
            enemyRespawner.AddEnemy(1);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyRespawner.AddEnemy(-1);
        }
    }
}
