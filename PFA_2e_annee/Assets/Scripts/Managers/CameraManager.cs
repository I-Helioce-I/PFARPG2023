using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField][ReadOnlyInspector] private Camera _mainCam;
    [SerializeField] private List<CinemachineVirtualCameraBase> _battleCameras = new List<CinemachineVirtualCameraBase>();
    [SerializeField] private List<CinemachineVirtualCameraBase> _explorationCameras = new List<CinemachineVirtualCameraBase>();
    [SerializeField][ReadOnlyInspector] private CinemachineVirtualCameraBase _currentCamera;
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
        if (_currentCamera) _currentCamera.enabled = false;
        camera.enabled = false;
        _currentCamera = camera;
        foreach (CinemachineVirtualCameraBase Vcamera in _battleCameras)
        {
            Vcamera.enabled = false;
        }
        foreach (CinemachineVirtualCameraBase Vcamera in _explorationCameras)
        {
            Vcamera.enabled = false;
        }
        _currentCamera.enabled = true;
    }

    public void SetCameraField(List<CinemachineVirtualCameraBase> cameraField)
    {
        _currentCamera.enabled = false;
        _currentCamera = cameraField[0];
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
        _currentCamera.enabled = true;
    }
}
