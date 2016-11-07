using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    static int gmode = 0;
    public int AmApple;
    public static EatValue eat;
    public static float light;
    public static Dictionary<string, KeyCode> Snake1Keys = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> Snake2Keys = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> MultiKeys = new Dictionary<string, KeyCode>();
    public Material S1, S2, snake1col, snake2col, Multi, text1col, text2col;
    public Image Load, Tutorials;
    public Dropdown mode;

    public static Mode gameMode = Mode.Normal;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        mode.value = gmode;
        if (Snake1Keys.Count == 0)
        {
            AmApple = 1;
            light = 1;
            eat = EatValue.GetAll;
            Snake1Keys["Up"] = KeyCode.W;
            Snake1Keys["Left"] = KeyCode.A;
            Snake1Keys["Right"] = KeyCode.D;
            Snake1Keys["Down"] = KeyCode.S;
            MultiKeys["Up"] = KeyCode.UpArrow;
            MultiKeys["Left"] = KeyCode.LeftArrow;
            MultiKeys["Right"] = KeyCode.RightArrow;
            MultiKeys["Down"] = KeyCode.DownArrow;

            Snake2Keys["Up"] = KeyCode.UpArrow;
            Snake2Keys["Left"] = KeyCode.LeftArrow;
            Snake2Keys["Right"] = KeyCode.RightArrow;
            Snake2Keys["Down"] = KeyCode.DownArrow;
        }

        Load.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && Tutorials.gameObject.activeSelf)
            Tutorials.gameObject.SetActive(false);
    }
    public void StartGame()
    {
        Load.gameObject.SetActive(true);
        switch (gameMode)
        {
            case Mode.Normal:
                SceneManager.LoadScene("Game");
                break;
            case Mode.Multiplayer_Normal:
                SceneManager.LoadScene("MultiPlayer");
                break;
            case Mode.Multiplayer_3D:
                SceneManager.LoadScene("MultiPlayer3D");
                break;
        }
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void ChangeMode()
    {
        gmode = mode.value;
        switch(mode.value)
        {
            case 0:
                gameMode = Mode.Normal;
                break;
            case 1:
                gameMode = Mode.Multiplayer_Normal;
                break;
            case 2:
                gameMode = Mode.Multiplayer_3D;
                break;
        }
    }

    public void Exit()
    {
        S1.SetColor("_Color", snake1col.color);
        S2.SetColor("_Color", snake2col.color);
        text1col.SetColor("_Color", snake1col.color);
        text2col.SetColor("_Color", snake2col.color);
        S1.name = "Green Snake";
        S2.name = "Purple Snake";
        Application.Quit();
    }

    public void LoadTutorials()
    {
        Tutorials.gameObject.SetActive(true);
    }
}
public enum EatValue
{
    GetAll,
    GetOne,
    GetNothing
}

public enum Mode
{
    Normal,
    Multiplayer_Normal,
    Multiplayer_3D
}