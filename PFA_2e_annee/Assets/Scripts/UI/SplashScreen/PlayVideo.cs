using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayVideo : MonoBehaviour
{

    [SerializeField]
    private VideoPlayer[] _videoPlayerList;
    [SerializeField]
    private GDTFadeEffect _fadeToBlack = null;

    private float _initialTime = 5f;
    private float _initialDelay = 2f;

    void Start()
    {
        StartCoroutine(StartSplashscreen());
    }

    public IEnumerator StartSplashscreen()
    {
        //freeze
        yield return new WaitForSeconds(3f);
        _videoPlayerList[0].gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        _fadeToBlack.gameObject.SetActive(false);
        _fadeToBlack.firstToLast = true;
        _fadeToBlack.timeEffect = 1f;
        _fadeToBlack.initialDelay = 0f;

        
        yield return new WaitForSeconds(2f);
        _fadeToBlack.gameObject.SetActive(true);
        
        //Drops
        yield return new WaitForSeconds(1.5f);
        _fadeToBlack.gameObject.SetActive(false);
        _videoPlayerList[0].gameObject.SetActive(false);
        _videoPlayerList[1].gameObject.SetActive(true);

        yield return new WaitForSeconds(7f);
        _fadeToBlack.gameObject.SetActive(true);

        //Gas
        yield return new WaitForSeconds(1.5f);
        _fadeToBlack.gameObject.SetActive(false);
        _fadeToBlack.firstToLast = false;
        _fadeToBlack.timeEffect = 3f;
        _videoPlayerList[1].gameObject.SetActive(false);


        yield return new WaitForSeconds(2f);
        _fadeToBlack.gameObject.SetActive(true);
        _videoPlayerList[2].gameObject.SetActive(true);
        _videoPlayerList[2].Play();

        yield return new WaitForSeconds(0.1f);
        _videoPlayerList[2].Pause();

        yield return new WaitForSeconds(2f);
        _videoPlayerList[2].Play();


        yield return new WaitForSeconds(2f);
        _fadeToBlack.gameObject.SetActive(false);
        _fadeToBlack.firstToLast = true;
        _fadeToBlack.timeEffect = 1f;


        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);

    }
   
}

