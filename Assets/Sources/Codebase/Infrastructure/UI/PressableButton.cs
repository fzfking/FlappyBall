using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources.Codebase.Infrastructure.UI
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