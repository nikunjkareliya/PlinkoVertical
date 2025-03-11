using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlinkoVertical
{
    public class PegX0 : MonoBehaviour, IPeg
    {
        public PegConfig pegConfig;

        public void Interact()
        {
            Debug.Log($"Hit with peg of type -> {pegConfig.pegType}");
        }
    }
}