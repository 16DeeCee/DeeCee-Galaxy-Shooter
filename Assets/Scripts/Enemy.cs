using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4f;
    private Player _playerOne, _playerTwo;

    private Animator _animator;

    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _explosionClip;

    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate;
    private float _canFire = -1f;

    private NewGameManager _gameManager;
    

    // Start is called before the first frame update
    void Start()
    {
        // RandomPosition();
        GameObject _playerOneGameObject = GameObject.Find("Player One");
        _playerOne = _playerOneGameObject ? _playerOneGameObject.GetComponent<Player>() : null;
        _gameManager = GameObject.Find("Game Manager").GetComponent<NewGameManager>();
        _animator = gameObject.GetComponent<Animator>();
        _explosionClip = GetComponent<AudioSource>();
        _explosionClip.clip = _explosionSound;

        if (_playerOne == null)
            Debug.LogError("Player is null.");

        if (_gameManager == null)
            Debug.LogError("Game Manager is null.");

        if (_animator == null)
            Debug.LogError("Animator is null.");

        if (_explosionClip == null)
            Debug.LogError("Audio Source is null.");

        if (_gameManager != null && _gameManager.isCoOpMode)
        {
            GameObject _playerTwoGameObject = GameObject.Find("Player Two");
            _playerTwo = _playerTwoGameObject ? _playerTwoGameObject.GetComponent<Player>() : null;
        }
    }

    // Update is called once per frame

    void Update()
    {
        EnemyMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1f, 3f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position - new Vector3(0, 1.5f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].ActivateEnemyLaser();
            }

            // Debug.Break();
        }
    }

    private void EnemyMovement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -5.4f)
        {
            RandomPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _speed = 0;
            _animator.SetTrigger("enemyDestroyed");

            Debug.Log("You hit me!");

            Player player = other.GetComponent<Player>();
            if (player != null)
                player.Damage();

            _explosionClip.Play();
            gameObject.GetComponent<Collider2D>().enabled = false;
            _canFire += 3f;
            Destroy(gameObject, 2.5f);
        }
        
        // Destroy the enemy and laser when collision occurs
        if (other.CompareTag("Laser"))
        {
            _speed = 0;
            _animator.SetTrigger("enemyDestroyed");

            Debug.Log("I hit you!");

            _explosionClip.Play();
            gameObject.GetComponent<Collider2D>().enabled = false;
            _canFire += 3f;
            Destroy(gameObject, 2.5f);

            if (_playerOne != null)
                _playerOne.AddScore(10);

            if (_playerTwo != null)
                _playerTwo.AddScore(10);

            Destroy(other.gameObject);
        }
    }

    void RandomPosition()
    {
        float randomX = Random.Range(-9.25f, 9.25f);
        transform.position = new Vector3(randomX, 7.25f, 0);
    }
}
