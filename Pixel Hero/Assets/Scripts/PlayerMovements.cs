using UnityEngine;
using System.Collections;

public class PlayerMovements : MonoBehaviour {

    public Transform SpawnPoint;
    public float walkSpeed = 6.0f;
    public float runSpeed = 10f;
    public float attackCooldown = 0.25f;
    public float staminaCooldown = 3f;
    public float staminaRun;
    public float staminaRegen;
    public float staminaSword;

    Vector3 move;
    Rigidbody2D playerRigidbody;
    Animator anim;

    float attackTimer = 0;
    static public  float staminaTimer = 0;
    static public float stamina = 100f;
    bool attacking = false;
    bool running = false;
    bool moving;
    float speed;

    // Use this for initialization
    void Start()
    {
        // Get required components
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Run();
        Attack();
        Stamina();

        if(!attacking)
            Move();

        // Set sprite sorting order according to vertical position
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    void Attack()
    {
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && attacking)
            attacking = false;

        if (Input.GetButtonDown("Fire1") && !running && stamina > 0)
        {
            anim.SetBool("Left", false); anim.SetBool("Right", false); ; anim.SetBool("Up", false); ; anim.SetBool("Down", false);
            attacking = true;
            attackTimer = attackCooldown;
            anim.SetTrigger("Attack");
            stamina -= staminaSword;
        }
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > 0)
            {
                if (moving)
                {
                    speed = runSpeed;
                    anim.speed = 2;
                    running = true;
                    stamina -= Time.deltaTime * staminaRun;
                }
            }
            else
            {
                running = false;
                anim.speed = 1;
                speed = walkSpeed;
            }
        }
        else
        {
            running = false;
            anim.speed = 1;
            speed = walkSpeed;
        }
    }

    void Stamina()
    {
        if (running || attacking)
            staminaTimer = staminaCooldown;

        if (staminaTimer > 0)
            staminaTimer -= Time.deltaTime;

        if (stamina < 100 && staminaTimer <= 0)
            stamina += Time.deltaTime * staminaRegen;
    }

    void Move()
    {
        // Get vertical/horizontal input value with wasd or arrows
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

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
}
