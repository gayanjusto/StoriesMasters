using UnityEngine;

public class NpcRotationManager : MonoBehaviour {

	// Lock the NPC object so it won't rotate
    //It's visual direction must be changed by sprites instead
	void Update () {
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
	}
}
