using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class GameManager : Photon.MonoBehaviour
{
    int sum;
    public int otherCnt = 0;
    public bool En = true;
    bool pause = false, finish = false;
    float x, y, z;
    public Text Toto, PointsMul;
    private List<GameObject> Apple;
    public GameObject snake1, snake2, apple;
    bool started = false;
    // Use this for initialization
    public void ForceStart()
    {
        Start();
    }

    void Start()
    {
        if (snake1 != null && snake2 != null)
        {
            Toto.gameObject.SetActive(true);
            if (PhotonNetwork.connected)
                PhotonPlayer.Find(2).SetScore(-2);
            Toto.text = "3";
            //System.Threading.Thread.Sleep(1000);
            Toto.text = "2";
            // System.Threading.Thread.Sleep(1000);
            Toto.text = "1";
            //System.Threading.Thread.Sleep(1000);
            Toto.text = "";
            started = true;
            snake1.GetComponent<Snake>().enabled = true;
            //snake2.GetComponent<Snake>().enabled = true;
            if (PhotonNetwork.connected)
                PhotonPlayer.Find(2).SetScore(0);
            sum = 0;
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = MenuScript.light;
            if (!PhotonNetwork.connected)
                Toto.gameObject.active = false;
            if (!PhotonNetwork.connected)
                StretchBoard();

            Apple = new List<GameObject>();
            if (!PhotonNetwork.connected)
            {
                Apple.Add(Instantiate(apple) as GameObject);
            }
            else
            {

                PhotonNetwork.sendRate = 50;
                PhotonNetwork.sendRateOnSerialize = 50;
                if (photonView.isMine)
                {
                    float rndApple = 0.06f;
                    if (MenuScript.gameMode == Mode.Multiplayer_3D)
                        rndApple = Random.Range(-1, 1);//CHECK RANGE!!!!!
                    Apple.Add(PhotonNetwork.Instantiate(apple.name, new Vector3(0, rndApple, 0), Quaternion.identity, 0));
                    Debug.Log("Woof");
                }
            }

            Apple[0].SetActive(false);
            if (!PhotonNetwork.connected)
            {
                float rndSnake = 0.05f;
                if (MenuScript.gameMode == Mode.Multiplayer_3D)
                    rndSnake = Random.Range(-1, 1);//CHECK RANGE!!!!!
                snake1.transform.position = new Vector3(Random.Range(-1, 1), rndSnake, Random.Range(-1, 1));
                do
                {
                    snake2.transform.position = new Vector3(Random.Range(-1, 1), rndSnake, Random.Range(-1, 1));
                } while (snake1.transform.position == snake2.transform.position);
            }
            else
            {
                snake1.transform.position = new Vector3(-0.5f, 0.05f, 0);
                snake2.transform.position = new Vector3(0.5f, 0.05f, 0);
            }

        }
    }



    public void StretchBoard()
    {
        this.gameObject.transform.localScale = new Vector3(Screen.width * 4.2f / Screen.height, 1, 4.2f);
    }

    public bool inPos(float x, float z, float y)
    {
        if (snake1.activeSelf && snake1.transform.position.x == x && snake1.transform.position.z == z)
        {
            if (MenuScript.gameMode != Mode.Multiplayer_3D)
                return true;
            else
                return snake1.transform.position.y == y;
        }


        if (snake2.activeSelf && snake2.transform.position.x == x && snake2.transform.position.z == z)
        {
            if (MenuScript.gameMode != Mode.Multiplayer_3D)
                return true;
            else
                return snake2.transform.position.y == y;
        }
        foreach (GameObject ob in GameObject.FindGameObjectsWithTag("Body1"))
        {
            if (ob.transform.position.x == x && ob.transform.position.z == z)
            {
                if (MenuScript.gameMode != Mode.Multiplayer_3D)
                    return true;
                else
                    return ob.transform.position.y == y;
            }
        }
        foreach (GameObject ob in GameObject.FindGameObjectsWithTag("Body2"))
        {
            if (ob.transform.position.x == x && ob.transform.position.z == z)
            {
                if (MenuScript.gameMode != Mode.Multiplayer_3D)
                    return true;
                else
                    return ob.transform.position.y == y;
            }
        }

        return false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {
        if (snake1 == null || snake2 == null)
            return;
        if (!started && snake1 != null && snake2 != null)
            return;
        if (PhotonNetwork.connected)
        {
            En = (PhotonPlayer.Find(2).GetScore() > -1);
            if (En)
                otherCnt = PhotonPlayer.Find(2).GetScore();

        }
        if (PointsMul != null)
            PointsMul.text = otherCnt.ToString();
        if (PointsMul != null)
            sum = snake1.GetComponent<Snake>().count + otherCnt;
        else
            sum = snake1.GetComponent<Snake>().count + snake2.GetComponent<Snake>().count;
        if ((int)Mathf.Sqrt(sum) + 1 > Apple.Count)
        {
            if (!PhotonNetwork.connected)
            {
                Apple.Add(Instantiate(apple) as GameObject);
            }
            else
            {
                if (photonView.isMine)
                {
                    Apple.Add(PhotonNetwork.Instantiate(apple.name, new Vector3(0, 0.05f, 0), Quaternion.identity, 0));
                    Debug.Log("Woof");
                }
            }
        }
        if (Input.GetKey(KeyCode.Escape) && !PhotonNetwork.connected)
            SceneManager.LoadScene("Main Menu");
        if (Input.GetKey(KeyCode.R))
        {
            Toto.gameObject.SetActive(true);
            Toto.text = "Reseting...";
            if (PhotonNetwork.connected)
                SceneManager.LoadScene("MultiPlayer");
            else
                SceneManager.LoadScene("Game");
        }
        if (Input.GetKeyUp(KeyCode.P) && !finish)
        {
            if (!PhotonNetwork.connected)
                Toto.gameObject.SetActive(!Toto.gameObject.activeSelf);
            Toto.text = "Paused";
            snake1.GetComponent<Snake>().enabled = !snake1.GetComponent<Snake>().enabled;
            snake2.GetComponent<Snake>().enabled = !snake2.GetComponent<Snake>().enabled;
            if (PhotonNetwork.connected && PhotonPlayer.Find(2).GetScore() > -1)
                PhotonPlayer.Find(2).SetScore(-2);
            else if (PhotonNetwork.connected)
                PhotonPlayer.Find(2).SetScore(otherCnt);
            pause = !pause;

        }
        List<GameObject> remove = new List<GameObject>();
        for (int i = 0; i < Apple.Count; i++)
        {

            if (!PhotonNetwork.connected && !Apple[i].gameObject.activeSelf)
            {
                x = Random.Range(-1.9f, 1.9f);
                z = Random.Range(-1.9f, 1.9f);
                y = Random.Range(-1.9f, 1.9f);
                while (inPos(x, z, y) || inApplePos(Apple[i]))
                {
                    x = Random.Range(-1.9f, 1.9f);
                    z = Random.Range(-1.9f, 1.9f);
                    y = Random.Range(-1.9f, 1.9f);
                }

                Apple[i].SetActive(true);
                if (MenuScript.gameMode != Mode.Multiplayer_3D)
                    Apple[i].transform.position = new Vector3(x, 0.06f, z);
                else
                    Apple[i].transform.position = new Vector3(x, y, z)/1.5f;
            }
            if (PhotonNetwork.connected && !Apple[i].gameObject.GetActive())
            {
                remove.Add(Apple[i]);

            }
        }
        int cnt = 0;
        foreach (GameObject o in remove)
        {
            Apple.Remove(o);
            if (!PhotonNetwork.connected)
                GameObject.Destroy(o);
            else
                PhotonNetwork.Destroy(o);
            cnt++;
        }
        for (int i = 0; i < cnt; i++)
        {
            GameObject o = PhotonNetwork.Instantiate(apple.name, new Vector3(0, 0.06f, 0), Quaternion.identity, 0);
            Apple.Add(o);
            x = Random.Range(-1.9f, 1.9f);
            z = Random.Range(-1.9f, 1.9f);
            y = Random.Range(-1.9f, 1.9f);
            while (inPos(x, z, y) || inApplePos(o))
            {
                x = Random.Range(-1.9f, 1.9f);
                z = Random.Range(-1.9f, 1.9f);
                y = Random.Range(-1.9f, 1.9f);
            }

            o.SetActive(true);
            if (MenuScript.gameMode != Mode.Multiplayer_3D)
                o.transform.position = new Vector3(x, 0.06f, z);
            else
                o.transform.position = new Vector3(x, y, z) / 1.5f;

        }
        bool SM1 = snake1.GetComponent<Snake>().enabled, SM2 = snake2.GetComponent<Snake>().enabled;

        if (!SM1 && (!SM2 || !En) && (!Toto.gameObject.activeSelf || Toto.text == "") && !finish)
        {
            finish = true;
            Toto.text = "DRAWWWWWW!!!";
            Toto.gameObject.SetActive(true);
            if (PhotonNetwork.connected)
                PhotonPlayer.Find(2).SetScore(-1);
        }
        if (SM1 && (!SM2 || !En) && !pause && !finish)
        {
            finish = true;
            snake1.GetComponent<Snake>().enabled = false;
            Toto.gameObject.SetActive(true);
            Toto.text = snake1.GetComponent<Renderer>().material.name.Split(' ')[0] + " Snake Won!";
            if (PhotonNetwork.connected)
            {
                PhotonPlayer.Find(2).SetScore(-1);
                Toto.text = "Player 1" + " Won!";
            }
            Toto.text.Replace("(Instance)", "");
        }
        if (!SM1 && (SM2 || En) && !pause && !finish)
        {
            finish = true;
            snake2.GetComponent<Snake>().enabled = false;
            Toto.gameObject.SetActive(true);
            Toto.text = snake2.GetComponent<Renderer>().material.name.Split(' ')[0] + " Snake Won!";
            if (PhotonNetwork.connected)
            {
                PhotonPlayer.Find(2).SetScore(-1);
                Toto.text = "Player 2" + " Won!";
            }

        }
    }


    private bool inApplePos(GameObject ap)
    {
        for (int i = 0; i < Apple.Count; i++)
        {
            if (Apple[i] != ap && Apple[i].transform.position.x == x && Apple[i].transform.position.z == z)
            {
                if (MenuScript.gameMode != Mode.Multiplayer_3D)
                    return true;
                else
                    return Apple[i].transform.position.y == y;
            }
        }
        return false;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(PointsMul.text);
            stream.SendNext(Toto.text);

        }
        else
        {
            PointsMul.text = (string)stream.ReceiveNext();
            Toto.text = (string)stream.ReceiveNext();
        }


    }

}
