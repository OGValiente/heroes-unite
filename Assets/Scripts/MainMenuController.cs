using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button PlayButton;
    public Button QuitButton;
    public HeroCollectionSO HeroCollection;

    private void Start()
    {
        PlayButton.onClick.AddListener(() =>
        {
            Prepare();
            SceneManager.LoadScene("Game");
        });

        QuitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void Prepare()
    {
        // Get first three heroes
        for (int i = 0; i < 3; i++)
        {
            PlayerData.OwnedHeroes.Add(HeroCollection.Heroes[i]);
        }
    }
}
