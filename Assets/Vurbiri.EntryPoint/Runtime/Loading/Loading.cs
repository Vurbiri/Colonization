//Assets\Vurbiri.EntryPoint\Runtime\Loading\Loading.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.EntryPoint
{
    public class Loading : IDisposable
    {
        private static Loading _instance;

        private readonly Queue<ALoadingStep> _steps = new();
        private readonly MonoBehaviour _mono;
        private readonly ILoadingScreen _screen;
        private Action _finalAction;
        private Coroutine _runningCoroutine = null;
        private ALoadingStep _currentStep = null;
        private float _currentWeight, _maxWeight;

        public static Loading Create(MonoBehaviour mono, ILoadingScreen screen) => _instance ??= new(mono, screen);
        private Loading(MonoBehaviour mono, ILoadingScreen screen)
        {
            _mono = mono;
            _screen = screen;
        }

        public void Add(IEnumerator coroutine) => Add(new CoroutineStep(coroutine));
        public void Add(IEnumerator coroutine, string desc) => Add(new CoroutineStep(coroutine, desc));
        public void Add(IEnumerator coroutine, string desc, float weight) => Add(new CoroutineStep(coroutine, desc, weight));

        public void Add(ALoadingStep step)
        {
            _steps.Enqueue(step); 
            Run(step.Weight);
        }
        public void Add(Action finalAction, ALoadingStep step)
        {
            Add(step);
            _finalAction = finalAction;
        }

        public void Add(params ALoadingStep[] steps)
        {
            float weight = 0f;
            for (int i = 0; i < steps.Length; i++)
            {
                _steps.Enqueue(steps[i]);
                weight += steps[i].Weight;
            }
            Run(weight);
        }
        public void Add(Action finalAction, params ALoadingStep[] steps)
        {
            Add(steps);
            _finalAction = finalAction;
        }

        private void Run(float weight)
        {
            if (_currentStep == null)
            {
                if (_runningCoroutine != null)
                    _mono.StopCoroutine(_runningCoroutine);

                _screen.Progress = _currentWeight = _maxWeight = 0f;
                _currentStep = _steps.Peek();
                _runningCoroutine = _mono.StartCoroutine(Run_Cn());
            }

            _maxWeight += weight;
        }

        private IEnumerator Run_Cn()
        {
            yield return _screen.SmoothOn();

            while (_steps.Count > 0)
            {
                _currentStep = _steps.Dequeue();
                _screen.Description = _currentStep.Description;

                while (_currentStep.MoveNext())
                    yield return null;

                _currentWeight += _currentStep.Weight;
                _screen.Progress = _currentWeight / _maxWeight;
                _currentStep = null;
            }

            yield return _screen.SmoothOff();

            _runningCoroutine = null;

            _finalAction?.Invoke();
            _finalAction = null;
        }

        public void Dispose()
        {
            if(_instance == this)
                _instance = null;
        }
    }
}
