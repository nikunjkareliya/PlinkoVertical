using Shared.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRun3D
{    
    public class BallController : MonoBehaviour
    {        
        [SerializeField] private Camera _camera;
        private Rigidbody _rb;

        private float _maxVelocity = 15f;
        private Vector2 _startTouchPosition;
        private Vector2 _currentTouchPosition;              

        // Sensitivity and speed
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float _initialForce= 10f;
        [SerializeField] private float rotationSpeed = 20f;
        [SerializeField] private float swipeThreshold = 50f; // Minimum swipe distance to detect a swipe

        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Renderer _renderer;
        private GameState _currentState;
        private GameModel _gameModel;

        private void Awake()
        {
            _gameModel = GetGameModel();
            _rb = GetComponent<Rigidbody>();

            SharedEvents.OnGameStateChanged.Register(HandleGameStateChanged);
            GameEvents.OnLevelLoad.Register(HandleLevelLoad);

            
        }

        private void OnDestroy()
        {
            SharedEvents.OnGameStateChanged.Unregister(HandleGameStateChanged);
            GameEvents.OnLevelLoad.Unregister(HandleLevelLoad);

            
        }

        private void HandleGameStateChanged(GameState state)
        {
            _currentState = state;

            if (state == GameState.Gameplay)
            {                
                _rb.velocity = transform.forward * _initialForce;
            }
        }

        private void HandleLevelLoad(int levelID)
        {
            Debug.Log($"LOAD LEVEL ID -> {levelID}");
            
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            _rb.position = _spawnPoints[levelID].position;            
        }

        private void HandleBallSkinChanged(Material material)
        {
            _renderer.sharedMaterial = material;
        }

        void Update()
        {
            if (_currentState == GameState.Gameplay)
            { 
                MouseSwipeDetection();
            }
        }

        private void FixedUpdate()
        {            
            if (_currentState == GameState.Gameplay)
            { 
                _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);
            }            
        }
        
        void MouseSwipeDetection()
        {
            if (Input.GetMouseButtonDown(0))  // When mouse button is pressed down
            {
                _startTouchPosition = Input.mousePosition;                
            }
            else if (Input.GetMouseButton(0))  // When mouse is dragged
            {
                _currentTouchPosition = Input.mousePosition;
                Vector2 swipeDelta = _currentTouchPosition - _startTouchPosition;                                
                Vector3 targetDirection = new Vector3(swipeDelta.x, 0, swipeDelta.y).normalized;
                //Debug.Log($"swipeDelta -> {swipeDelta}");

                Vector3 worldDirection = _camera.transform.TransformDirection(targetDirection);
                worldDirection.y = 0f;

                //rb.AddForce(targetDirection * moveSpeed * (swipeDelta.magnitude / Screen.dpi), ForceMode.Force);
                _rb.AddForce(worldDirection * moveSpeed * (swipeDelta.magnitude / Screen.dpi), ForceMode.Force);

                //rb.AddForce(Vector3.forward * moveSpeed * (swipeDelta.magnitude / Screen.dpi), ForceMode.Force);

                // Calculate rotation based on swipe direction
                //if (swipeDelta.sqrMagnitude > 0.1f)  // Prevent tiny rotations for minor swipes
                //{
                //    // Find the angle to rotate around the Y-axis based on swipe direction
                //    float targetAngle = Mathf.Atan2(swipeDelta.x, swipeDelta.y) * Mathf.Rad2Deg;

                //    // Create the rotation towards the target direction
                //    Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

                //    // Smoothly interpolate the ball's rotation towards the target rotation
                //    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
                //}


                // Update the startTouchPosition to the current position, so the delta is calculated between drag points
                _startTouchPosition = _currentTouchPosition;
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            var endGate = other.GetComponent<LevelEndGateView>();

            if (endGate != null)
            {
                Debug.Log($"LEVEL COMPLETED -> {_gameModel.currentLevel}");
                GameEvents.OnLevelCompleted.Execute(_gameModel.currentLevel);
                SharedEvents.OnGameStateChanged.Execute(GameState.LevelCompleted);

            }

            var coin = other.GetComponent<CoinView>();

            if (coin != null)
            {
                _gameModel.coinsLevel += 10;
                GameEvents.OnLevelCoinsUpdated.Execute(_gameModel.coinsLevel);
                Destroy(coin.gameObject);
                
            }


        }

        private void OnCollisionEnter(Collision other)
        {
            //var railingTrigger = other.gameObject.GetComponent<RailingHitTrigger>();

            //if (railingTrigger != null)
            //{
            //    var audio = ServiceLocator.Current.Get<IAudioService>();
            //    audio.PlayClip(AudioClips.MetalHit);
            //}            
        }

        private GameModel GetGameModel()
        {
            GameModel gameModel = ModelStore.Get<GameModel>();

            if (gameModel == null)
            {
                gameModel = new GameModel();
                ModelStore.Register<GameModel>(gameModel);
            }

            return gameModel;
        }


    }
    
}