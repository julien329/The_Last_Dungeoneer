using UnityEngine;
using System.Collections;

public class PickSword : MonoBehaviour {

    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovements>().EquipSword();
            Destroy(gameObject);
        }
    }
}
