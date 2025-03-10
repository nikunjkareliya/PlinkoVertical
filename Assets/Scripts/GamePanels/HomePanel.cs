using Shared.Core;
using UnityEngine;

namespace Ziggurat7
{
    public class HomePanel : BasePanel
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