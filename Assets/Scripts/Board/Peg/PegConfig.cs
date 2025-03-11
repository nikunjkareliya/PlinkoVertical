using UnityEngine;

namespace PlinkoVertical
{
    [CreateAssetMenu(fileName = "PegConfig", menuName = "ScriptableObjects/PegConfig")]
    public class PegConfig : ScriptableObject
    {
        public PegType pegType;
        public ParticleSystem particleHit;
    }
}