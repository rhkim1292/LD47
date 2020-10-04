using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float Score;
    public Transform player;
    public Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position;
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AddScore();
    }

    void AddScore()
    {
        Score = Vector2.Distance(player.position, transform.position);
        ScoreText.text = Score.ToString("0");
    }
}
