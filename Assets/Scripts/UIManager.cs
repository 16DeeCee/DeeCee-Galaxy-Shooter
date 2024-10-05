using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestScoreText;

    [SerializeField]
    private Text _gameoverText;
    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Image _livesDisplayPlayerOne;

    [SerializeField]
    private Image _livesDisplayPlayerTwo;

    private int _playerOneLives = 3, _playerTwoLives = 3;

    private NewGameManager _gameManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _pausePanel;

    private Coroutine gameOverFlicker;

    private int _totalScore;
    private int _highScore;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (_bestScoreText)
            _bestScoreText.text = "Best: " + _highScore.ToString();
        _gameManager = GameObject.Find("Game Manager").GetComponent<NewGameManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        
        if (_spawnManager == null)
            Debug.LogError("Spawn Manager is null.");

        if (_gameManager == null)
            Debug.LogError("Game Manager is null");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPanelActive = _pausePanel.activeSelf;

            if (isPanelActive)
                ResumeGame();
            else
                PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 0)
            Application.Quit();

    }

    public void ChangeScoreText(int score)
    {
        _totalScore = score;
        _scoreText.text = "Score: " + _totalScore.ToString(); 
    }

    private void CheckBestScore(int score)
    {
        if (score > _highScore)
            PlayerPrefs.SetInt("HighScore", score);

        _bestScoreText.text = "Best: " + score.ToString();

    }

    public void ChangeLivesDisplay(int playerLives, bool playerOne)
    {
        if (playerLives >= 0 && playerOne)
        {
            _playerOneLives = playerLives;
            _livesDisplayPlayerOne.sprite = _livesSprites[playerLives];
        } 
        else if (playerLives >= 0 && playerOne == false)
        {
            _playerTwoLives = playerLives;
            _livesDisplayPlayerTwo.sprite = _livesSprites[playerLives];
        }
    }

    private void DisplayGameOver()
    {
        // _gameoverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        //_gameoverText.gameObject.SetActive(true);

        _gameManager.SetGameOver();

        if (gameOverFlicker == null)
            gameOverFlicker = StartCoroutine(FlickeringGameOver());
    }

    public void IsGameOver(int score)
    {
        if (_gameManager.isCoOpMode && _playerOneLives < 1 && _playerTwoLives < 1)
        {
            _spawnManager.StopSpawning();
            DisplayGameOver();
        }
        else if (_gameManager.isCoOpMode == false && _playerOneLives < 1)
        {
            _spawnManager.StopSpawning();
            DisplayGameOver();
            CheckBestScore(score);
        }          
    }

    private void PauseGame()
    {
        _pausePanel.SetActive(true);
        _pausePanel.GetComponent<Animator>().SetBool("isPaused", true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _pausePanel.SetActive(false);
        _pausePanel.GetComponent<Animator>().SetBool("isPaused", false);
        Time.timeScale = 1;
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    IEnumerator FlickeringGameOver()
    {
        while (true)
        {
            _gameoverText.gameObject.SetActive(!_gameoverText.IsActive());

            yield return new WaitForSeconds(1f);
        }

    }
}
