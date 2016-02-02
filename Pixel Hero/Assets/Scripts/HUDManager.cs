using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

    public GameObject staminaBar;
    public GameObject recent;

    float staminaBarSize;

	// Use this for initialization
	void Start ()
    {
        staminaBarSize = staminaBar.transform.localScale.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        staminaBar.transform.localScale = new Vector3((PlayerMovements.stamina/100) * staminaBarSize, staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);

        if (PlayerMovements.staminaTimer <= 0.25)
            recent.transform.localScale = staminaBar.transform.localScale;
	}
}
