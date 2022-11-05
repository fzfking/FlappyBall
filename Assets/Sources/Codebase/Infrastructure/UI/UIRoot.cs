using UnityEngine;

namespace Sources.Codebase.Infrastructure.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UIRoot: MonoBehaviour
    {
        public Canvas Canvas => GetComponent<Canvas>();
    }
}