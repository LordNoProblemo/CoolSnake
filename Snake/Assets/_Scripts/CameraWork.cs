using UnityEngine;
using System.Collections;

public class CameraWork : Photon.MonoBehaviour {

	
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine)
            gameObject.SetActive(true);
        else
            GameObject.Destroy(gameObject);
	}
}
