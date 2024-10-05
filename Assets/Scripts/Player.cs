using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2f;

    [SerializeField]
    private GameObject _laserPrefab;

    private float _startFire = -1f;
    private float _fireRate = 0.2f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _shieldPrefab;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private GameObject[] _engines;

    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _laserClip;

    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;

    private int _score = 0;

    private NewGameManager _gameManager;
    private UIManager _uiManager;
    private Animator _animator;

    [SerializeField]
    private bool _playerOne, _playerTwo;

    // Start is called before the first frame update
    void Start()
    {
        // transform.position = new Vector3(0, 0, 0);
        _gameManager = GameObject.Find("Game Manager").GetComponent<NewGameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _animator = GetComponent<Animator>();
        _laserClip = GetComponent<AudioSource>();
        _laserClip.clip = _laserSound;

        if (_gameManager == null)
            Debug.LogError("Game Manager is null.");

        if (_uiManager == null)
            Debug.LogError("UI Manager is null.");

        if (_animator == null)
            Debug.LogError("Animator is null.");

        if (_laserClip == null)
            Debug.LogError("Laser clip is null.");
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerOne)
        {
            PlayerMovements("Horizontal Player 1", "Vertical Player 1");

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _startFire)
                ShootLaser();
        }

        if (_playerTwo)
        {
            PlayerMovements("Horizontal Player 2", "Vertical Player 2");

            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > _startFire)
                ShootLaser();
        }

    }

    void PlayerMovements(string xAxis, string yAxis)
    {
        float horizontalInput = Input.GetAxisRaw(xAxis);
        float verticalInput = Input.GetAxisRaw(yAxis);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(_speed * Time.deltaTime * direction);

        _animator.SetBool("turnLeft", horizontalInput < 0);
        _animator.SetBool("turnRight", horizontalInput > 0);

        Vector3 playerPosition = transform.position;

        // clamp to assign a min and max value
        playerPosition.y = Mathf.Clamp(playerPosition.y, -3.8f, 0);

        // optimized
        if (playerPosition.x > 11.2f)
            playerPosition.x = -11.2f;
        else if (playerPosition.x < -11.2f)
            playerPosition.x = 11.2f;

        transform.position = playerPosition;
    }

    void ShootLaser()
    {
        // set a new value of startFire for cooldown of laser and create a new laser game object
        _startFire = Time.time + _fireRate;
        Vector3 laserPos = transform.position + new Vector3(0, 1.05f, 0);

        if (_isTripleShotActive)
        {
            //Vector3 laserPos = transform.position - new Vector3(3.962f, -0.10f, 10.89f);
            Instantiate(_tripleShotPrefab, laserPos, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, laserPos, Quaternion.identity);
        }

        _laserClip.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldPrefab.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.ChangeLivesDisplay(_lives, _playerOne);

        if (_lives <= 0)
        {
            _speed = 0;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            _uiManager.IsGameOver(_score);
        }
        else
        {
            _engines[_lives - 1].SetActive(true);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotDuration());
    }

    IEnumerator TripleShotDuration()
    {
        yield return new WaitForSeconds(5f);

        _isTripleShotActive = false;
    }

    public void ActivateSpeedBoost()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostDuration());
    }

    IEnumerator SpeedBoostDuration()
    {
        yield return new WaitForSeconds(5f);

        _speed /= _speedMultiplier;
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shieldPrefab.SetActive(true);
    }

    public void AddScore(int scorePoints)
    {
        _score += scorePoints;
        _uiManager.ChangeScoreText(_score);
    }
}
