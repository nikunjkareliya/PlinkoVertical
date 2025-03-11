using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlinkoVertical
{
    public class PegSaw : MonoBehaviour, IDestroyable
    {
        public PegConfig pegConfig;

        public void Interact()
        {
            Debug.Log($"Hit with peg of type -> {pegConfig.pegType}");
        }
    }
}