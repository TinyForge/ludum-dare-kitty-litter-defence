using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    private Vector3 _startingPosition;
    private bool _hasStarted = false;
    private bool _showingMenu = false;

    public void StartLevel(int lives)
    {
        if (_hasStarted)
            return;
        _hasStarted = true;
        _startingPosition = transform.position;
        LevelController.Instance.TotalLives = lives;
        LevelController.Instance.StartLevel();

        transform.DOMoveY(_startingPosition.y - 20f, 0.5f).SetEase(Ease.InBack);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _hasStarted)
        {
            LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        DOTween.CompleteAll(transform);
        if (!_showingMenu)
        {
            PauseMenu.SetActive(true);
            PauseMenu.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f);
            Time.timeScale = 0f;
        }
        else
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        _showingMenu = !_showingMenu;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
