using Shared.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ziggurat7
{
    public class GameplayPanel : BasePanel
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