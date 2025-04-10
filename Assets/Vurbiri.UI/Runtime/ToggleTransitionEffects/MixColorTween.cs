//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\MixColorTween.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        private class MixColorTween : IEnumerator
        {
            private const int MARK = 0, STATE = 1;
            private const float MIN_DUATION = 0.01351f, MIN_DELTA_DUATION = 0.05f, MIN_DELTA_DUATION_N = -MIN_DELTA_DUATION;

            private readonly Colors _markColors, _stateColors;
            private Graphic _target;
            private CanvasRenderer _renderer;
            private Coroutine _coroutine;
            private Task _currentTask, _nextTask;
            private bool _isTwoTask;

            public float markDuration;
            public bool IsValid => _renderer != null;
            public object Current => null;

            public MixColorTween(Graphic target, float duration)
            {
                _target = target;
                _renderer = target.canvasRenderer;
                markDuration = duration;
                _markColors = new(MARK);
                _stateColors = new(STATE);
            }

            public void SetMarkColorInstant(Color target)
            {
                if (_markColors.Set(target)) return;

                if (SetupInstant(_markColors, _stateColors))
                    _coroutine = _target.StartCoroutine(this);
            }
            public void SetMarkColor(Color target)
            {
                if (_markColors.Set(target)) return;

                if(Setup(_markColors, _stateColors, markDuration))
                    _coroutine = _target.StartCoroutine(this);
            }
            public void SetStateColor(Color target, float duration)
            {
                if (_stateColors.Set(target)) return;

                if (Setup(_stateColors, _markColors, duration))
                    _coroutine = _target.StartCoroutine(this);
            }

            public bool SetTarget(Graphic target)
            {
                if (_coroutine != null) { _target.StopCoroutine(_coroutine); _coroutine = null; _isTwoTask = false; }

                _target = target;
                if (target == null) _renderer = null;
                else _renderer = target.canvasRenderer;

                return _renderer != null;
            }

            public bool MoveNext()
            {
                if (_currentTask.Next(_renderer))
                    return true;

                if(_isTwoTask)
                {
                    _isTwoTask = false;
                    _currentTask = _nextTask;
                    return true;
                }

                _coroutine = null;
                return false;
            }

            public bool Check(Color color) => _renderer.GetColor() != color * _stateColors.current;

            public void Reset() { }

            private bool SetupInstant(Colors main, Colors adv)
            {
                if (_coroutine == null)
                {
                    _renderer.SetColor(_markColors.current * _stateColors.current);
                    return false;
                }

                _target.StopCoroutine(_coroutine); _coroutine = null;

                if (!_isTwoTask & main.id == _currentTask.id)
                {
                    _renderer.SetColor(_markColors.current * _stateColors.current);
                    return false;
                }

                if (main.id == _currentTask.id) _currentTask.Add(_nextTask); 

                Color interim = Color.Lerp(adv.previous, adv.current, _currentTask.progress / _currentTask.duration) * main.current;

                _isTwoTask = false;
                _renderer.SetColor(interim);
                _currentTask.Continue(interim, _markColors.current * _stateColors.current);
                return true;
            }

            private bool Setup(Colors main, Colors adv, float mainDuration)
            {
                if (mainDuration < MIN_DUATION)
                    return SetupInstant(main, adv);

                if (_coroutine == null)
                    return _currentTask.Set(main.id, _renderer.GetColor(), _markColors.current * _stateColors.current, mainDuration);

                _target.StopCoroutine(_coroutine); _coroutine = null;

                float advDuration;
                if (!_isTwoTask)
                {
                    if (main.id == _currentTask.id)
                        return _currentTask.Set(main.id, _renderer.GetColor(), _markColors.current * _stateColors.current, _currentTask.progress);

                    advDuration = _currentTask.duration - _currentTask.progress;
                }
                else
                {
                    mainDuration = _currentTask.progress;
                    advDuration = _currentTask.duration - _currentTask.progress;
                    if (main.id == _currentTask.id) advDuration += _nextTask.duration - _nextTask.progress;
                    else mainDuration += _nextTask.progress;
                }

                float delta = mainDuration - advDuration;
                if (delta > MIN_DELTA_DUATION)
                {
                    TwoTaskSetup(main, adv, mainDuration, advDuration, main.previous);
                    return true;
                }
                if (delta < MIN_DELTA_DUATION_N)
                {
                    TwoTaskSetup(adv, main, advDuration, mainDuration, Color.Lerp(adv.previous, adv.current, _currentTask.progress / _currentTask.duration));
                    return true;
                }

                _isTwoTask = false;
                return _currentTask.Set(main.id, _renderer.GetColor(), _markColors.current * _stateColors.current, mainDuration);
            }

            private void TwoTaskSetup(Colors main, Colors adv, float mainDuration, float advDuration, Color previous)
            {
                Color interim = Color.Lerp(previous, main.current, advDuration / mainDuration) * adv.current;

                _isTwoTask = true;
                _currentTask.Set(adv.id, _renderer.GetColor(), interim, advDuration);
                _nextTask.Set(main.id, interim, _markColors.current * _stateColors.current, mainDuration - advDuration);
            }

            #region Nested: Setting, Task
            //***********************************
            private class Colors
            {
                public int id;
                public Color previous;
                public Color current;

                public Colors(int id)
                {
                    this.id = id;
                    previous = current = Color.white;
                }

                public bool Set(Color color)
                {
                    bool equal = current == color;
                    previous = current;
                    current = color;
#if UNITY_EDITOR
                    return equal && Application.isPlaying;
#else
                    return equal;
#endif
                }
            }
            //***********************************
            private struct Task
            {
                public int id;
                public Color start;
                public Color target;
                public float duration;
                public float progress;

                public bool Next(CanvasRenderer renderer)
                {
                    if (progress < duration)
                    {
                        progress += Time.unscaledDeltaTime;
                        renderer.SetColor(Color.Lerp(start, target, progress / duration));
                        return true;
                    }

                    progress = duration;
                    renderer.SetColor(target);
                    return false;
                }

                public bool Set(int id, Color start, Color target, float duration)
                {
                    this.id = id;
                    this.start = start;
                    this.target = target;
                    this.duration = duration;
                    progress = 0f;
                    return true;
                }

                public void Continue(Color start, Color target)
                {
                    this.start = start;
                    this.target = target;
                    duration -= progress;
                    progress = 0f;
                }

                public void Add(Task task)
                {
                    duration += task.duration;
                    progress += task.progress;
                }
            }
#endregion
        }
    }
}
