using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Button RestartButton;
    [SerializeField] Button MainMenuButton;
    [SerializeField] Button QuitButton;
    [SerializeField] Text scoreField;

    // Start is called before the first frame update
    void Start()
    {
        RestartButton.GetComponent<Button>().onClick.AddListener(restartGame);
        MainMenuButton.GetComponent<Button>().onClick.AddListener(goMainMenu);
        QuitButton.GetComponent<Button>().onClick.AddListener(goQuit);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        scoreField.text = GameObject.Find("TextScore").GetComponent<Text>().text;
        GameObject.Find("Canvas_n").SetActive(false);
    }

    public void DeleteAll()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Destroy(o);
        }
    }

    // Update is called once per frame
    void restartGame()
    {
        DeleteAll();
        SceneManager.LoadScene("Level_1");
    }

    void goMainMenu()
    {
        DeleteAll();
        SceneManager.LoadScene("MainMenu");
    }

    void goQuit()
    {
        Application.Quit();
    }
}
