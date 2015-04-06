using UnityEngine;
using System.Collections;
/// <summary>
/// a class for keeping score
/// </summary>
public class ScoreWatcher : MonoBehaviour
{
	public int currScore = 0;///< our current score 
	private TextMesh scoreMesh = null;///< a place holder for our text game object
	
    ///<summary>
    /// sets up our scene for updating the text when we score
    /// </summary>
	void Start()
	{
		scoreMesh = gameObject.GetComponent<TextMesh>();
		scoreMesh.text = "0";
	}
	
    /// <summary>
    /// calls the event when enemies die or the boss dies and calls addScore()
    /// </summary>
	void OnEnable()
	{
		EnemyControllerScript.enemyDied += addScore;
		BossEventController.bossDied += addScore;
	}
	
    /// <summary>
    /// turns off the addScore() call
    /// </summary>
	void OnDisable()
	{
		EnemyControllerScript.enemyDied -= addScore;
		BossEventController.bossDied -= addScore;
	}
	
    /// <summary>
    /// adds the score value of the destroyed enemy to the current score
    /// and updates the score on-screen
    /// </summary>
    /// <param name="scoreToAdd"></param>
	void addScore(int scoreToAdd)
	{
		currScore += scoreToAdd;
		scoreMesh.text = currScore.ToString();
	}
}