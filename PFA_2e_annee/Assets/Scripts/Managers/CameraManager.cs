using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField][ReadOnlyInspector] private Camera _mainCam;
    [SerializeField] private List<CinemachineVirtualCameraBase> _battleCameras = new List<CinemachineVirtualCameraBase>();
    [SerializeField] private List<CinemachineVirtualCameraBase> _explorationCameras = new List<CinemachineVirtualCameraBase>();
    [ReadOnlyInspector] public CinemachineVirtualCameraBase CurrentCamera;

    private IEnumerator _currentCameraCoroutine;
    public List<CinemachineVirtualCameraBase> BattleCameras
    {
        get
        {
            return _battleCameras;
        }
    }
    public List<CinemachineVirtualCameraBase> ExplorationCameras
    {
        get
        {
            return _explorationCameras;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        _mainCam = Camera.main;
    }

    public void SetCamera(CinemachineVirtualCameraBase camera)
    {
        if (CurrentCamera) CurrentCamera.enabled = false;
        camera.enabled = false;
        CurrentCamera = camera;
        foreach (CinemachineVirtualCameraBase Vcamera in _battleCameras)
        {
            Vcamera.enabled = false;
        }
        foreach (CinemachineVirtualCameraBase Vcamera in _explorationCameras)
        {
            Vcamera.enabled = false;
        }
        CurrentCamera.enabled = true;
    }

    public void SetCameraField(List<CinemachineVirtualCameraBase> cameraField)
    {
        CurrentCamera.enabled = false;
        CurrentCamera = cameraField[0];
        foreach (CinemachineVirtualCameraBase Vcamera in _battleCameras)
        {
            Vcamera.enabled = false;
        }
        foreach (CinemachineVirtualCameraBase Vcamera in _explorationCameras)
        {
            Vcamera.enabled = false;
        }

        foreach (CinemachineVirtualCameraBase Vcamera in cameraField)
        {
            Vcamera.enabled = true;
        }
        CurrentCamera.enabled = true;
    }

    public void SmoothCurrentCameraFov(float from, float to, float overTime, Action onTransitionCompleted)
    {
        if (_currentCameraCoroutine != null) StopCoroutine(_currentCameraCoroutine);
        _currentCameraCoroutine = SmoothFovTransition(from, to, overTime, onTransitionCompleted);
        StartCoroutine(SmoothFovTransition(from, to, overTime, onTransitionCompleted));
    }

    public void SmoothCurrentCameraRotation(Vector3 from, Vector3 to, float overTime, Action onTransitionCompleted)
    {
        if (_currentCameraCoroutine != null) StopCoroutine(_currentCameraCoroutine);
        _currentCameraCoroutine = SmoothRotation(from, to, overTime, onTransitionCompleted);
        StartCoroutine(SmoothRotation(from, to, overTime, onTransitionCompleted));
    }

    private IEnumerator SmoothFovTransition(float from, float to, float overTime, Action onTransitionCompleted)
    {
        CinemachineVirtualCamera vcam = CurrentCamera.GetComponent<CinemachineVirtualCamera>();
        vcam.m_Lens.FieldOfView = from;
        float timer = 0f;
        float progress = 0f;
        float FOVfrom = vcam.m_Lens.FieldOfView;
        float lerpdFOV = FOVfrom;

        while (timer < overTime)
        {
            timer += Time.deltaTime;
            progress = timer / overTime;
            progress = SmoothProgress(progress);
            lerpdFOV = Mathf.Lerp(FOVfrom, to, progress);
            vcam.m_Lens.FieldOfView = lerpdFOV;
            yield return null;
        }

        vcam.m_Lens.FieldOfView = from;

        if (onTransitionCompleted != null) onTransitionCompleted();
    }

    private IEnumerator SmoothRotation(Vector3 from, Vector3 to, float overTime, Action onTransitionCompleted)
    {
        CinemachineVirtualCamera vcam = CurrentCamera.GetComponent<CinemachineVirtualCamera>();
        float timer = 0f;
        float progress = 0f;
        Vector3 position = vcam.transform.position;
        Vector3 lerpdRotation = from;

        vcam.transform.SetPositionAndRotation(position, Quaternion.Euler(from));

        while (timer < overTime)
        {
            timer += Time.deltaTime;
            progress = timer / overTime;
            progress = SmoothProgress(progress);
            lerpdRotation = Vector3.Lerp(from, to, progress);
            vcam.transform.SetPositionAndRotation(position, Quaternion.Euler(lerpdRotation));
            yield return null;
        }

        vcam.transform.SetPositionAndRotation(position, Quaternion.Euler(to));

        if (onTransitionCompleted != null) onTransitionCompleted();
    }

    private float SmoothProgress(float progress)
    {
        progress = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, progress);
        progress = Mathf.Sin(progress);
        progress = (progress / 2f) + .5f;
        return progress;
    }
}
