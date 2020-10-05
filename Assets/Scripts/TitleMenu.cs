using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] Button StartButton;
    [SerializeField] Button QuitButton;
    [SerializeField] Button CreditsButton;
    [SerializeField] Button BackButton;

    [SerializeField] GameObject credits;


    // Start is called before the first frame update
    void Start()
    {
        StartButton.GetComponent<Button>().onClick.AddListener(startGame);
        QuitButton.GetComponent<Button>().onClick.AddListener(goQuit);
        CreditsButton.GetComponent<Button>().onClick.AddListener(showCredits);
        BackButton.GetComponent<Button>().onClick.AddListener(backMenu);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
    }

    void startGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    void goQuit()
    {
        Application.Quit();
    }

    void showCredits()
    {
        StartButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);
        CreditsButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(true);

        credits.SetActive(true);

    }

    void backMenu()
    {
        StartButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);
        CreditsButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(false);

        credits.SetActive(false);
    }
}
