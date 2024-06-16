using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject game;
    public GameObject mainMenu;
    public GameObject loading;
    public GameObject hpBar;
    public GameObject score;

    private void Awake()
    {
        Time.timeScale = 0f;
        mainMenu.SetActive(true);
        loading.SetActive(false);
        game.SetActive(false);
        hpBar.SetActive(false);
        score.SetActive(false);
    }
    public void FromMenuToGame()
    {
        StartCoroutine(EndLoading());
    }
    IEnumerator EndLoading()
    {
        loading.SetActive(true);
        mainMenu.SetActive(false);
        yield return new WaitForSecondsRealtime(3f);
        loading.SetActive(false);
        game.SetActive(true);
        hpBar.SetActive(true);
        score.SetActive(true);
        Time.timeScale = 1f;
    }

}
