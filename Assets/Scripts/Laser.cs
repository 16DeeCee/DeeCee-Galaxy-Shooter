using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 8f;

    private bool _isPlayer = true;
    // Start is called before the first frame update
    // void Start()
    // {
        // transform.position = new Vector3(transform.position.x, transform.position.y + 1.05f, 0);
    // }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayer)
            PlayerLaser();
        else
            EnemyLaser();
    }

    private void PlayerLaser()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
                return;
            }

            Destroy(this.gameObject);
        }
    }

    private void EnemyLaser()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -8f)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void ActivateEnemyLaser()
    {
        _isPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isPlayer == false)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
                player.Damage();

            Destroy(gameObject);
        }
    }
}
