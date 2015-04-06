using UnityEngine;
using System.Collections;
/// <summary>
/// this class counts the rounds we have gone through
/// </summary>
[RequireComponent(typeof(TextMesh))]
public class RoundWatcher : MonoBehaviour
{
	public int currRound = 1;///< our round counter
	private TextMesh roundDisplayMesh = null;///< our text that displays which round we are on.

    ///<summary>
    /// initializes our round counter
    /// </summary>
	void Start ()
	{		
		roundDisplayMesh = gameObject.GetComponent<TextMesh>();    
         
		currRound = 1;
		roundDisplayMesh.text = "Round: " + currRound.ToString();
     }
    /// <summary>
    /// when the boss dies we increment the round
    /// </summary>
	void OnEnable()
	{
		BossEventController.bossDied += increaseRound;
	}
    /// <summary>
    /// this is to make sure we don't call it twice
    /// </summary>
	void OnDisable()
	{
		BossEventController.bossDied -= increaseRound;         
	}

    /// <summary>
    /// updates our round counter on screen
    /// </summary>
    /// <param name="ignore"></param>
	void increaseRound(int ignore)
	{
		currRound += 1;
		roundDisplayMesh.text = "Round: " + currRound.ToString();    
	}
}

