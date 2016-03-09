using UnityEngine;
using System.Collections;

public class SortOrder : MonoBehaviour {

    // Set sprite sorting order according to vertical position (for not moving objects)
    void Start () {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }
}
