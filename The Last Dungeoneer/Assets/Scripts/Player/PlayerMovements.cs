using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovements : MonoBehaviour {

    public Transform SpawnPoint;
    public List<GameObject> hitColliders = new List<GameObject>();
    public GameObject hud;
    public float walkSpeed = 6.0f;
    public float runSpeed = 10f;
    public float attackCooldown = 0.25f;
    public float staminaCooldown = 3f;
    public float staminaRun;
    public float staminaRegen;
    public float staminaSword;
    public float spinCooldown = 5;

    private Vector3 move;
    private Rigidbody2D playerRigidbody;
    private Animator anim;
    private HUDManager hudManager;

    private float attackTimer = 0;
    static public float spinTimer = 0;
    static public float staminaTimer = 0;
    static public float stamina = 100f;
    private bool attacking = false;
    private bool running = false;
    private bool moving;
    private float speed;
    private bool spinning = false;
    private bool weaponEquiped = false;
    private bool weaponIsSword = false;

    // Use this for initialization
    void Start()
    {
        // Get required components
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        hudManager = hud.GetComponent<HUDManager>();
    }

    void Update()
    {
        Run();
        Attack();
        if(weaponIsSword)
            SpecialAttacksSword();
        Stamina();

        // Stop moving if attacking
        if (!attacking)
            Move();

        // Set sprite sorting order according to vertical position
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        SwordHitCollider();
    }

    private void SpecialAttacksSword()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !running && spinTimer <= 0)
        {
            anim.SetTrigger("Spin");
            spinTimer = spinCooldown;
            InvokeRepeating("SwordSpinCooldown", 1f, 1f);
            spinning = true;
        }

        if(spinTimer > 0)
            spinning = anim.GetCurrentAnimatorStateInfo(1).IsName("SwordSpin");
    }

    private void Attack()
    {
        // If attack timer is up, lower it until 0
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        // If attack timer is null, set to not attacking
        if (attackTimer <= 0 && attacking)
            attacking = false;

        // Poll for attack input if not running and have enough stamina
        if (Input.GetKeyDown(KeyCode.UpArrow) && !running && stamina > 0)
        {
            // Put a delay between attacks, set attack animation and apply stamina cost
            attacking = true;
            attackTimer = attackCooldown;
            anim.SetTrigger("Attack");
            stamina = Mathf.Clamp(stamina - staminaSword, 0f, 100f);
        }
    }

    private void Run()
    {
        // Pool for run input
        if (Input.GetKey(KeyCode.LeftShift) && !spinning)
        {
            // If have enought stamina
            if (stamina > 0)
            {
                // If not just pressing the input without moving
                if (moving)
                {
                    // Set running speed, speed up walking animations and slowly remove stamina
                    speed = runSpeed;
                    anim.speed = 2;
                    running = true;
                    stamina = Mathf.Clamp(stamina - (Time.deltaTime * staminaRun), 0f, 100f);
                }
            }
            // If not enought stamina, walk instead.
            else
            {
                running = false;
                anim.speed = 1;
                speed = walkSpeed;
            }
        }
        // If not pressing run input, walk.
        else
        {
            running = false;
            anim.speed = 1;
            speed = walkSpeed;
        }
    }

    private void Stamina()
    {
        // If running or attacking, restart stamina regen cooldown
        if (running || attacking)
            staminaTimer = staminaCooldown;

        // If stamina cooldown is on, count down till null;
        if (staminaTimer > 0)
            staminaTimer -= Time.deltaTime;

        // If stamina is not full and cooldown is null, regen stamina
        if (stamina < 100 && staminaTimer <= 0)
            stamina = Mathf.Clamp(stamina + (Time.deltaTime * staminaRegen), 0f, 100f);
    }

    private void Move()
    {
        // Get vertical/horizontal input value with wasd or arrows
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // If vertical or hozontal value is not null, set moving true
        if (h != 0 || v != 0)
            moving = true;
        else
            moving = false;

        // Set the movement vector based on the axis input.
        move.Set(h, v, 0f);

        //Move current position using ClampMagnitude to normalize the diagonal run speed.
        playerRigidbody.MovePosition(transform.position + Vector3.ClampMagnitude(move, 1.0f) * speed * Time.deltaTime);

        // Walk/idle up animation
        bool up = (v > 0);
        anim.SetBool("Up", up);

        // Walk/idle down animation
        bool down = (v < 0);
        anim.SetBool("Down", down);

        // Walk/idle down animation
        bool right = (h > 0);
        anim.SetBool("Right", right);

        // Walk/idle down animation
        bool left = (h < 0);
        anim.SetBool("Left", left);
    }

    // Activate proper sword hit collider according to the current mechanim animation state (Up, down, left, right).
    private void SwordHitCollider()
    {
        if (weaponEquiped)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdleRight"))
            {
                hitColliders[0].SetActive(false);
                hitColliders[1].SetActive(false);
                hitColliders[2].SetActive(false);
                hitColliders[3].SetActive(true);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdleLeft"))
            {
                hitColliders[0].SetActive(false);
                hitColliders[1].SetActive(false);
                hitColliders[2].SetActive(true);
                hitColliders[3].SetActive(false);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdleUp"))
            {
                hitColliders[0].SetActive(true);
                hitColliders[1].SetActive(false);
                hitColliders[2].SetActive(false);
                hitColliders[3].SetActive(false);
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdleDown"))
            {
                hitColliders[0].SetActive(false);
                hitColliders[1].SetActive(true);
                hitColliders[2].SetActive(false);
                hitColliders[3].SetActive(false);
            }
        }
        else
        {
            hitColliders[0].SetActive(false);
            hitColliders[1].SetActive(false);
            hitColliders[2].SetActive(false);
            hitColliders[3].SetActive(false);
        }
    }

    public void EquipSword()
    {
        anim.SetLayerWeight(1, 100);
        weaponEquiped = true;
        weaponIsSword = true;
        hudManager.ActivateSwordAbilities();
    }

    public void AttackCollision(Collider2D other)
    {
        if (attacking)
            Destroy(other.gameObject);
    }


    /**************************************************** Special Attacks Countdown****************************************/

    void SwordSpinCooldown()
    {
        spinTimer--;
        if (spinTimer <= 0)
            CancelInvoke("SwordSpinCooldown");
    }

}
