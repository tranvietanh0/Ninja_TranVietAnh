using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHideMap : MonoBehaviour
{
    [SerializeField] private float timeLoadScene = 2f;
    private string nameScene = "HideMap";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(LoadMap());
        }
    }

    IEnumerator LoadMap()
    {
        yield return new WaitForSeconds(timeLoadScene);
        SceneManager.LoadScene(nameScene);
    }
}
