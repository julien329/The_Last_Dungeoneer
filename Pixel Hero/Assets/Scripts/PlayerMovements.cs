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
    }


    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        move.Set(h, v, 0f);

        //Move current position using ClampMagnitude to normalize the diagonal run speed.
        playerRigidbody.MovePosition(transform.position + Vector3.ClampMagnitude(move, 1.0f) * speed * Time.deltaTime);

        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
