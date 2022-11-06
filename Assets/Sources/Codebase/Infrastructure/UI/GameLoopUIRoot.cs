using System;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;

namespace Sources.Codebase.Infrastructure.UI
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
            _timePassedLabel.text = _stringBuilder.ToString().Replace("{time}", timeSpan.TotalSeconds.ToString("F1",CultureInfo.InvariantCulture));
        }
    }
}