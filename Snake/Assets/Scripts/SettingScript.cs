using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class SettingScript : MonoBehaviour
{
    public Image Fill, Handle;
    public float MaxLight = 2;
    public bool mulset = false;
    public Canvas other, mul;
    bool inChange = false;
    private List<KeyCode> NotAllowed = new List<KeyCode>();
    public Text T1, T2;
    public Material snake1col;
    public Material snake2col;
    public Material text1col;
    public Material text2col;
    public Material S1, S2;
    public Button[] Buttons1, Buttons2;
    public Button[] MoveKeysButtons1, MoveKeysButtons2;
    public Material[] colors;
    public Button None, One, All;
    // Use this for initialization
    void Start()
    {

        if (None != null)
        {
            switch (MenuScript.eat)
            {
                case (EatValue.GetAll):
                    None.GetComponent<Image>().color = new Color(None.GetComponent<Image>().color.r, None.GetComponent<Image>().color.g, None.GetComponent<Image>().color.b);
                    One.GetComponent<Image>().color = new Color(One.GetComponent<Image>().color.r, One.GetComponent<Image>().color.g, One.GetComponent<Image>().color.b);
                    All.GetComponent<Image>().color = new Color(All.GetComponent<Image>().color.r, All.GetComponent<Image>().color.g, All.GetComponent<Image>().color.b, 130 / 255f);
                    break;
                case EatValue.GetOne:
                    None.GetComponent<Image>().color = new Color(None.GetComponent<Image>().color.r, None.GetComponent<Image>().color.g, None.GetComponent<Image>().color.b);
                    One.GetComponent<Image>().color = new Color(One.GetComponent<Image>().color.r, One.GetComponent<Image>().color.g, One.GetComponent<Image>().color.b, 130 / 255f);
                    All.GetComponent<Image>().color = new Color(All.GetComponent<Image>().color.r, All.GetComponent<Image>().color.g, All.GetComponent<Image>().color.b);
                    break;
                case EatValue.GetNothing:
                    None.GetComponent<Image>().color = new Color(None.GetComponent<Image>().color.r, None.GetComponent<Image>().color.g, None.GetComponent<Image>().color.b, 130 / 255f);
                    One.GetComponent<Image>().color = new Color(One.GetComponent<Image>().color.r, One.GetComponent<Image>().color.g, One.GetComponent<Image>().color.b);
                    All.GetComponent<Image>().color = new Color(All.GetComponent<Image>().color.r, All.GetComponent<Image>().color.g, All.GetComponent<Image>().color.b);
                    break;
            }
        }
        if (gameObject.GetComponentInChildren<Slider>() != null)
        {
            gameObject.GetComponentInChildren<Slider>().value = MenuScript.light / MaxLight;
            Fill.color = new Color((90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f, (90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f, (90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f);
            Handle.color = new Color((90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f, (90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f, (90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f);
        }
        NotAllowed.Add(KeyCode.Escape);
        NotAllowed.Add(KeyCode.P);
        NotAllowed.Add(KeyCode.R);
        NotAllowed.Add(KeyCode.Tab);
        NotAllowed.Add(KeyCode.Underscore);
        NotAllowed.Add(KeyCode.Backspace);
        NotAllowed.Add(KeyCode.CapsLock);
        NotAllowed.Add(KeyCode.Equals);
        NotAllowed.Add(KeyCode.Delete);
        NotAllowed.Add(KeyCode.Home);
        NotAllowed.Add(KeyCode.End);
        NotAllowed.Add(KeyCode.Insert);
        NotAllowed.Add(KeyCode.F10);
        NotAllowed.Add(KeyCode.F11);
        NotAllowed.Add(KeyCode.F12);
        NotAllowed.Add(KeyCode.F9);
        NotAllowed.Add(KeyCode.F8);
        NotAllowed.Add(KeyCode.F7);
        NotAllowed.Add(KeyCode.F6);
        NotAllowed.Add(KeyCode.F5);
        NotAllowed.Add(KeyCode.F4);
        NotAllowed.Add(KeyCode.F2);
        NotAllowed.Add(KeyCode.F3);
        NotAllowed.Add(KeyCode.F1);

        for (int i = 0; i < MoveKeysButtons1.Length; i++)
        {
            MoveKeysButtons1[i].GetComponentInChildren<Text>().text = MenuScript.Snake1Keys[MoveKeysButtons1[i].name].ToString();
            if(mulset)
                MoveKeysButtons1[i].GetComponentInChildren<Text>().text = MenuScript.MultiKeys[MoveKeysButtons1[i].name].ToString();

        }
        for (int i = 0; i < MoveKeysButtons2.Length; i++)
            MoveKeysButtons2[i].GetComponentInChildren<Text>().text = MenuScript.Snake2Keys[MoveKeysButtons2[i].name].ToString();

        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].color == S1.color)
            {
                Press(Buttons1[i], colors[i]);
                snake1col = colors[i];
            }
            if (colors[i].color == S2.color)
            {
                Press(Buttons2[i], colors[i]);
                snake2col = colors[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Settings1")
        {
            S1.SetColor("_Color", snake1col.color);
            S2.SetColor("_Color", snake2col.color);
            text1col.SetColor("_Color", snake1col.color);
            text2col.SetColor("_Color", snake2col.color);
        }
        if (Input.GetKey(KeyCode.Escape) && !inChange)
        {

            SceneManager.LoadScene("Main Menu");
        }

    }

    public void Snake1Color(Material RGB)
    {


        if (RGB != snake2col && RGB != snake1col)
        {
            snake1col = RGB;
            ChangeHighlight(1, Buttons1);
        }
        Debug.Log(snake1col + "");
    }

    public void Snake2Color(Material RGB)
    {
        if (RGB != snake1col && RGB != snake2col)
        {
            snake2col = RGB;
            ChangeHighlight(2, Buttons2);
        }
        Debug.Log(snake1col + "");
    }


    private void ChangeHighlight(int snake, Button[] rest)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if ((colors[i] == snake1col && snake == 1) || (colors[i] == snake2col && snake == 2))
            {
                Press(rest[i], colors[i]);
                if (snake == 1)
                    S1.name = colors[i].name + " Snake";
                if (snake == 2)
                    S2.name = colors[i].name + " Snake";
            }
            else
                Unpress(rest[i], colors[i]);
        }

    }

    private void Unpress(Button b, Material m)
    {
        Color col = m.color;
        b.GetComponent<Image>().color = new Color(col.r, col.g, col.b, 1);
    }
    private void Press(Button b, Material m)
    {
        Color col = m.color;
        b.GetComponent<Image>().color = new Color(col.r, col.g, col.b, 130f / 255f);
    }
    public void Switch1(string dir)
    {
        if (inChange)
            return;
        inChange = true;
        StartCoroutine(WaitInput(dir));
    }
    public void ChangeSettings()
    {
        other.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public IEnumerator WaitInput(string dir)
    {
        if (dir.Split(' ')[dir.Split(' ').Length - 1] == "1")
        {
            for (int i = 0; i < MoveKeysButtons1.Length; i++)
            {
                if (dir.Split(' ')[0] == MoveKeysButtons1[i].name)
                {
                    MoveKeysButtons1[i].GetComponent<Image>().color = new Color(MoveKeysButtons1[i].GetComponent<Image>().color.r, MoveKeysButtons1[i].GetComponent<Image>().color.g, MoveKeysButtons1[i].GetComponent<Image>().color.b, 130f / 255);
                }
            }
        }
        if (dir.Split(' ')[dir.Split(' ').Length - 1] == "2")
        {
            for (int i = 0; i < MoveKeysButtons2.Length; i++)
            {
                if (dir.Split(' ')[0] == MoveKeysButtons2[i].name)
                {
                    MoveKeysButtons2[i].GetComponent<Image>().color = new Color(MoveKeysButtons2[i].GetComponent<Image>().color.r, MoveKeysButtons2[i].GetComponent<Image>().color.g, MoveKeysButtons2[i].GetComponent<Image>().color.b, 130f / 255);
                }
            }
        }
        if (dir.Split(' ')[dir.Split(' ').Length - 1] == "M")
        {
            for (int i = 0; i < MoveKeysButtons2.Length; i++)
            {
                if (dir.Split(' ')[0] == MoveKeysButtons1[i].name)
                {
                    MoveKeysButtons1[i].GetComponent<Image>().color = new Color(MoveKeysButtons1[i].GetComponent<Image>().color.r, MoveKeysButtons1[i].GetComponent<Image>().color.g, MoveKeysButtons1[i].GetComponent<Image>().color.b, 130f / 255);
                }
            }
        }

        while (!Input.anyKey)
        {
            yield return null;
        }
        string key = Input.inputString;
        KeyCode k = KeyCode.Escape;
        if (key != null && key != "" && key != "=" && key != "-" && key != "]" && key != "[" && key != "0" && key != "1" && key != "2" && key != "3" && key != "4" && key != "5" && key != "6" && key != "7" && key != "8" && key != "9" && key != "`" && key != KeyCode.Backspace.ToString() && key != "," && key != "." && key != "/" && key != "\\")
            k = (KeyCode)Enum.Parse(typeof(KeyCode), key, true);

        Debug.Log(k.ToString());
        List<KeyCode> Not = new List<KeyCode>(NotAllowed);
        Not.AddRange(MenuScript.Snake1Keys.Values);
        Not.AddRange(MenuScript.Snake2Keys.Values);
        if (Not.Contains(k) == false)
        {
            if (dir.Split(' ')[dir.Split(' ').Length - 1] == "1")
                MenuScript.Snake1Keys[dir.Split(' ')[0]] = k;
            if (dir.Split(' ')[dir.Split(' ').Length - 1] == "2")
                MenuScript.Snake2Keys[dir.Split(' ')[0]] = k;

        }
        if (!NotAllowed.Contains(k))
            if (dir.Split(' ')[dir.Split(' ').Length - 1] == "M")
                MenuScript.MultiKeys[dir.Split(' ')[0]] = k;

        for (int i = 0; i < MoveKeysButtons1.Length; i++)
        {

            MoveKeysButtons1[i].GetComponentInChildren<Text>().text = MenuScript.Snake1Keys[MoveKeysButtons1[i].name].ToString();
            if (dir.Split(' ')[dir.Split(' ').Length - 1] == "M")
                MoveKeysButtons1[i].GetComponentInChildren<Text>().text = MenuScript.MultiKeys[MoveKeysButtons1[i].name].ToString();

        }
        for (int i = 0; i < MoveKeysButtons2.Length; i++)
            MoveKeysButtons2[i].GetComponentInChildren<Text>().text = MenuScript.Snake2Keys[MoveKeysButtons2[i].name].ToString();
        inChange = false;
        if (dir.Split(' ')[dir.Split(' ').Length - 1] == "1")
        {
            for (int i = 0; i < MoveKeysButtons1.Length; i++)
            {
                if (dir.Split(' ')[0] == MoveKeysButtons1[i].name)
                {
                    MoveKeysButtons1[i].GetComponent<Image>().color = new Color(MoveKeysButtons1[i].GetComponent<Image>().color.r, MoveKeysButtons1[i].GetComponent<Image>().color.g, MoveKeysButtons1[i].GetComponent<Image>().color.b, 255);
                }
            }
        }
        if (dir.Split(' ')[dir.Split(' ').Length - 1] == "2")
        {
            for (int i = 0; i < MoveKeysButtons2.Length; i++)
            {
                if (dir.Split(' ')[0] == MoveKeysButtons2[i].name)
                {
                    MoveKeysButtons2[i].GetComponent<Image>().color = new Color(MoveKeysButtons2[i].GetComponent<Image>().color.r, MoveKeysButtons2[i].GetComponent<Image>().color.g, MoveKeysButtons2[i].GetComponent<Image>().color.b, 255);
                }
            }
        }
        if (dir.Split(' ')[dir.Split(' ').Length - 1] == "M")
        {
            for (int i = 0; i < MoveKeysButtons1.Length; i++)
            {
                if (dir.Split(' ')[0] == MoveKeysButtons1[i].name)
                {
                    MoveKeysButtons1[i].GetComponent<Image>().color = new Color(MoveKeysButtons1[i].GetComponent<Image>().color.r, MoveKeysButtons1[i].GetComponent<Image>().color.g, MoveKeysButtons1[i].GetComponent<Image>().color.b, 255);
                }
            }
        }
    }

    public void ChangeLight()
    {

        MenuScript.light = gameObject.GetComponentInChildren<Slider>().value * MaxLight;
        GameObject.Find("Directional Light").GetComponent<Light>().intensity = MenuScript.light;
        Fill.color = new Color((90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f, (90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f, (90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f);
        Handle.color = new Color((90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f, (90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f, (90 + 110 * gameObject.GetComponentInChildren<Slider>().value) / 255f);
    }

    public void ChangeEat(int to)
    {
        if (None != null && One != null && All != null)
        {
            if (to == 2)
            {
                this.None.GetComponent<Image>().color = new Color(None.GetComponent<Image>().color.r, None.GetComponent<Image>().color.g, None.GetComponent<Image>().color.b, 1);
                this.One.GetComponent<Image>().color = new Color(One.GetComponent<Image>().color.r, One.GetComponent<Image>().color.g, One.GetComponent<Image>().color.b, 1);
                this.All.GetComponent<Image>().color = new Color(All.GetComponent<Image>().color.r, All.GetComponent<Image>().color.g, All.GetComponent<Image>().color.b, 130 / 255f);
                MenuScript.eat = EatValue.GetAll;
                Debug.Log("2");
            }

            else if (to == 1)
            {
                this.None.GetComponent<Image>().color = new Color(None.GetComponent<Image>().color.r, None.GetComponent<Image>().color.g, None.GetComponent<Image>().color.b, 1);
                this.One.GetComponent<Image>().color = new Color(One.GetComponent<Image>().color.r, One.GetComponent<Image>().color.g, One.GetComponent<Image>().color.b, 130 / 255f);
                this.All.GetComponent<Image>().color = new Color(All.GetComponent<Image>().color.r, All.GetComponent<Image>().color.g, All.GetComponent<Image>().color.b, 1);
                MenuScript.eat = EatValue.GetOne;
                Debug.Log("1");
            }

            else if (to == 0)
            {
                this.None.GetComponent<Image>().color = new Color(None.GetComponent<Image>().color.r, None.GetComponent<Image>().color.g, None.GetComponent<Image>().color.b, 130 / 255f);
                this.One.GetComponent<Image>().color = new Color(One.GetComponent<Image>().color.r, One.GetComponent<Image>().color.g, One.GetComponent<Image>().color.b, 1);
                this.All.GetComponent<Image>().color = new Color(All.GetComponent<Image>().color.r, All.GetComponent<Image>().color.g, All.GetComponent<Image>().color.b, 1);
                MenuScript.eat = EatValue.GetNothing;
                Debug.Log("0");
            }

        }

    }
    public void ChangeSettings2()
    {
        mul.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
