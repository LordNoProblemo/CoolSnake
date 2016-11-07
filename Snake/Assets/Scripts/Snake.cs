using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Snake : MonoBehaviour
{

    public Dictionary<string, KeyCode> MoveKeys;
    public Color SnakeColor;
    public List<GameObject> BodyList = new List<GameObject>();
    public float speed = 0.02f;
    public bool destroyed = false;
    public Text My;
    public Vector3 dire = new Vector3(0, 0, 1);
    public char dir = 'u';
    public float x, z;
    public GameObject body, other;
    public bool eat = false;

    private int am = 0;
    public int count = 0;
    // Use this for initialization
    protected virtual void Start()
    {
        My.color = gameObject.GetComponent<Renderer>().material.color;


    }
    public void UpdateText()
    {
        My.text = count.ToString();
    }


    public void OnCollisionExit(Collision other)
    {

    }
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Apple")
        {

            other.gameObject.active = false;
            eat = true;
        }
        else if (other.gameObject.tag == body.tag)
        {
            for(int i =0;i<BodyList.Count;i++)
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
        else if (other.gameObject == this.other)
        {
            this.other.gameObject.GetComponent<Snake>().enabled = false;

            enabled = false;
        }
        else if (other.gameObject.tag == this.other.GetComponent<Snake>().body.tag)
        {


            other.gameObject.tag = "null";

            UpdateText();
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
    public void Update()
    {



    }
    public void FixedUpdate()
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

    public void MoveHead()
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
}

