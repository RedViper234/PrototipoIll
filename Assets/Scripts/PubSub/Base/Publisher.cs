using System;
using System.Collections.Generic;
using UnityEngine;

    public static class Publisher
    {
        private static Dictionary<ValueType, List<ISubscriber>> _allSubscribers = new Dictionary<ValueType, List<ISubscriber>>();

        public static void Subscribe(ISubscriber subscriber, ValueType messageType)
        {
            if (_allSubscribers.ContainsKey(messageType))
            {
                _allSubscribers[messageType].Add(subscriber);
            }
            else
            {
                List<ISubscriber> subscribers = new List<ISubscriber> { subscriber };

                _allSubscribers.Add(messageType, subscribers);
            }
        }

        public static void Publish(IMessage message)
        {
            ValueType messageType = message as ValueType;

            if (!_allSubscribers.ContainsKey(messageType))
            {
                Debug.Log("Messagio di tipo: "+messageType);
                return;
            }
            else
            {
                Debug.LogWarning("Contengono la chiave");
            }

            foreach (ISubscriber subscriber in _allSubscribers[messageType])
            {
                subscriber.OnPublish(message);
            }
        }

        public static void Unsubscribe(ISubscriber subscriber, ValueType messageType)
        {
            if (_allSubscribers.ContainsKey(messageType))
            {
                _allSubscribers[messageType].Remove(subscriber);
            }
        }
    }
