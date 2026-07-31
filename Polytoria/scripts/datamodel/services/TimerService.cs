// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

// using Godot;
using Polytoria.Attributes;
// using Polytoria.Enums;
using Polytoria.Scripting;
// using Polytoria.Shared;
// using Polytoria.Utils;
using System;
// using System.Collections.Generic;

namespace Polytoria.Datamodel.Services;

[Static("Timer"), ExplorerExclude, SaveIgnore]
public sealed partial class TimerService : Instance
{
	[ScriptMethod]
	public TimerObject New()
	{
		TimerObject timer = new();
		return timer;
	}


	public override void Init()
	{
		base.Init();
		SetProcess(true);
	}

	public override void Process(double delta)
	{
		// TODO: advance timers
		base.Process(delta);
	}


	public class TimerObject : IScriptObject
	{
		private bool _running = false;
		private bool _paused = false;
		private bool _looped = false;
		private float _duration = 1;
		private float _progress = 0;
		private float _speedScale = 1;

		[ScriptProperty]
		public bool Looped
		{
			get => _looped;
			set
			{
				_looped = value;
			}
		}

		[ScriptProperty]
		public float SpeedScale
		{
			get => _speedScale;
			set
			{
				_speedScale = value;
			}
		}

		[ScriptProperty]
		public float Duration
		{
			get => _duration;
			set
			{
				_duration = MathF.Max(value, 0);
				InvokeSpilledTime();
			}
		}

		[ScriptProperty]
		public float ElapsedTime
		{
			get => _progress;
			set
			{
				_progress = MathF.Max(value, 0);
				InvokeSpilledTime();
			}
		}

		[ScriptProperty]
		public float RemainingTime
		{
			get => _duration - _progress;
			set
			{
				_progress =  _duration - MathF.Min(value, _duration);
				InvokeSpilledTime();
			}
		}

		[ScriptProperty] public bool IsRunning => _running && !_paused;
		[ScriptProperty] public bool IsPaused => _paused;
		[ScriptProperty] public bool IsStopped => !_running;

		// TODO: should return how many times the timer has finished since the last update tick
		[ScriptProperty]
		public PTSignal<float> Finished { get; private set; } = new();


		// TODO: maybe remove timer if garbage collected?


		private void InvokeSpilledTime()
		{
			if (_progress >= _duration)
			{
				float times = _progress / _duration;
				_progress = _duration <= 0 ? 0 : _progress % _duration;
				Finished.Invoke(times);
			}
		}


		[ScriptMethod]
		public void Start()
		{
			if (!_running || _paused)
			{
				_running = true;
				_paused = false;
				// TODO: add to running timer list
			}
		}

		[ScriptMethod]
		public void Pause()
		{
			if (_running && !_paused)
			{
				_paused = true;
				// TODO: remove from running timer list
			}
		}

		[ScriptMethod]
		public void Stop()
		{
			if (_running)
			{
				_running = false;
				_paused = false;
				_progress = 0;
				// TODO: remove from running timer list
			}
		}
	}
}
