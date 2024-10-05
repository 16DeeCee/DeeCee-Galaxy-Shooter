using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _powerupID;

    // [SerializeField]
    // private AudioClip _powerupClip;
    [SerializeField]
    private AudioSource _powerupClip;

    // Start is called before the first frame update
    void Start()
    {
        _powerupClip = GameObject.Find("Powerup Sound").GetComponent<AudioSource>();

        if (_powerupClip == null)
            Debug.LogError("Powerup Sound is null.");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -7.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    default:
                        Debug.Log("Default");
                        break;
                }
            }

            //AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
            _powerupClip.Play();
            Destroy(this.gameObject);
        }
    }
}
