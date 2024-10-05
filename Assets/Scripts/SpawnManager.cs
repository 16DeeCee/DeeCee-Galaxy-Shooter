using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerUp;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _startSpawning = true;

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_startSpawning)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9.25f, 9.25f), 7.25f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5f);
        while (_startSpawning)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9.25f, 9.25f), 9f, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUp[randomPowerUp], randomPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void StopSpawning()
    {
        _startSpawning = false;
    }
}
