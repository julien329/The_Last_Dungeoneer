using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour {

    public RectTransform staminaBar;
    public RectTransform healthBar;
    public RectTransform staminaRecent;
    public GameObject swordAbilities;

    private float staminaBarSize;
    private float healthBarSize;
    private bool swordEquiped;
    private Text[] swordCooldownsText;
    private Image[] swordAbilitiesImage;

    // Use this for initialization
    void Awake ()
    {
        // Save original stamina bar x size
        staminaBarSize = staminaBar.localScale.x;
        healthBarSize = healthBar.localScale.x;

        // Get components for the swordAbilies object in the HUD
        swordCooldownsText = swordAbilities.GetComponentsInChildren<Text>();
        swordAbilitiesImage = swordAbilities.GetComponentsInChildren<Image>();
    }

    void Start()
    {
        StartCoroutine("StaminaUpdate");
        StartCoroutine("HealthUpdate");
    }

    IEnumerator StaminaUpdate()
    {
        while (true)
        {
            // Change the x size of the stamina bar in ratio with missing stamina
            staminaBar.localScale = new Vector3((PlayerMovements.stamina / 100f) * staminaBarSize, staminaBar.localScale.y, staminaBar.localScale.z);
            // Just before the stamina cooldown ends, remove the rencently used stamina secondary bar (hide behind stamina bar)
            if (PlayerMovements.staminaTimer <= 0.25)
                staminaRecent.localScale = staminaBar.localScale;
            // Dont update if stamina is full
            yield return new WaitUntil(() => PlayerMovements.stamina < 100);
        }
    }

    IEnumerator HealthUpdate()
    {
        while (true)
        {
            healthBar.localScale = new Vector3(healthBarSize, healthBar.localScale.y, healthBar.localScale.z);
            // Dont update if health is full
            yield return new WaitUntil(() => false);
        }
    }

    IEnumerator SwordAbilitiesUpdate()
    {
        // While the swordAbilities object is active in the HUD
        while(swordAbilities.activeInHierarchy)
        {
            // If spin attack in cooldown
            if (PlayerMovements.spinTimer > 0)
            {
                // Set cooldown text and apply black filter over the image
                swordCooldownsText[0].text = PlayerMovements.spinTimer.ToString();
                swordAbilitiesImage[0].color = new Color(0.2f, 0.2f, 0.2f);
            }

            else
            {
                // Remove cooldown text and restore original color
                swordCooldownsText[0].text = "";
                swordAbilitiesImage[0].color = new Color(1f, 1f, 1f);
                // Wait for an ability cooldown to start before restarting the coroutine
                yield return new WaitUntil(()=> PlayerMovements.spinTimer > 0);
            }
            // Update UI every .1 second
            yield return new WaitForSeconds(.1f);
        }
    }

    public void ActivateSwordAbilities()
    {
        // Activate the swordAbilities object in the HUD and stat the couroutine to manage it
        swordAbilities.SetActive(true);
        StartCoroutine("SwordAbilitiesUpdate");
    }
}
