using UnityEngine;
using System.Collections;

public class CameraWork : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
	}
}
