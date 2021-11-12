using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;
    private float startTime;
    private float timeTaken;
    private int collectiblesPicked = 0;
    public int maxCollectibles = 10;
    private bool isPlaying;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;
    public TextMeshProUGUI candlesCollected;


    void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        candlesCollected.text = "Candles Collected: " + collectiblesPicked + "/" + maxCollectibles;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
            return;

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;
        //rig.velocity = new Vector3(x, rig.velocity.y, z);
        rig.velocity = new Vector3(x, rig.velocity.y, z);

        Vector3 movementDirection = new Vector3(x, 0, z);
        movementDirection.Normalize();

        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        curTimeText.text = (Time.time - startTime).ToString("F2");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            collectiblesPicked++;
            Destroy(other.gameObject);
            candlesCollected.text = "Candles Collected: " + collectiblesPicked + "/" + maxCollectibles;
            if (collectiblesPicked == maxCollectibles)
                End();
        }
    }

    public void Begin()
    {
        startTime = Time.time;
        isPlaying = true;

        playButton.SetActive(false);
    }

    void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        Debug.Log("End Function");
        Debug.Log(Leaderboard.instance);
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
    }

    public void OnResetClicked()
    {
        SceneManager.LoadScene(1);
    }
}
