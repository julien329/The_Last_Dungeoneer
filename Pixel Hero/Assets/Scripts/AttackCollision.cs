using UnityEngine;
using System.Collections;

public class AttackCollision : MonoBehaviour {

    void OnTriggerStay2D(Collider2D other)
    {
        transform.parent.gameObject.GetComponent<PlayerMovements>().AttackCollision(other);
    }
}
