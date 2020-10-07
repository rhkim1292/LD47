using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    float initialPos;
    public Transform player;
    public Text ScoreText;
    private int totalScore;
    private int currScore;
    private bool restarted;

    [SerializeField]
    private int penalty;

    void Start()
    {
        restarted = false;
        initialPos = player.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (restarted)
        {
            if (totalScore + currScore - penalty < 0)
            {
                totalScore = 0;
            }
            else
            {
                totalScore = totalScore + currScore - penalty;
            }
        }
        else
        {
            currScore = (int)Vector2.Distance(new Vector2(player.position.x, 0), new Vector2(initialPos, 0));
            ScoreText.text = (totalScore+currScore).ToString();
        }
        restarted = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 11)
        {
            restarted = true;
        }
    }
}
