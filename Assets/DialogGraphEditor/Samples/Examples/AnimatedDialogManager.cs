using System;
using System.Threading;
using UnityEngine;

namespace DialogGraph.Examples
{
    /// <summary>
    /// Example on how to extend the DialogManager to add a simple animation on the panel and the message typewriting effect.
    /// </summary>
    public class AnimatedDialogManager : DialogManager
    {
        [SerializeField] private CanvasGroup panelCanvasGroup;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private float typingSpeed = 2f;

        private bool _isTyping;
        private string _cachedMessage;
        private CancellationTokenSource _cancelTyping;

        protected async override void Start()
        {
            panelCanvasGroup.alpha = 0f;
            _ = base.ShowPanel(false);
            base.Start();
        }

        protected override async Awaitable ShowPanel(bool enable)
        {
            if (enable)
            {
                await base.ShowPanel(enable);
                await FadePanel(1f);
            }
            else
            {
                await FadePanel(0f);
                await base.ShowPanel(enable);
            }
        }

        private async Awaitable FadePanel(float targetAlpha)
        {
            if (panelCanvasGroup.alpha == targetAlpha) return;

            var timer = 0f;
            var startAlpha = panelCanvasGroup.alpha;

            while (timer < fadeDuration)
            {
                panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.Clamp01(timer / fadeDuration));
                timer += Time.deltaTime;
                await Awaitable.NextFrameAsync();
            }

            panelCanvasGroup.alpha = targetAlpha;
        }

        protected override async Awaitable ShowMessage(string message)
        {
            try
            {
                _cancelTyping = new CancellationTokenSource();

                var curMsg = string.Empty;
                var delay = 1f / (message.Length * typingSpeed);
                _cachedMessage = message;
                while (curMsg.Length < message.Length)
                {
                    _isTyping = true;
                    curMsg += message[curMsg.Length];
                    await base.ShowMessage(curMsg);
                    await Awaitable.WaitForSecondsAsync(delay, cancellationToken: _cancelTyping.Token);
                }

                await base.ShowMessage(message);
                _isTyping = false;
                _cachedMessage = string.Empty;
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        protected override void ShowNextNode(string nextNodeId)
        {
            if (_isTyping)
            {
                _ = base.ShowMessage(_cachedMessage);
                _isTyping = false;
                _cancelTyping?.Cancel();
                _cancelTyping?.Dispose();
                _cancelTyping = null;
                return;
            }

            base.ShowNextNode(nextNodeId);
        }
    }
}