using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon;

public class MultiSnake : Photon.MonoBehaviour
{

  
    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;

    void Start()
    {
        PhotonNetwork.sendRate = 50;
        PhotonNetwork.sendRateOnSerialize = 50;
    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            this.correctPlayerPos = transform.position;
            this.correctPlayerRot = transform.rotation;
            // transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, 0);
            //transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, 0);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(gameObject.GetActive());
           

        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
            gameObject.SetActive((bool)stream.ReceiveNext());
           
        }
    }

}
