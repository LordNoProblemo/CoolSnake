using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{

    public Image Load;
    public GameObject FloorPre;
    public Canvas PointsPre;
    public Text nameExist;
    public InputField Name;
    public Canvas Ui;
    public Text p1, p2;
    public Dropdown List;
    private static RoomInfo[] roomsList;
    public string version = "2.1";
    private int playernum;
    bool spawned = false,full=false;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(version);
        nameExist.gameObject.SetActive(false);
    }

    public void StartRoom()
    {
        if (nameExists(Name.text))
        {
            nameExist.gameObject.SetActive(true);
            return;
        }
        nameExist.gameObject.SetActive(false);
        PhotonNetwork.CreateRoom(Name.text, new RoomOptions() { MaxPlayers = 2 }, null);
    }

    public void JoinRoom()
    {
        if (nameExists(List.captionText.text))
            PhotonNetwork.JoinRoom(List.captionText.text);
    }

    void Awake()
    {
        PhotonNetwork.autoJoinLobby = true;
    }

    public bool nameExists(string name)
    {
        foreach (RoomInfo r in roomsList)
        {
            if (r.name == name)
                return true;
        }
        return false;
    }

    static bool IsFull(RoomInfo r)
    {
        return r.playerCount == r.maxPlayers;
    }
    void OnGUI()
    {

        if (!PhotonNetwork.connected)
        {
            
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }

        else if (PhotonNetwork.room == null)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            p1.gameObject.SetActive(false);
            p2.gameObject.SetActive(false);
            Load.gameObject.SetActive(false);
            if (roomsList != null)
            {
                List<string> ops = new List<string>();
                for (int i = 0; i < roomsList.Length; i++)
                {
                    if (!IsFull(roomsList[i]))
                        ops.Add(roomsList[i].name);
                }
                List.ClearOptions();
                List.AddOptions(ops);
            }
           
            }




        Ui.gameObject.SetActive(PhotonNetwork.room == null);
    }

    void OnCreatedRoom()
    {
        full = false;
        spawned = false;
        GameObject Floor = PhotonNetwork.InstantiateSceneObject(FloorPre.name, new Vector3(0, -0.5f, 0), Quaternion.identity, 0, null);
        Floor.GetComponent<GameManager>().enabled = true;
        GameObject Canvas = PhotonNetwork.InstantiateSceneObject(PointsPre.name, new Vector3(0, 0, 0), Quaternion.identity, 0, null);
        foreach (Text t in Canvas.GetComponentsInChildren<Text>())
        {
            Floor.GetComponent<GameManager>().Toto = t;

        }

        Floor.GetComponent<GameManager>().Toto.text = "";


    }

    void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();

    }

    public GameObject playerPrefab1, playerPrefab2, body2prefab;

    public GameObject Cube;
    void OnJoinedRoom()
    {
        Debug.Log("Woof2");
        playernum = PhotonNetwork.room.playerCount;

        if (PhotonNetwork.room.playerCount == 1)
        {
            GameObject snake1 = PhotonNetwork.Instantiate(playerPrefab1.name, new Vector3(-0.1f, 0.05f, 0), Quaternion.identity, 0);
            Debug.Log(snake1 == null);
            GameObject Floor = null;
            Text points1 = null, points2 = null, Toto = null;
            p1.gameObject.SetActive(true);
            if (MenuScript.gameMode == Mode.Multiplayer_3D)
                snake1.transform.GetChild(0).gameObject.SetActive(true);
            foreach (GameObject o in PhotonView.FindObjectsOfType<GameObject>())
            {

                if (o.tag == "Floor" ||o.name.Contains("Box"))
                    Floor = o;

            }

            foreach (Text o in PhotonView.FindObjectsOfType<Text>())
            {
                if (o.gameObject.tag == "P1")
                    points1 = o;
                if (o.gameObject.tag == "P2")
                    points2 = o;

            }
            if (Floor != null)
            {
                Floor.GetComponent<GameManager>().snake1 = snake1;

                Floor.GetComponent<GameManager>().PointsMul = points2;
                snake1.GetComponent<Snake>().My = points1;
                

                Floor.GetComponent<GameManager>().enabled = false;

            }

        }
       

        if (PhotonNetwork.room.playerCount == 2)
        {
            GameObject snake1;
            p2.gameObject.SetActive(true);
            if (MenuScript.gameMode == Mode.Multiplayer_Normal)
                snake1 = PhotonNetwork.Instantiate(playerPrefab2.name, new Vector3(+0.1f, -0.5f, 0), Quaternion.identity, 0);
            else
            {
                snake1 = PhotonNetwork.Instantiate(playerPrefab2.name, new Vector3(+0.1f, 0.05f, 0), Quaternion.identity, 0);
                snake1.transform.GetChild(0).gameObject.SetActive(true);
            }
            Debug.Log(snake1 == null);
           // GameObject cube = PhotonNetwork.Instantiate(Cube.name, new Vector3(+0.1f, -0.51f, 0), Quaternion.identity, 0);
            //cube.GetComponent<DataSend>().snake = snake1;
            //snake1.GetComponent<Snake>().enabled = true;

        }

        spawned = true;

    }
    void OnJoinedRoomFailed()
    {
        Debug.Log("Can't join room");
    }

    void OnDisconnectRoom()
    {
        Cursor.visible = true;
        Cursor.lockState=CursorLockMode.None;
        p1.gameObject.SetActive(false);
        p2.gameObject.SetActive(false);
        PhotonNetwork.DestroyAll();
        spawned = false;
        full = false;
        if(MenuScript.gameMode==Mode.Multiplayer_Normal)
            Application.LoadLevel("MultiPlayer");
        else
            Application.LoadLevel("MultiPlayer3D");
    }
    void OnDisconnectFromPhoton()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PhotonNetwork.DestroyAll();
        Application.LoadLevel("MainMenu");


    }
    void Update()
    {
        if (PhotonNetwork.room != null)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                PhotonNetwork.LeaveRoom();
                spawned = false;



            }
            else if (PhotonNetwork.room.playerCount == 2)
                full = true;
            else if (full && PhotonNetwork.room.playerCount < 2)
            {
                OnDisconnectRoom();
                PhotonNetwork.LeaveRoom(); spawned = false;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                PhotonNetwork.Disconnect();
                Application.LoadLevel("Main Menu");



            }
        }
        
    }

}
