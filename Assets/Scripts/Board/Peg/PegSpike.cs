using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlinkoVertical
{
    public class PegSpike : MonoBehaviour, IDestroyable
    {
        public PegConfig pegConfig;

        public void Interact()
        {
            Debug.Log($"Hit with peg of type -> {pegConfig.pegType}");
        }
    }
}