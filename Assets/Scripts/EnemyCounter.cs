using UnityEngine;
using System.Collections;

public class EnemyCounter : MonoBehaviour {
	public GameObject vortex;
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

	//your problem here is that it is subtracting from the counter when the platform is destroyed.
    void OnDestroy()
    {
        enemyRespawner.AddEnemy(-1);
    }
}
