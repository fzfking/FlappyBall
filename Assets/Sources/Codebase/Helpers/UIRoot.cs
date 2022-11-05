using UnityEngine;

namespace Sources.Codebase.Helpers
{
    [RequireComponent(typeof(Canvas))]
    public class UIRoot: MonoBehaviour
    {
        public Canvas Canvas => GetComponent<Canvas>();
    }
}