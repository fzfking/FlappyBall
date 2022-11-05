using System;
using System.Text;
using Sources.Codebase.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Codebase.Infrastructure.UI
{
    public class EndGameUIRoot : UIRoot
    {
        [SerializeField] private TextMeshProUGUI LastAttemptLabel;
        [SerializeField] private TextMeshProUGUI AttemptsCountLabel;
        [SerializeField] private Button ChangeDifficultyButton;
        [SerializeField] private Button RestartButton;
        public event Action OnRestartClicked;
        public event Action OnChangeDifficultyClicked;
        private StringBuilder _lastAttemptStringBuilder;
        private StringBuilder _attemptCountStringBuilder;

        private void Awake()
        {
            _lastAttemptStringBuilder = new StringBuilder(LastAttemptLabel.text);
            _attemptCountStringBuilder = new StringBuilder(AttemptsCountLabel.text);
            RestartButton.onClick.AddListener(() => OnRestartClicked?.Invoke());
            ChangeDifficultyButton.onClick.AddListener(() => OnChangeDifficultyClicked?.Invoke());
        }

        public void SetLastAttemptTime(TimeSpan timeSpan)
        {
            LastAttemptLabel.text = _lastAttemptStringBuilder.ToString().Replace("{time}", timeSpan.ToString("g"));
        }
        public void SetAttemptsCount(int count)
        {
            AttemptsCountLabel.text = _attemptCountStringBuilder.ToString().Replace("{count}", count.ToString());
        }
    }
}