using UnityEngine;
using System.Collections;

public class FindSnake : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(PhotonView.Find(2001)!=null)
        {
            PhotonView.Find(0);
            gameObject.GetComponent<GameManager>().snake2 = PhotonView.Find(2001).gameObject;
            PhotonView.Find(2001).gameObject.GetComponent<Snake>().My= gameObject.GetComponent<GameManager>().PointsMul;
            PhotonView.Find(2001).gameObject.GetComponent<Snake>().MullOther = gameObject.GetComponent<GameManager>().PointsMul;
            PhotonView.Find(2001).gameObject.GetComponent<Snake>().enabled = true;
            gameObject.GetComponentInParent<GameManager>().enabled = true;
            enabled = false;
        }
	}
}
