using UnityEngine;
using System.Collections.Generic;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _minSize = 5f;
    [SerializeField] private float _maxSize = 15f;
    [SerializeField] private float _padding = 2f;
    [SerializeField] private float _smoothSpeed = 5f; // Controls how quickly camera follows targets
    [SerializeField] private float _verticalOffset = 2f; // How much to shift the camera up from the center

    private Camera _cam;
    [SerializeField] private List<Transform> _targets = new List<Transform>();
    private float _initialXPosition; // Store the initial X position

    void Awake()
    {
        _cam = GetComponent<Camera>();
        _initialXPosition = transform.position.x; // Store the initial X position
        GameEvents.OnCameraTargetAdd.Register(HandleCameraTargetAdd);
        GameEvents.OnCameraTargetRemove.Register(HandleCameraTargetRemove);
    }

    private void OnDestroy()
    {
        GameEvents.OnCameraTargetAdd.Unregister(HandleCameraTargetAdd);
        GameEvents.OnCameraTargetRemove.Unregister(HandleCameraTargetRemove);
    }

    private void HandleCameraTargetAdd(Transform target)
    {
        _targets.Add(target);
    }

    private void HandleCameraTargetRemove(Transform target)
    {
        _targets.Remove(target);
    }

    void LateUpdate()
    {
        if (_targets.Count == 0)
            return;

        // Get the vertical center point and required size
        Vector3 centerPoint = GetCenterPoint();
        Vector2 requiredSize = GetRequiredSize();

        // Create target position with original X value and apply vertical offset
        Vector3 targetPosition = new Vector3(_initialXPosition, centerPoint.y + _verticalOffset, transform.position.z);

        // Smoothly move camera position (only Y changes)
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed * Time.deltaTime);

        // Smoothly adjust camera size
        float targetSize = Mathf.Clamp(requiredSize.y / 2 + _padding, _minSize, _maxSize);
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, targetSize, _smoothSpeed * Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        if (_targets.Count == 1)
            return _targets[0].position;

        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        foreach (var target in _targets)
        {
            bounds.Encapsulate(target.position);
        }

        return bounds.center;
    }

    Vector2 GetRequiredSize()
    {
        if (_targets.Count == 1)
            return new Vector2(_minSize * 2, _minSize * 2);

        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        foreach (var target in _targets)
        {
            bounds.Encapsulate(target.position);
        }

        return new Vector2(bounds.size.x, bounds.size.y);
    }
}





//public class CameraController : MonoBehaviour
//{
//    [SerializeField] private float _minSize = 5f;
//    [SerializeField] private float _maxSize = 15f;
//    [SerializeField] private float _padding = 2f;
//    [SerializeField] private float _smoothSpeed = 5f; // Controls how quickly camera follows targets

//    private Camera _cam;
//    [SerializeField] private List<Transform> _targets = new List<Transform>();
//    private float _initialXPosition; // Store the initial X position

//    void Awake()
//    {
//        _cam = GetComponent<Camera>();
//        _initialXPosition = transform.position.x; // Store the initial X position
//        GameEvents.OnCameraTargetAdd.Register(HandleCameraTargetAdd);
//        GameEvents.OnCameraTargetRemove.Register(HandleCameraTargetRemove);
//    }

//    private void OnDestroy()
//    {
//        GameEvents.OnCameraTargetAdd.Unregister(HandleCameraTargetAdd);
//        GameEvents.OnCameraTargetRemove.Unregister(HandleCameraTargetRemove);
//    }

//    private void HandleCameraTargetAdd(Transform target)
//    {
//        _targets.Add(target);
//    }

//    private void HandleCameraTargetRemove(Transform target)
//    {
//        _targets.Remove(target);
//    }

//    void LateUpdate()
//    {
//        if (_targets.Count == 0)
//            return;

//        // Get the vertical center point and required size
//        Vector3 centerPoint = GetCenterPoint();
//        Vector2 requiredSize = GetRequiredSize();

//        // Create target position with original X value
//        Vector3 targetPosition = new Vector3(_initialXPosition, centerPoint.y, transform.position.z);

//        // Smoothly move camera position (only Y changes)
//        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed * Time.deltaTime);

//        // Smoothly adjust camera size
//        float targetSize = Mathf.Clamp(requiredSize.y / 2 + _padding, _minSize, _maxSize);
//        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, targetSize, _smoothSpeed * Time.deltaTime);
//    }

//    Vector3 GetCenterPoint()
//    {
//        if (_targets.Count == 1)
//            return _targets[0].position;

//        var bounds = new Bounds(_targets[0].position, Vector3.zero);
//        foreach (var target in _targets)
//        {
//            bounds.Encapsulate(target.position);
//        }

//        return bounds.center;
//    }

//    Vector2 GetRequiredSize()
//    {
//        if (_targets.Count == 1)
//            return new Vector2(_minSize * 2, _minSize * 2);

//        var bounds = new Bounds(_targets[0].position, Vector3.zero);
//        foreach (var target in _targets)
//        {
//            bounds.Encapsulate(target.position);
//        }

//        return new Vector2(bounds.size.x, bounds.size.y);
//    }
//}





//using UnityEngine;
//using System.Collections.Generic;
//using System;

//public class CameraController : MonoBehaviour
//{
//    [SerializeField] private float _minSize = 5f;
//    [SerializeField] private float _maxSize = 15f;
//    [SerializeField] private float _padding = 2f;

//    private Camera _cam;
//    [SerializeField] private List<Transform> _targets = new List<Transform>();

//    void Awake()
//    {
//        _cam = GetComponent<Camera>();

//        GameEvents.OnCameraTargetAdd.Register(HandleCameraTargetAdd);
//        GameEvents.OnCameraTargetRemove.Register(HandleCameraTargetRemove);
//    }

//    private void OnDestroy()
//    {
//        GameEvents.OnCameraTargetAdd.Unregister(HandleCameraTargetAdd);
//        GameEvents.OnCameraTargetRemove.Unregister(HandleCameraTargetRemove);
//    }
//    private void HandleCameraTargetAdd(Transform target)
//    {
//        _targets.Add(target);
//    }

//    private void HandleCameraTargetRemove(Transform target)
//    {
//        _targets.Remove(target);
//    }


//    void LateUpdate()
//    {
//        if (_targets.Count == 0)
//            return;

//        Vector3 centerPoint = GetCenterPoint();
//        Vector2 size = GetRequiredSize();

//        // Update camera position and size
//        transform.position = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);
//        _cam.orthographicSize = Mathf.Clamp(size.y / 2 + _padding, _minSize, _maxSize);
//    }

//    Vector3 GetCenterPoint()
//    {
//        if (_targets.Count == 1)
//            return _targets[0].position;

//        var bounds = new Bounds(_targets[0].position, Vector3.zero);
//        foreach (var target in _targets)
//        {
//            bounds.Encapsulate(target.position);
//        }

//        return bounds.center;
//    }

//    Vector2 GetRequiredSize()
//    {
//        if (_targets.Count == 1)
//            return new Vector2(_minSize * 2, _minSize * 2);

//        var bounds = new Bounds(_targets[0].position, Vector3.zero);
//        foreach (var target in _targets)
//        {
//            bounds.Encapsulate(target.position);
//        }

//        return new Vector2(bounds.size.x, bounds.size.y);
//    }
//}