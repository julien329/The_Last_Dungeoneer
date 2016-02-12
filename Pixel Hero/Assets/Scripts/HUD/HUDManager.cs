using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour {

    public GameObject staminaBar;
    public GameObject healthBar;
    public GameObject recent;
    public GameObject swordAbilities;

    private float staminaBarSize;
    private float healthBarSize;
    private bool swordEquiped;
    private Text[] swordCooldownsText;
    private Image[] swordAbilitiesImage;

    // Use this for initialization
    void Start ()
    {
        // Save original stamina bar x size
        staminaBarSize = staminaBar.transform.localScale.x;
        healthBarSize = healthBar.transform.localScale.x;

        swordCooldownsText = swordAbilities.GetComponentsInChildren<Text>();
        swordAbilitiesImage = swordAbilities.GetComponentsInChildren<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Change the x size of the stamina bar in ratio with missing stamina
        staminaBar.transform.localScale = new Vector3((PlayerMovements.stamina/100) * staminaBarSize, staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);
        healthBar.transform.localScale = new Vector3(healthBarSize, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

        // Just before the stamina cooldown ends, remove the rencently used stamina secondary bar (hide behind stamina bar)
        if (PlayerMovements.staminaTimer <= 0.25)
            recent.transform.localScale = staminaBar.transform.localScale;

        AbilitiesUpdate();
	}

    void AbilitiesUpdate()
    {
        if(swordEquiped)
        {
            if (PlayerMovements.spinTimer > 0)
            {
                swordCooldownsText[0].text = PlayerMovements.spinTimer.ToString();
                swordAbilitiesImage[0].color = new Color(0.2f, 0.2f, 0.2f);
            }

            else
            {
                swordCooldownsText[0].text = "";
                swordAbilitiesImage[0].color = new Color(1f, 1f, 1f);
            }
        }
    }

    public void ActivateSwordAbilities()
    {
        swordAbilities.SetActive(true);
        swordEquiped = true;

        foreach(Text text in swordCooldownsText)
            text.text = "";
    }
}
