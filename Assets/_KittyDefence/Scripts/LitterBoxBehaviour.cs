using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LitterBoxBehaviour : MonoBehaviour
{
    public GameObject LosingScreen;
    public GameObject WinningScreen;
    public GameObject WinningGameScreen;
    public TextMeshProUGUI TxtWaveNumber;
    private PoopBehaviour[] _poops;
    private int _currentPoopId = 0;

    // Start is called before the first frame update
    void Start()
    {
        _poops = GetComponentsInChildren<PoopBehaviour>(true);
        LevelController.Instance.OnLivesLost += SpawnPoop;
        LevelController.Instance.OnGameCompleted += ShowWinningMessage;
        LevelController.Instance.OnWaveStart += UpdateWaveInfo;
    }

    private void OnDestroy()
    {
        LevelController.Instance.OnLivesLost -= SpawnPoop;
        LevelController.Instance.OnGameCompleted -= ShowWinningMessage;
        LevelController.Instance.OnWaveStart -= UpdateWaveInfo;
    }

    private void UpdateWaveInfo(int waveNo)
    {
        TxtWaveNumber.text = (waveNo + 1).ToString();
    }

    private void ShowWinningMessage(bool levelWon, bool gameWon)
    {
        if (levelWon && gameWon)
            WinningGameScreen.SetActive(true);
        else if (levelWon)
            WinningScreen.SetActive(true);
        else
            LosingScreen.SetActive(true);
    }

    public void Restart()
    {
        LevelController.Instance.StartLevel();
    }

    private void SpawnPoop(int currentLives)
    {
        Debug.Log("Spawning poop");
        if (_currentPoopId >= _poops.Length)
            return;
        _poops[_currentPoopId].gameObject.SetActive(true);
        _currentPoopId++;
    }

}
