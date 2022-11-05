using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Codebase.Helpers
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
