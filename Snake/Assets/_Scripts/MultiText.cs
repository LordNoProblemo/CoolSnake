using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiText : Photon.MonoBehaviour {

    public Text t;
    // Use this for initialization]
    void Start()
    {
        PhotonNetwork.sendRate = 50;
        PhotonNetwork.sendRateOnSerialize = 50;
        
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(t.text);
        }
        else
        {
            t.text = (string)stream.ReceiveNext();
        }
    }
}

