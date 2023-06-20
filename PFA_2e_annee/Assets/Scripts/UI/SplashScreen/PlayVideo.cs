using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class PlayVideo : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainCamera = null;
    [SerializeField]
    private VideoPlayer _currentVideoPlayer = null;

    [SerializeField]
    private VideoPlayer[] _videoPlayerList;

    private int _currentIndex = 0;


    void Start()
    {
        StartCoroutine(StartSplashscreen());
    }

    public IEnumerator StartSplashscreen()
    {

        yield return new WaitForSeconds(3f);
        _videoPlayerList[0].gameObject.SetActive(false);
        _videoPlayerList[1].gameObject.SetActive(true);

        yield return new WaitForSeconds(4f);
        _videoPlayerList[1].gameObject.SetActive(false);
        _videoPlayerList[2].gameObject.SetActive(true);

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);
        
    }
   
}

