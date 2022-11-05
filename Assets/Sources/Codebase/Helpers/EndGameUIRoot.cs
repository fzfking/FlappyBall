using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Codebase.Helpers
{
    public class EndGameUIRoot : UIRoot
    {
        [SerializeField] private TextMeshProUGUI LastAttemptLabel;
        [SerializeField] private TextMeshProUGUI AttemptsCountLabel;
        [SerializeField] private Button ChangeDifficultyButton;
        [SerializeField] private Button RestartButton;
        private StringBuilder _stringBuilder;

        private void Awake()
        {
            _stringBuilder = new StringBuilder(LastAttemptLabel.text);
        }

        public void SetLastAttemptTime(TimeSpan timeSpan)
        {
            LastAttemptLabel.text = _stringBuilder.ToString().Replace("{time}", timeSpan.ToString("g"));
        }
    }
}