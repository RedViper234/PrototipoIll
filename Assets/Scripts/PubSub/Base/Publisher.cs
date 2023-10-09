using System;
using System.Collections.Generic;
using UnityEngine;

    public static class Publisher
    {
        private static Dictionary<Type, List<ISubscriber>> _allSubscribers = new Dictionary<Type, List<ISubscriber>>();

        public static void Subscribe(ISubscriber subscriber, ValueType messageType)
        {
            Type tipoMessaggio = messageType.GetType();
            if (_allSubscribers.ContainsKey(tipoMessaggio))
            {
                _allSubscribers[tipoMessaggio].Add(subscriber);
            }
            else
            {
                List<ISubscriber> subscribers = new List<ISubscriber> { subscriber };

                _allSubscribers.Add(tipoMessaggio, subscribers);
            }
        }

        public static void Publish(IMessage message)
        {
            ValueType messageType = message as ValueType;
            
            if (!_allSubscribers.ContainsKey(messageType.GetType()))
            {
                return;
            }
            else
            {
                //Debug.LogWarning("Contengono la chiave");
            }

            foreach (ISubscriber subscriber in _allSubscribers[messageType.GetType()])
            {
                subscriber.OnPublish(message);
            }
        }

        public static void Unsubscribe(ISubscriber subscriber, ValueType messageType)
        {
            if (_allSubscribers.ContainsKey(messageType.GetType()))
            {
                _allSubscribers[messageType.GetType()].Remove(subscriber);
            }
        }
    }
