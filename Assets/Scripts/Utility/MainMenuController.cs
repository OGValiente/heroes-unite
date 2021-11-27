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
        GameStateController.SetGameState(GameState.Idle);
        
        PlayButton.onClick.AddListener(() =>
        {
            Prepare();
            SceneManager.LoadScene(Constants.GameScene);
        });

        QuitButton.onClick.AddListener(Application.Quit);
    }

    private void Prepare()
    {
        for (int i = 0; i < Constants.InitialHeroCount; i++)
        {
            PlayerData.OwnedHeroes.Add(HeroCollection.Heroes[i]);
        }
    }
}
