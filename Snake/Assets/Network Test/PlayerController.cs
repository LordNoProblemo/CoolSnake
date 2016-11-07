using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    // Use this for initialization
    void Start () {
	
	}
    public override void OnStartLocalPlayer()
    {

        GetComponent<Renderer>().material.color = Color.blue;
    }
    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f, 0);
        transform.Translate(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * 3.0f);
	}
    void Fire()
    {
        // Create the Bullet from the Bullet Prefab
        GameObject bullet = (GameObject)Instantiate(bulletPrefab,bulletSpawn.position,bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
