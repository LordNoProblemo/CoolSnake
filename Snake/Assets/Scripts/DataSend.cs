using UnityEngine;
using System.Collections;

public class DataSend : Photon.MonoBehaviour {
    public GameObject snake;
    public int cnt=0;
    public bool en=true;
	// Use this for initialization
	void Start () {
        PhotonNetwork.sendRate = 50;
        PhotonNetwork.sendRateOnSerialize = 50;
    }
	
	// Update is called once per frame
	void Update () {
        en = snake.GetComponent<Snake>().enabled;
        cnt = snake.GetComponent<Snake>().count;

    }
    void OnCollisionStay(Collision col)
    {
        if (PhotonNetwork.connected && col.gameObject.GetComponent<GameManager>() != null)
        {
            col.gameObject.GetComponent<GameManager>().otherCnt = cnt;
            col.gameObject.GetComponent<GameManager>().En = en;
        }
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(en);
            stream.SendNext(cnt);

        }
        else
        {
            // Network player, receive data
            en = (bool)stream.ReceiveNext();
            cnt = (int)stream.ReceiveNext();
        }
    }
}
