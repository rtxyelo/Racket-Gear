using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOverPanel;

    public int GameTime { get => (int)_gameSessionTime; }

    private float _gameSessionTime = 180f;

    [HideInInspector]
    public UnityEvent IsGameOver;

    private ScoreCounter _scoreCounter;

    [SerializeField]
    private InputController _inputController;

    [SerializeField]
    private BallsBorder _ballsBorder;

    public void Awake()
    {
        IsGameOver = new UnityEvent();
        
        _scoreCounter = FindObjectOfType<ScoreCounter>();

        _ballsBorder.GameLose.AddListener(GameOver);
    }

    private void GameOver()
    {
        //Debug.Log("Game over!");

        IsGameOver.Invoke();

        _inputController.IsActive = false;

        _gameOverPanel.SetActive(true);
    }

    private void Update()
    {
        _gameSessionTime -= Time.deltaTime;

        if (_gameSessionTime < 0 )
        {
            GameOver();
        }
    }

}
