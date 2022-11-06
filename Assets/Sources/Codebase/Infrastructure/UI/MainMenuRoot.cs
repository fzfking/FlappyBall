using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Codebase.Infrastructure.UI
{
    [RequireComponent(typeof(Canvas))]
    public class MainMenuRoot : UIRoot
    {
        [SerializeField] private TMP_Dropdown _difficultyDropdown;
        [SerializeField] private Button _startButton;
        public TMP_Dropdown DifficultyDropdown => _difficultyDropdown;
        public Button StartButton => _startButton;
    }
}
