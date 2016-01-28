using UnityEngine;
using System.Collections;

public class BaseMap : MonoBehaviour {

    public Transform floor;
    public Transform border;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < 23; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if(i == 0 || i == 22 || j == 0 || j == 8)
                    Instantiate(border, new Vector3(-11 + i, -4 + j, 0), Quaternion.identity);
                else
                    Instantiate(floor, new Vector3(-11 + i, -4 + j, 0), Quaternion.identity);

            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
