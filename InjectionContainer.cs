using System;
using System.Collections.Generic;
using UnityEngine;

namespace IC
{
    public class InjectionContainer
    {
        private InjectionContainer _parent;
        private Dictionary<Type, object> _objects = new Dictionary<Type, object>();
        private GameObject _gameObject;

        public InjectionContainer(GameObject gameObject, InjectionContainer parent = null)
        {
            _gameObject = gameObject;
            _parent = parent;
        }

        public T Install<T>(T obj)
        {
            var type = typeof(T);
            if (_objects.ContainsKey(type))
                Debug.LogError($"Object of type {type} has been installed previously.");
            _objects.Add(type, obj);

            return obj;
        }

        public void Fetch<T>(out T value)
        {
            var type = typeof(T);

            if (_objects.ContainsKey(type))
                value = (T)_objects[type];
            else if (_parent == null)
                value = default;
            else
                _parent.Fetch<T>(out value);

#if UNITY_EDITOR
            if (value == null)
                Debug.unityLogger.LogError($"<color=#FF0000>InjectionContainer</color>", $"{_gameObject.name} has no {type} installed.");
#endif
        }
    }
}