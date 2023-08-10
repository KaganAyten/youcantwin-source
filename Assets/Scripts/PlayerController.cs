using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private void OnEnable()
    {
        CollisionHandler.OnPlayerDied += Die;
    }
    private void OnDisable()
    {
        CollisionHandler.OnPlayerDied -= Die;
    }
    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
    }
}
