using UnityEngine;
using System.Collections;

public class PlayerMovements : MonoBehaviour {

    public float speed = 8.0f;
    public Transform SpawnPoint;
    float attackTimer = 0;
    public float cooldown = 0.25f;
    bool attacking = false;
    public float stamina = 100f;
    public float staminaCost;
    public float staminaUpRate;

    Vector3 move;
    Rigidbody2D playerRigidbody;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        // Get required components
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        // Get vertical/horizontal input value with wasd or arrows
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            speed = 12f;
            stamina -= Time.deltaTime * staminaCost;
        }
        else
        {
            speed = 6f;
            if (stamina < 100)
                stamina += Time.deltaTime * staminaUpRate;
        }

        Debug.Log(stamina, gameObject);
 
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && attacking)
            attacking = false;

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("Left", false); anim.SetBool("Right", false); ; anim.SetBool("Up", false); ; anim.SetBool("Down", false);
            attacking = true;
            attackTimer = cooldown;
            anim.SetTrigger("Attack");
        }

        // Move the player around the scene.
        if(!attacking)
            Move(h, v);

        // Set sprite sorting order according to vertical position
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    void Move(float h, float v)
    {
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
