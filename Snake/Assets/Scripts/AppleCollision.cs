using UnityEngine;
using System.Collections;

public class AppleCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision other)
    {
        gameObject.active = false;
    }
}
