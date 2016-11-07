using UnityEngine;
using System.Collections;

public class Enter : Photon.MonoBehaviour {

    bool woof = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionStay(Collision other)
    {
        if (woof)
            return;
        woof = true;
        Debug.Log(other.gameObject.GetComponentInParent<GameManager>() == null);
        if (other.gameObject.GetComponent<GameManager>() == null && other.gameObject.GetComponentInParent<GameManager>()==null)
            return;
        if (other.gameObject.GetComponent<GameManager>() != null)
        {
            other.gameObject.GetComponent<GameManager>().snake2 = gameObject;
            gameObject.GetComponent<Snake>().My = other.gameObject.GetComponent<GameManager>().PointsMul;
            gameObject.GetComponent<Snake>().MullOther = other.gameObject.GetComponent<GameManager>().PointsMul;
            gameObject.GetComponent<Snake>().enabled = true;
            
            other.gameObject.GetComponent<GameManager>().enabled = true;
          
            gameObject.transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);
        }
        else
        {
        
            other.gameObject.GetComponentInParent<GameManager>().snake2 = gameObject;
            gameObject.GetComponent<Snake>().My = other.gameObject.GetComponentInParent<GameManager>().PointsMul;
            gameObject.GetComponent<Snake>().MullOther = other.gameObject.GetComponentInParent<GameManager>().PointsMul;
            gameObject.GetComponent<Snake>().enabled = true;
            //other.gameObject.GetComponent<GameManager>().ForceStart();
            other.gameObject.GetComponentInParent<GameManager>().enabled = true;
            gameObject.transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);

        }


    }
}
