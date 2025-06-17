using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.EntryPoint
{
    public class Loading : IDisposable
    {
        private static Loading s_instance;

        private readonly Queue<ILoadingStep> _steps = new();
        private readonly MonoBehaviour _mono;
        private readonly ILoadingScreen _screen;

        private Coroutine _runningCoroutine = null;
        private ILoadingStep _currentStep = null;
        private float _currentWeight, _maxWeight;

        public static Loading Create(MonoBehaviour mono, ILoadingScreen screen) => s_instance ??= new(mono, screen);
        private Loading(MonoBehaviour mono, ILoadingScreen screen)
        {
            _mono = mono;
            _screen = screen;
        }

        public void Add(IEnumerator coroutine) => Add(new CoroutineStep(coroutine));
        public void Add(IEnumerator coroutine, string desc) => Add(new CoroutineStep(coroutine, desc));
        public void Add(IEnumerator coroutine, string desc, float weight) => Add(new CoroutineStep(coroutine, desc, weight));

        public void Add(ILoadingStep step)
        {
            _steps.Enqueue(step); 
            Run(step.Weight);
        }
        public void Add(ILoadingStep stepA, ILoadingStep stepB)
        {
            _steps.Enqueue(stepA); _steps.Enqueue(stepB);
            Run(stepA.Weight + stepB.Weight);
        }
        public void Add(params ILoadingStep[] steps)
        {
            float weight = 0f;
            for (int i = 0; i < steps.Length; i++)
            {
                _steps.Enqueue(steps[i]);
                weight += steps[i].Weight;
            }
            Run(weight);
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

                yield return _currentStep.GetEnumerator();

                _currentWeight += _currentStep.Weight;
                _screen.Progress = _currentWeight / _maxWeight;

                _currentStep = null;
            }

            yield return _screen.SmoothOff();

            _runningCoroutine = null;
        }

        public void Dispose()
        {
            if(s_instance == this)
                s_instance = null;
        }
    }
}
