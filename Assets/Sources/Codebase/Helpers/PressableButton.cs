using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources.Codebase.Helpers
{
    public class PressableButton : Button
    {
        public event Action OnButtonDown; 
        public event Action OnButtonUp; 
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnButtonDown?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            OnButtonUp?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnButtonDown = null;
            OnButtonUp = null;
        }
    }
}