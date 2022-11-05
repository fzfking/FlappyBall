using System;
using System.Globalization;
using System.Text;
using Sources.Codebase.Infrastructure.UI;
using TMPro;
using UnityEngine;

namespace Sources.Codebase.Helpers
{
    public class GameLoopUIRoot : UIRoot
    {
        [SerializeField] private PressableButton _upButton;
        [SerializeField] private TextMeshProUGUI _timePassedLabel;
        public PressableButton UpButton => _upButton;
        private StringBuilder _stringBuilder;

        private void Start()
        {
            _stringBuilder = new StringBuilder(_timePassedLabel.text);
        }

        public void SetTimePassedLabel(TimeSpan timeSpan)
        {
            _timePassedLabel.text = _stringBuilder.ToString().Replace("{time}", timeSpan.TotalSeconds.ToString(CultureInfo.InvariantCulture));
        }
    }
}