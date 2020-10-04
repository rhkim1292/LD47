using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    float initialPos;
    public Transform player;
    public Text ScoreText;
    public int Score;
    private bool restarted;

    void Start()
    {
        restarted = false;
        initialPos = player.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Score = (int)Vector2.Distance(new Vector2(player.position.x, 0), new Vector2(initialPos, 0));
        ScoreText.text = Score.ToString();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.layer);
        if (col.gameObject.layer == 11)
        {
            restarted = true;
        }
    }
    /*void AddScore()
    {
        Score = Vector2.Distance(player.position, transform.position);
        ScoreText.text = Score.ToString("0");
    }*/
}
