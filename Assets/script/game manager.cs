using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPre;
    [SerializeField] private GameObject _cmsPre;

    [SerializeField] private float _timeMin;
    [SerializeField] private float _timeMax;

    [SerializeField] private Text _scoreUI;
    private float _score = 0;

    private GameObject _playerInstance;

    [SerializeField] private float _scoreIncreaseInterval = 0.1f;
    private float _scoreTimer = 0f;

    [SerializeField] private Vector3 _spawnPositionCMS;

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _replayButton;
    [SerializeField] private Text _gameOverText;

    [SerializeField] private AudioSource _backgroundMusic;

    private Coroutine _spawnCMSCoroutine;

    void Start()
    {
        _startButton.onClick.AddListener(StartGame);
        _replayButton.onClick.AddListener(ReplayGame);
        _replayButton.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        Time.timeScale = 0;
        _backgroundMusic.Play();
    }

    void Update()
    {
        if (_playerInstance != null)
        {
            bool checkPlayerDead = _playerInstance.GetComponent<Player>().IsDead();
            if (checkPlayerDead)
            {
                Time.timeScale = 0;
                _gameOverText.gameObject.SetActive(true);
                _replayButton.gameObject.SetActive(true);

                if (_spawnCMSCoroutine != null)
                {
                    StopCoroutine(_spawnCMSCoroutine);
                    _spawnCMSCoroutine = null;
                }
            }
            else
            {
                _scoreTimer += Time.deltaTime;

                if (_scoreTimer >= _scoreIncreaseInterval)
                {
                    _score++;
                    _scoreUI.text = "Score: " + _score.ToString();
                    _scoreTimer = 0f;
                }
            }
        }
    }

    void SpawnCMS()
    {
        Instantiate(_cmsPre, _spawnPositionCMS, Quaternion.identity);
    }

    private IEnumerator SpawnCMSCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_timeMin, _timeMax));
            SpawnCMS();
        }
    }

    private void StartGame()
    {
        foreach (GameObject cms in GameObject.FindGameObjectsWithTag("CMS"))
        {
            Destroy(cms);
        }

        _playerInstance = Instantiate(_playerPre, new Vector3(-4.3f, -1.5f, -1f), Quaternion.identity);
        _score = 0;
        _scoreUI.text = "Score: " + _score.ToString();
        _startButton.gameObject.SetActive(false);
        Time.timeScale = 1;

        if (_spawnCMSCoroutine != null)
        {
            StopCoroutine(_spawnCMSCoroutine);
        }

        _spawnCMSCoroutine = StartCoroutine(SpawnCMSCoroutine());
    }

    private void ReplayGame()
    {
        foreach (GameObject cms in GameObject.FindGameObjectsWithTag("CMS"))
        {
            Destroy(cms);
        }

        if (_playerInstance != null)
        {
            Destroy(_playerInstance);
        }

        Time.timeScale = 1;
        _replayButton.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _score = 0;
        _scoreUI.text = "Score: " + _score.ToString();

        StartGame();
    }
}

