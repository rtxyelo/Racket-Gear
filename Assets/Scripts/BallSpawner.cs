using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _ballsPrefabs = new(4);    

    [SerializeField]
    private List<Transform> _spawnPositions = new(9);

    private bool SpawnAllow = true;

    private float _spawnTime = 1.2f;

    private GameController _gameController;

    private HitSoundBehaviour _hitSoundBehaviour;

    private void Awake()
    {
        _hitSoundBehaviour = FindObjectOfType<HitSoundBehaviour>();
        _gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        _gameController.IsGameOver.AddListener(GameOver);
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (SpawnAllow)
        {
            int spawnIndex = Random.Range(0, _spawnPositions.Count);
            int baloonIndex = Random.Range(0, _ballsPrefabs.Count);

            GameObject ball = Instantiate(_ballsPrefabs[baloonIndex], _spawnPositions[spawnIndex].transform.position, Quaternion.identity, transform);
            //ball.GetComponent<BallController>().IsBallHit.AddListener(BallBurst);
            yield return new WaitForSeconds(_spawnTime);
        }
    }

    //private void BallBurst()
    //{
    //    //Debug.Log("Ball Burst!");
    //}

    private void GameOver()
    {
        SpawnAllow = false;

        gameObject.SetActive(false);
    }
}
