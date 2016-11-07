using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Snake : Photon.MonoBehaviour
{

    public Dictionary<string, KeyCode> MoveKeys;
    public Color SnakeColor;
    public List<GameObject> BodyList = new List<GameObject>();
    public float speed = 0.02f,tempSpeed;
    public bool destroyed = false;
    public Text My, MullOther;
    public Vector3 dire = new Vector3(0, 0, 1);
    public char dir = 'u';
    public float x, z;
    public GameObject body, other;
    public bool eat = false;
    private string tag;
    private int am = 0;
    public int count = 0;

    //for 3d 
    private Quaternion m_CharacterTargetRot;//help to give rotation to the charecter


    // Use this for initialization
    protected virtual void Start()
    {
        tempSpeed = speed;
        /* if (My != null)
             My.color = gameObject.GetComponent<Renderer>().material.color;*/
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.sendRate = 50;
            PhotonNetwork.sendRateOnSerialize = 50;
        }
        if (gameObject.tag == "Snake2" && PhotonNetwork.connected)
        {

            PhotonPlayer.Find(2).SetScore(0);
            count = 0;
        }

        //for 3d
        if (MenuScript.gameMode == Mode.Multiplayer_3D)
            m_CharacterTargetRot.y = 1f;//initiate the rotation

    }
    public void UpdateText()
    {
        if (My != null)
        {
            My.text = count.ToString();

        }

    }


  
    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("FUCK");
        if (PhotonNetwork.connected && !photonView.isMine)
            return;
        if (MenuScript.gameMode == Mode.Normal)
        {
            if (other.gameObject.tag == "Apple")
            {

                other.gameObject.active = false;

                eat = true;
            }
            else if (other.gameObject.tag == body.tag)
            {
                for (int i = 0; i < BodyList.Count; i++)
                {
                    if (BodyList[i] == other.gameObject && i < 2)
                        return;
                }
                foreach (ContactPoint i in other.contacts)
                {
                    if (i.normal.normalized == -dire)
                    {
                        enabled = false;

                        break;
                    }
                }
            }


            else if (other.gameObject.tag == "Wall")
            { enabled = false; }
            else if (other.gameObject.tag == this.other.tag)
            {
                other.gameObject.GetComponent<Snake>().enabled = false;

                enabled = false;
            }
            else if (other.gameObject.tag == this.other.GetComponent<Snake>().body.tag)
            {


                other.gameObject.tag = "null";

                UpdateText();
            }
        }
        if (PhotonNetwork.connected) //MenuScript.gameMode == Mode.Multiplayer_Normal || MenuScript.gameMode == Mode.Multiplayer_3D
        {
            if (other.gameObject.tag == "Apple")
            {

                other.gameObject.active = false;

                eat = true;
            }
            else if (other.gameObject.tag == body.tag)
            {
                for (int i = 0; i < BodyList.Count; i++)
                {
                    if (BodyList[i] == other.gameObject && i < 2)
                        return;
                }
                foreach (ContactPoint i in other.contacts)
                {
                    if (i.normal.normalized == -dire)
                    {
                        enabled = false;
                        if (gameObject.tag == "Snake2")
                            PhotonPlayer.Find(2).SetScore(-1);
                        break;
                    }
                }
            }


            else if (other.gameObject.tag == "Wall")
            {
                enabled = false;
                if (gameObject.tag == "Snake2")
                    PhotonPlayer.Find(2).SetScore(-1);
            }
            else if (other.gameObject.tag == this.other.tag)
            {
                other.gameObject.GetComponent<Snake>().enabled = false;

                enabled = false;

                PhotonPlayer.Find(2).SetScore(-1);
            }
            else if (other.gameObject.tag == this.other.GetComponent<Snake>().body.tag)
            {


                // other.gameObject.tag = "null";

                UpdateText();
            }
        }


    }

  

    public void destroy()
    {
        destroyed = true;
        for (int i = 0; i < BodyList.Count; i++)
        {
            GameObject.Destroy(BodyList[i]);
        }
        gameObject.active = false;
    }
    // Update is called once per frame
  
    public void FixedUpdate()
    {
        if (MenuScript.gameMode == Mode.Normal)
            SinglePlayerMove();
        else if (photonView.isMine)
        {
            if (MenuScript.gameMode == Mode.Multiplayer_Normal)
                MultiPlayerMove();
            else if (MenuScript.gameMode == Mode.Multiplayer_3D)
                MultiPlayer3DMove();
        }

    }

    void SinglePlayerMove()
    {
        int j = 0;
        for (j = 0; j < BodyList.Count && BodyList[j].gameObject.tag != "null"; j++)
        {

        }
        if (j < BodyList.Count)
        {
            if (MenuScript.eat == EatValue.GetAll)
                other.GetComponent<Snake>().count += (BodyList.Count - j);
            else if (MenuScript.eat == EatValue.GetOne)
                other.GetComponent<Snake>().count += 1;

            for (int k = j; k < BodyList.Count; k++)
                GameObject.Destroy(BodyList[k]);
            count -= (BodyList.Count - j);
            BodyList.RemoveRange(j, BodyList.Count - j);
        }

        UpdateText();
        int i = 0;
        if (eat)
        {
            count = BodyList.Count + 1;
            UpdateText();
            eat = false;
        }
        if (count > BodyList.Count)
        {


            if (BodyList.Count == 0)
            {
                BodyList.Add((GameObject)Instantiate(body));
                BodyList[0].gameObject.active = true;

                switch (dir)
                {
                    case 'u':
                        BodyList[0].transform.position = transform.position + new Vector3(0, 0, -0.1f);

                        break;
                    case 'd':
                        BodyList[0].transform.position = transform.position + new Vector3(0, 0, 0.1f);
                        break;
                    case 'r':
                        BodyList[0].transform.position = transform.position + new Vector3(-0.1f, 0, 0);
                        break;
                    case 'l':
                        BodyList[0].transform.position = transform.position + new Vector3(0.1f, 0, 0);
                        break;
                }


                i++;
            }
            else if (i == 0)
            {
                BodyList.Add(Instantiate(body));

                switch (dir)
                {
                    case 'u':
                        BodyList[BodyList.Count - 1].transform.position = BodyList[BodyList.Count - 2].transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    case 'd':
                        BodyList[BodyList.Count - 1].transform.position = BodyList[BodyList.Count - 2].transform.position + new Vector3(0, 0, 0.1f);
                        break;
                    case 'r':
                        BodyList[BodyList.Count - 1].transform.position = BodyList[BodyList.Count - 2].transform.position + new Vector3(-0.1f, 0, 0);
                        break;
                    case 'l':
                        BodyList[BodyList.Count - 1].transform.position = BodyList[BodyList.Count - 2].transform.position + new Vector3(0.1f, 0, 0);
                        break;
                }
                i++;
                BodyList[BodyList.Count - 1].active = true;


            }




        }

        for (int k = BodyList.Count - 1 - i; k > 0; k--)
        {

            BodyList[k].transform.LookAt(BodyList[k - 1].transform.position);
            BodyList[k].transform.position = BodyList[k - 1].transform.position - 0.1f * BodyList[k].transform.forward;
        }
        if (BodyList.Count > 0 && i == 0)
        {

            BodyList[0].transform.LookAt(transform.position);
            BodyList[0].transform.position = transform.position - 0.1f * BodyList[0].transform.forward;
        }
        MoveHead();
    }

    void MultiPlayerMove()
    {

        if (gameObject.tag == "Snake2" && PhotonNetwork.connected)
        {
            if (PhotonPlayer.Find(2).GetScore() <0)
            {
                speed = 0;
                return;

            }
            else
                speed = tempSpeed;
            count = PhotonPlayer.Find(2).GetScore();


        }
        if (gameObject.tag == "Snake1" && PhotonNetwork.connected)
        {
            if (PhotonPlayer.Find(1).GetScore() > 0)
            {
                count += PhotonPlayer.Find(1).GetScore();
                PhotonPlayer.Find(1).SetScore(0);
            }

        }
        int j = 0;
        for (j = 0; j < BodyList.Count && BodyList[j].gameObject.tag != "null"; j++)
        {

        }
        if (j < BodyList.Count)
        {
            if (MenuScript.eat == EatValue.GetAll)
            {

                if (gameObject.tag == "Snake1")
                    PhotonPlayer.Find(2).SetScore(PhotonPlayer.Find(2).GetScore() + (BodyList.Count - j));
                else
                    PhotonPlayer.Find(1).SetScore((BodyList.Count - j));
            }
            else if (MenuScript.eat == EatValue.GetOne)
            {

                if (gameObject.tag == "Snake1")
                    PhotonPlayer.Find(2).SetScore(PhotonPlayer.Find(2).GetScore() + 1);
                else
                    PhotonPlayer.Find(1).SetScore(1);
            }

            for (int k = j; k < BodyList.Count; k++)
                PhotonNetwork.Destroy(BodyList[k]);
            count -= (BodyList.Count - j);
            if (gameObject.tag == "Snake2")
                PhotonPlayer.Find(2).SetScore(count);
            BodyList.RemoveRange(j, BodyList.Count - j);

            if (gameObject.tag == "Snake1")
                UpdateText();
        }


        int i = 0;
        if (eat)
        {
            count = BodyList.Count + 1;
            if (gameObject.tag == "Snake2")
                PhotonPlayer.Find(2).SetScore(count);
            eat = false;
        }
        if (count > BodyList.Count)
        {

            UpdateText();
            if (BodyList.Count == 0)
            {
                BodyList.Add(PhotonNetwork.Instantiate(body.name, new Vector3(0, 0, 0), Quaternion.identity, 0));
                BodyList[0].gameObject.active = true;

                switch (dir)
                {
                    case 'u':
                        BodyList[0].transform.position = transform.position + new Vector3(0, 0, -0.1f);

                        break;
                    case 'd':
                        BodyList[0].transform.position = transform.position + new Vector3(0, 0, 0.1f);
                        break;
                    case 'r':
                        BodyList[0].transform.position = transform.position + new Vector3(-0.1f, 0, 0);
                        break;
                    case 'l':
                        BodyList[0].transform.position = transform.position + new Vector3(0.1f, 0, 0);
                        break;
                }


                i++;
            }
            else if (i == 0)
            {
                BodyList.Add(PhotonNetwork.Instantiate(body.name, new Vector3(0, 0, 0), Quaternion.identity, 0));

                switch (dir)
                {
                    case 'u':
                        BodyList[BodyList.Count - 1].transform.position = BodyList[BodyList.Count - 2].transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    case 'd':
                        BodyList[BodyList.Count - 1].transform.position = BodyList[BodyList.Count - 2].transform.position + new Vector3(0, 0, 0.1f);
                        break;
                    case 'r':
                        BodyList[BodyList.Count - 1].transform.position = BodyList[BodyList.Count - 2].transform.position + new Vector3(-0.1f, 0, 0);
                        break;
                    case 'l':
                        BodyList[BodyList.Count - 1].transform.position = BodyList[BodyList.Count - 2].transform.position + new Vector3(0.1f, 0, 0);
                        break;
                }
                i++;
                BodyList[BodyList.Count - 1].active = true;


            }




        }

        for (int k = BodyList.Count - 1 - i; k > 0; k--)
        {

            BodyList[k].transform.LookAt(BodyList[k - 1].transform.position);
            BodyList[k].transform.position = BodyList[k - 1].transform.position - 0.1f * BodyList[k].transform.forward;
        }
        if (BodyList.Count > 0 && i == 0)
        {

            BodyList[0].transform.LookAt(transform.position);
            BodyList[0].transform.position = transform.position - 0.1f * BodyList[0].transform.forward;
        }
        MoveHead();
    }

    void MultiPlayer3DMove()
    {
        if (gameObject.tag == "Snake2" && PhotonNetwork.connected)
        {
            if (PhotonPlayer.Find(2).GetScore() == -1)
            {
                enabled = false;
                return;

            }
            count = PhotonPlayer.Find(2).GetScore();


        }
        if (gameObject.tag == "Snake1" && PhotonNetwork.connected)
        {
            if (PhotonPlayer.Find(1).GetScore() > 0)
            {
                count += PhotonPlayer.Find(1).GetScore();
                PhotonPlayer.Find(1).SetScore(0);
            }

        }
        int j = 0;
        for (j = 0; j < BodyList.Count && BodyList[j].gameObject.tag != "null"; j++)
        {

        }
        if (j < BodyList.Count)
        {
            if (MenuScript.eat == EatValue.GetAll)
            {

                if (gameObject.tag == "Snake1")
                    PhotonPlayer.Find(2).SetScore(PhotonPlayer.Find(2).GetScore() + (BodyList.Count - j));
                else
                    PhotonPlayer.Find(1).SetScore((BodyList.Count - j));
            }
            else if (MenuScript.eat == EatValue.GetOne)
            {

                if (gameObject.tag == "Snake1")
                    PhotonPlayer.Find(2).SetScore(PhotonPlayer.Find(2).GetScore() + 1);
                else
                    PhotonPlayer.Find(1).SetScore(1);
            }

            for (int k = j; k < BodyList.Count; k++)
                PhotonNetwork.Destroy(BodyList[k]);
            count -= (BodyList.Count - j);
            if (gameObject.tag == "Snake2")
                PhotonPlayer.Find(2).SetScore(count);
            BodyList.RemoveRange(j, BodyList.Count - j);

            if (gameObject.tag == "Snake1")
                UpdateText();
        }

        int i = 0;
        if (eat)
        {
            count = BodyList.Count + 1;
            if (gameObject.tag == "Snake2")
                PhotonPlayer.Find(2).SetScore(count);
            eat = false;
        }
        if (count > BodyList.Count)
        {

            UpdateText();
            if (BodyList.Count == 0)
            {
                BodyList.Add(PhotonNetwork.Instantiate(body.name, new Vector3(0, 0, 0), Quaternion.identity, 0));
                BodyList[0].gameObject.active = true;

                ///<summary>
                /// --- new
                /// insted of the switch with the 'dir'
                /// </summary>
                BodyList[0].transform.localPosition = transform.position + new Vector3(0, 0, -1f);


                i++;
            }
            else if (i == 0)
            {
                BodyList.Add(PhotonNetwork.Instantiate(body.name, new Vector3(0, 0, 0), Quaternion.identity, 0));

                ///<summary>
                /// --- new
                /// insted of the switch with the 'dir'
                /// </summary>
                BodyList[BodyList.Count - 1].transform.position = BodyList[BodyList.Count - 2].transform.position + new Vector3(0, 0, -0.1f);

                i++;
                BodyList[BodyList.Count - 1].active = true;


            }




        }

        for (int k = BodyList.Count - 1 - i; k > 0; k--)
        {

            BodyList[k].transform.LookAt(BodyList[k - 1].transform.position);
            BodyList[k].transform.position = BodyList[k - 1].transform.position - 0.2f * BodyList[k].transform.forward;
        }
        if (BodyList.Count > 0 && i == 0)
        {

            BodyList[0].transform.LookAt(transform.position);
            BodyList[0].transform.position = transform.position - 0.2f * BodyList[0].transform.forward;
        }

        Multiplayer3DMoveHead();
    }

    private void Multiplayer3DMoveHead()
    {
        
        //move the object in the Vertical axis
        this.transform.Translate(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * speed);

        //move the object in the Horizontal axis
        this.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0, 0);

        //lock the cursor and make it invisible
        //***----------this should be in the start fuction
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //rotate the object in the x and y axises using mouse input
        float yRot = Input.GetAxis("Mouse X") * 2f;
        float xRot = Input.GetAxis("Mouse Y") * 2f;
        m_CharacterTargetRot *= Quaternion.Euler(-xRot, yRot, 0f);
        this.transform.localRotation = m_CharacterTargetRot;
    }

    public void MoveHead()
    {

        if (!PhotonNetwork.connected || photonView.isMine)
        {
            if (Input.GetKey(MoveKeys["Right"]) && dir != 'l')
            {
                MoveRight();
            }
            if (Input.GetKey(MoveKeys["Left"]) && dir != 'r')
            {
                MoveLeft();
            }
            if (Input.GetKey(MoveKeys["Up"]) && dir != 'd')
            {
                MoveUp();
            }
            if (Input.GetKey(MoveKeys["Down"]) && dir != 'u')
            {
                MoveDown();
            }
        }
        transform.position = transform.position + speed * dire;

    }

    private void MoveRight()
    {
        dire = new Vector3(1, 0, 0);
        transform.LookAt(transform.position + new Vector3(1, 0, 0));
        dire = new Vector3(1, 0, 0);
        dir = 'r';
    }

    private void MoveLeft()
    {
        dire = new Vector3(-1, 0, 0);
        transform.LookAt(transform.position + new Vector3(-1, 0, 0));
        dir = 'l';
        dire = new Vector3(-1, 0, 0);
    }

    private void MoveDown()
    {
        dire = new Vector3(0, 0, -1);
        transform.LookAt(transform.position + new Vector3(0, 0, -1));
        dir = 'd';
        dire = new Vector3(0, 0, -1);
    }

    private void MoveUp()
    {
        dire = new Vector3(0, 0, 1);
        transform.LookAt(transform.position + new Vector3(0, 0, 1));
        dir = 'u';
        dire = new Vector3(0, 0, 1);
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting && photonView.isMine)
        {

            stream.SendNext(My.text);
            stream.SendNext(MullOther.text);

            stream.SendNext(enabled);
            stream.SendNext(count);


        }
        else
        {

            My.text = (string)stream.ReceiveNext();
            MullOther.text = (string)stream.ReceiveNext();

            enabled = (bool)stream.ReceiveNext();
            count = (int)stream.ReceiveNext();

        }
    }
}

