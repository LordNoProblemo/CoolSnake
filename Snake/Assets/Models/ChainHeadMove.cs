using UnityEngine;
using System.Collections;

public class ChainHeadMove : MonoBehaviour {

    public float speed = 5f;
    float x, z;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        transform.position += new Vector3(x, 0f, z);
    }
}
