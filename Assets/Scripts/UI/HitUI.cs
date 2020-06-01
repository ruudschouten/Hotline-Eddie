using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class HitUI : MonoBehaviour
    {
        [SerializeField] private Message[] messages;

        private List<Message> _usedMessages = new List<Message>();
        private List<Message> _availableMessages = new List<Message>();

        private void Awake()
        {
            _availableMessages = messages.ToList();
        }

        public void ShowRandomMessage()
        {
            var message = GetRandom();
            if (message == null)
            {
                return;
            }
            
            SetUsed(message);
            message.OnFade.AddListener(() => SetAvailable(message));
            message.ShowAndHide();
        }

        public void ShowAll()
        {
            foreach (var message in messages)
            {
                message.StopAllCoroutines();
                message.Group.alpha = 1;
            }
        }

        private void SetUsed(Message message)
        {
            _usedMessages.Add(message);
            _availableMessages.Remove(message);
        }

        private void SetAvailable(Message message)
        {
            message.OnFade.RemoveAllListeners();
            
            _usedMessages.Remove(message);
            _availableMessages.Add(message);
        }

        private Message GetRandom()
        {
            if (_availableMessages.Count <= 0)
            {
                return null;
            }
            return _availableMessages[Random.Range(0, _availableMessages.Count)];
        }
    }
}