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

        public object Current => _currentStep;

        public static Loading Create(MonoBehaviour mono, ILoadingScreen screen) => _instance ??= new(mono, screen);
        private Loading(MonoBehaviour mono, ILoadingScreen screen)
        {
            _mono = mono;
            _screen = screen;
        }

        public void Add(ALoadingStep step)
        {
            _steps.Enqueue(step); Run();
        }

        public void Add(Action finalAction, ALoadingStep step)
        {
            Add(step);
            _finalAction = finalAction;
        }

        public void Add(params ALoadingStep[] steps)
        {
            for (int i = 0; i < steps.Length; i++)
                _steps.Enqueue(steps[i]);
            Run();
        }

        public void Add(Action finalAction, params ALoadingStep[] steps)
        {
            Add(steps);
            _finalAction = finalAction;
        }

        private void Run()
        {
            if (_currentStep == null)
            {
                if (_runningCoroutine != null)
                    _mono.StopCoroutine(_runningCoroutine);

                _runningCoroutine = _mono.StartCoroutine(Run_Cn());
            }
        }

        private IEnumerator Run_Cn()
        {
            _currentStep = null;
            yield return _screen.SmoothOn();

            while (_steps.Count > 0)
            {
                _currentStep = _steps.Dequeue();
                _screen.Description = _currentStep.Description;

                while (_currentStep.MoveNext())
                {
                    _screen.Progress = _currentStep.Progress;
                    yield return null;
                }

                _currentStep = null;
            }

            yield return _screen.SmoothOff();

            _finalAction?.Invoke();

            _finalAction = null;
            _runningCoroutine = null;
        }

        public void Dispose()
        {
            if(_instance == this)
                _instance = null;
        }
    }
}
