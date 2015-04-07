using UnityEngine;
using System.Collections;
/// <summary>
/// This controls our ground, our clouds, everything that would be moving to make the game feel more realistic.
/// </summary>
public class ParallaxController : MonoBehaviour 
{
	public GameObject[] clouds;///< our array for the clouds
    public GameObject[] nearHills;///< our array for the closer hills
    public GameObject[] farHills;///< our array for the further away hills
    public GameObject[] lava;///< our array for the ground which I had no idea was supposed to be lava until just now.

	public float cloudLayerSpeedModifier;///< The speed our clouds move
    public float nearHillLayerSpeedModifier;///< The speed our close hills move
    public float farHillLayerSpeedModifier;///< The speed our far hills move
    public float lavalLayerSpeedModifier;///< The speed our lava moves

	public Camera myCamera;///< our camera came object

	private Vector3 lastCamPos;///< a placeholder for our cameras last position to make a slightly delayed effect.

    ///<summary>
    /// initilizes our camera postion variable to be where the camera starts
    /// </summary>
	void Start()
	{
		lastCamPos = myCamera.transform.position;
	}

    /// <summary>
    /// updates every frame the camera position, and applys it to the four parallax arrays
    /// </summary>
	void Update()
	{
		Vector3 currCamPos = myCamera.transform.position;
		float xPosDiff = lastCamPos.x - currCamPos.x;

		adjustParallaxPositionsForArray(clouds, cloudLayerSpeedModifier, xPosDiff);
		adjustParallaxPositionsForArray(nearHills, nearHillLayerSpeedModifier, xPosDiff);
		adjustParallaxPositionsForArray(farHills, farHillLayerSpeedModifier, xPosDiff);
		adjustParallaxPositionsForArray(lava, lavalLayerSpeedModifier, xPosDiff);

		lastCamPos = myCamera.transform.position;
	}

    /// <summary>
    /// creates the drag effect we see in game.
    /// </summary>
    /// <param name="layerArray"></param>
    /// <param name="layerSpeedModifier"></param>
    /// <param name="xPosDiff"></param>
	void adjustParallaxPositionsForArray(GameObject[] layerArray, float layerSpeedModifier, float xPosDiff)
	{
		for(int i = 0; i < layerArray.Length; i++)
		{
			Vector3 objPos = layerArray[i].transform.position;
			objPos.x += xPosDiff * layerSpeedModifier;
			layerArray[i].transform.position = objPos;
		}
	}
}
