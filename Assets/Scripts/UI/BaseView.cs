using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRun3D
{
    public abstract class BaseView : MonoBehaviour
    {
        public CanvasGroup _canvasGroup;
        private float _transitionSpeed = 0.25f;

        public virtual void Show()
        {
            _canvasGroup.DOFade(1f, _transitionSpeed).OnComplete(() =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }).SetUpdate(true);
        }

        public virtual void Hide(bool isInstant = false)
        {
            _canvasGroup.DOFade(0f, isInstant ? 0f : _transitionSpeed).OnComplete(() =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }).SetUpdate(true);
        }
    }
}