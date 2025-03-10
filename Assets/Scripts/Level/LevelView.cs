using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRun3D
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private int _levelID;
        [SerializeField] private GameObject _levelLock;
        [SerializeField] private GameObject _levelEndGate;

        public void ShowLock()
        {
            _levelLock.SetActive(true);
        }

        public void HideLock()
        {
            _levelLock.SetActive(false);
        }

        public void ShowLevelEndGate()
        {
            _levelEndGate.SetActive(true);
        }

        public void HideLevelEndGate()
        {
            _levelEndGate.SetActive(false);
        }
    }
}