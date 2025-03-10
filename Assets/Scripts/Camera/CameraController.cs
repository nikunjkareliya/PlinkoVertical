using DG.Tweening;
using Shared.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRun3D
{    
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        [SerializeField] private float _distanceOffset = 8f;
        [SerializeField] private float _heightOffset = 3f;
        
        private float _smoothSpeed = 2f;                
        [SerializeField] private float _cameraSpeed = 2f;                
        public float rotationSpeed = 1f;

        private Vector3 desiredPosition;
                
        [SerializeField] private Transform _cameraFocusPoint;
                
        [SerializeField] private float _maxRotationAngle = 5f; // Maximum angle in degrees for X and Y axes
        [SerializeField] private float _rotationDuration = 2f;
        [SerializeField] private float _introRotationSpeed = 0.2f; // Speed for random rotation in intro state
        private Quaternion _originalRotation = Quaternion.Euler(21, -8, 0);

        private GameStateModel _gameStateModel;

        private void Awake()
        {
            _gameStateModel = GetGameStateModel();

            GameEvents.OnGameInitialized.Register(HandleGameInitialized);
            SharedEvents.OnGameStateChanged.Register(HandleGameStateChanged);
        }

        private void OnDestroy()
        {
            SharedEvents.OnGameStateChanged.Unregister(HandleGameStateChanged);
            GameEvents.OnGameInitialized.Unregister(HandleGameInitialized);
        }

        private void HandleGameInitialized()
        {
            _smoothSpeed = 500;
            Invoke(nameof(ResetSmoothSpeed), 0.5f);
        }

        void ResetSmoothSpeed()
        {
            _smoothSpeed = _cameraSpeed;
        }

        private void HandleGameStateChanged(GameState state)
        {
            //Debug.Log($"HandleGameStateChanged -> new state -> {state}");

            if (state == GameState.CharacterSelect)
            {
                _distanceOffset = 3f;
                _heightOffset = 1f;
                _smoothSpeed = 4f;
            }
            else if (state == GameState.Home || state == GameState.Gameplay)
            {
                _distanceOffset = 8f;
                _heightOffset = 3f;
                _smoothSpeed = _cameraSpeed;
            }
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                Debug.LogWarning("CameraFollow: Target not assigned.");
                return;
            }

            //if (_gameStateModel.CurrentState == GameState.Home)
            //{
            //    //ApplyIntroRotation();
            //    desiredPosition = target.position - (target.forward * distanceOffset) + (Vector3.up * heightOffset);
            //    transform.position = desiredPosition; //Vector3.Lerp(transform.position, desiredPosition, 500 * Time.deltaTime);
            //    transform.LookAt(target.position);
            //}
            //else if (_gameStateModel.CurrentState == GameState.Gameplay)            
            if (_gameStateModel.CurrentState == GameState.Gameplay 
                || _gameStateModel.CurrentState == GameState.Home
                || _gameStateModel.CurrentState == GameState.CharacterSelect)
            {
                // Step 1: Calculate the desired position of the camera based on the target's rotation and position
                desiredPosition = target.position - (target.forward * _distanceOffset) + (Vector3.up * _heightOffset);
                //desiredPosition = target.position - target.forward;

                // Step 2: Smoothly interpolate from the current camera position to the desired position for smooth motion
                transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

                transform.LookAt(target.position);

                // Step 3: Calculate the target rotation for the camera to look at the target with height offset
                //targetRotation = Quaternion.LookRotation((target.position + Vector3.up * heightOffset) - transform.position);

                //// Step 4: Smoothly rotate the camera using Slerp
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


                //// Step 3: Make the camera look at the target to keep it centered
                //transform.LookAt(target.position + Vector3.up * heightOffset);
                ////transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position + Vector3.up * heightOffset), rotationSpeed * Time.deltaTime);
                ///
            }            
        }

        private void ApplyIntroRotation()
        {            
            float xRotation = Mathf.Sin(Time.time * _introRotationSpeed) * 2f; 
            float yRotation = Mathf.Cos(Time.time * _introRotationSpeed) * 2f;

            Quaternion introRotation = Quaternion.Euler(_originalRotation.eulerAngles.x + xRotation,
                                                        _originalRotation.eulerAngles.y + yRotation,
                                                        _originalRotation.eulerAngles.z);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, introRotation, Time.deltaTime * _smoothSpeed);
        }

        private GameStateModel GetGameStateModel()
        {
            GameStateModel gameStateModel = ModelStore.Get<GameStateModel>();

            if (gameStateModel == null)
            {
                gameStateModel = new GameStateModel();
                ModelStore.Register<GameStateModel>(gameStateModel);
            }

            return gameStateModel;
        }
    }

}