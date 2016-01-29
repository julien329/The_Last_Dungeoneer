using UnityEngine;
using System.Collections;

public class PlayerMovements : MonoBehaviour {

    public float speed = 8.0f;

    Vector3 move;
    Rigidbody2D playerRigidbody;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        // Move senteces
        //playerRigidbody.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * speed, 0.8f), Mathf.Lerp(0, Input.GetAxis("Vertical") * speed, 0.8f));

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Move the player around the scene.
        Move(h, v);

        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }


    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        move.Set(h, v, 0f);

        //Move current position using ClampMagnitude to normalize the diagonal run speed.
        playerRigidbody.MovePosition(transform.position + Vector3.ClampMagnitude(move, 1.0f) * speed * Time.deltaTime);

        bool up = (v > 0);
        anim.SetBool("Up", up);

        bool down = (v < 0);
        anim.SetBool("Down", down);

        bool right = (h > 0);
        anim.SetBool("Right", right);

        bool left = (h < 0);
        anim.SetBool("Left", left);

    }
}
