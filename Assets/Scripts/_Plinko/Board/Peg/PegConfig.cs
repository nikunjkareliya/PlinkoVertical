using UnityEngine;

[CreateAssetMenu(fileName = "PegConfig", menuName = "ScriptableObjects/PegConfig")]
public class PegConfig : ScriptableObject
{
    public PegType pegType;
    public ParticleSystem particleHit;
}
