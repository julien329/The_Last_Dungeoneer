using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour {

    public GameObject staminaBar;
    public GameObject recent;

    private float staminaBarSize;

    // Use this for initialization
    void Start ()
    {
        // Save original stamina bar x size
        staminaBarSize = staminaBar.transform.localScale.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Change the x size of the stamina bar in ratio with missing stamina
        staminaBar.transform.localScale = new Vector3((PlayerMovements.stamina/100) * staminaBarSize, staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);

        // Just before the stamina cooldown ends, remove the rencently used stamina secondary bar (hide behind stamina bar)
        if (PlayerMovements.staminaTimer <= 0.25)
            recent.transform.localScale = staminaBar.transform.localScale;
	}
}
