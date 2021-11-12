using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player to Follow")]
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0.0f, 24.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(player.transform.position.x, 24.0f, player.transform.position.z);
    }
}
