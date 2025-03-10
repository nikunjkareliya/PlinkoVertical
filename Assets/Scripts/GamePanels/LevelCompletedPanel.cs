using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Core;

namespace Ziggurat7
{
    public class LevelCompletedPanel : BasePanel
    {
        private void Start()
        {
            SharedEvents.OnPanelRegistered.Execute(this);
        }

        protected override void OnShow()
        {
            Debug.Log($"{State} is now visible");
        }
    }
}