using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	[SerializeField]
	private Button playButton;
	[SerializeField]
	private Button quitButton;
	[SerializeField]
	private HeroCollectionSO heroCollection;

    private void Start()
    {
        GameStateController.SetGameState(GameState.Idle);
        
        playButton.onClick.AddListener(() =>
        {
            Prepare();
            SceneManager.LoadScene(Constants.GameScene);
        });

        quitButton.onClick.AddListener(Application.Quit);
    }

    private void Prepare()
    {
        for (int i = 0; i < Constants.InitialHeroCount; i++)
		{
			var hero = heroCollection.Heroes[i];
            PlayerData.OwnedHeroes.Add(hero);
			PlayerData.OwnedHeroIds.Add(hero.Id);
		}
    }
}
