using UnityEngine;
using System.Collections;

public class BodyCheck : Photon.MonoBehaviour {
    public GameObject Enemy;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == Enemy.tag)
            gameObject.tag = "null";
    }
}
