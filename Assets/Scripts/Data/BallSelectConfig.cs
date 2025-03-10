using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRun3D
{
    [CreateAssetMenu(fileName = "BallSelectConfig", menuName = "ScriptableObjects/BallSelectConfig", order = 1)]
    public class BallSelectConfig : ScriptableObject
    {
        public BallData[] balls;
    }

    [System.Serializable]
    public class BallData
    {
        public int ID;
        public Material material;        
        public int cost;
    }
}