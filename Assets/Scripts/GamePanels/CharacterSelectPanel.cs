using Shared.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRun3D
{
    public class CharacterSelectPanel : BasePanel
    {
        private void Start()
        {
            SharedEvents.OnPanelRegistered.Execute(this);
        }

        // Example of overriding the OnShow method to add specific behavior
        protected override void OnShow()
        {
            Debug.Log($"{State} is now visible");
        }
    }
}