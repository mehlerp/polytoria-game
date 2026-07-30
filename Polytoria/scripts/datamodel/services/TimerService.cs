// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

// using Godot;
using Polytoria.Attributes;
// using Polytoria.Enums;
using Polytoria.Scripting;
// using Polytoria.Shared;
// using Polytoria.Utils;
// using System;
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
				_duration = value;
				// TODO: clamp progress value
				// TODO: call finished callback properly
			}
		}

		[ScriptProperty]
		public float ElapsedTime
		{
			get => _progress;
			set
			{
				_progress = value;
				// TODO: clamp progress value
				// TODO: call finished callback properly
			}
		}

		[ScriptProperty]
		public float RemainingTime
		{
			get => _duration - _progress;
			set
			{
				// TODO: clamp progress value
				// TODO: call finished callback properly
			}
		}

		[ScriptProperty] public bool IsRunning => _running && !_paused;
		[ScriptProperty] public bool IsPaused => _paused;

		// TODO: should return how many times the timer has finished since the last update tick
		[ScriptProperty]
		public PTSignal<int> Finished { get; private set; } = new();

		[ScriptMethod]
		public void Start()
		{
			_running = true;
			_paused = false;
			// TODO: start running the timer
		}

		[ScriptMethod]
		public void Pause()
		{
			if (_running)
			{
				_paused = true;
				// TODO: pause the timer
			}
		}

		[ScriptMethod]
		public void Stop()
		{
			_running = false;
			_paused = false;
			_progress = 0;
			// TODO: stop the timer
		}
	}
}
