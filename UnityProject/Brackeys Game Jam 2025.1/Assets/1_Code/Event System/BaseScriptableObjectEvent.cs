using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Events_system
{
    // Single type event with parameter
    [Serializable]
    public abstract class BaseGameEvent<TParameter> : ScriptableObject
    {
        public TParameter LastValue { get; protected set; }

        protected List<Action<TParameter>> _listeners = new List<Action<TParameter>>();

        public virtual void Raise(TParameter t)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke(t);
            }

            LastValue = t;
        }

        public TParameter RegisterListener(Action<TParameter> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }

            return LastValue;
        }

        public void UnRegisterListener(Action<TParameter> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        protected void OnEnable()
        {           
            SceneManager.sceneLoaded -= OnSceneChange;
            SceneManager.sceneLoaded += OnSceneChange;

            LastValue = default(TParameter);
        }

        protected void OnSceneChange(Scene arg0, LoadSceneMode arg1)
        {
            LastValue = default;
        }

        protected void OnDisable()
        {
            Clean();
        }

        protected void Clean()
        {
            LastValue = default;
            SceneManager.sceneLoaded -= OnSceneChange;
        }

        private void OnDestroy()
        {
            Clean();
        }


    }

    // Event without parameter
    [Serializable]
    public abstract class BaseGameEvent : ScriptableObject
    {
        private List<Action> _listeners = new List<Action>();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i]?.Invoke();
            }
        }

        public void RegisterListener(Action listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public void UnRegisterListener(Action listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }
    }
}