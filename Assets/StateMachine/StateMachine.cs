using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

#nullable enable

namespace StateMachine
{
	public interface IState
	{
		IStateMachine? Parent { get; set; }
		public string Key { get; }
		public void OnEnter(object? @event = null);
		public void Update();
		public void FixedUpdate();
		public void OnLeave();

		public bool HandleEvent(string eventKey, object? eventData = null);
	}

	public abstract class StateBase : IState
	{
		public IStateMachine? Parent { get; set; }
		public string Key { get; }

		protected StateBase(string key)
		{
			Key = key;
		}

		public virtual void OnEnter(object? @event)
		{
		}

		public virtual void Update()
		{
		}

		public virtual void FixedUpdate()
		{
		}

		public virtual void OnLeave()
		{
		}

		public virtual bool HandleEvent(string eventKey, object? eventData = null)
		{
			return false;
		}
	}

	public interface IStateMachine : IState
	{
		public IState? CurrentState { get; }
		public void SetState(string key);
		public void LeaveCurrentState();
	}

	public class StateMachine : StateBase, IStateMachine
	{
		private readonly Dictionary<string, IState> _states;
		private readonly Dictionary<IState, Dictionary<string, IState>> _transitions;
		private readonly Dictionary<IState, List<TransitionTrigger>> _transitionTriggers;

		public IState? CurrentState { get; private set; }

		public StateMachine(string key, Dictionary<string, IState> states, Dictionary<IState, Dictionary<string, IState>> transitions,
			Dictionary<IState, List<TransitionTrigger>> transitionTriggers) : base(key)
		{
			_states = states;
			_transitions = transitions;
			_transitionTriggers = transitionTriggers;
		}

		public override void Update()
		{
			if (CurrentState == null)
				return;

			var matchingTrigger = _transitionTriggers[CurrentState].FirstOrDefault(x => x.Trigger());

			if (matchingTrigger != null)
				SetStateInternal(matchingTrigger.To);

			CurrentState.Update();
		}

		public override void FixedUpdate()
		{
			CurrentState?.FixedUpdate();
		}

		public void SetState(string key)
		{
			if (!_states.ContainsKey(key))
				throw new Exception($"There is no state with key {key}");

			SetStateInternal(_states[key]);
		}

		private void SetStateInternal(IState state)
		{
			CurrentState?.OnLeave();
			CurrentState = state;
			CurrentState.Parent = this;
			CurrentState.OnEnter();
		}

		public void LeaveCurrentState()
		{
			var defaultExitWasHandled = HandleEvent("");

			if (!defaultExitWasHandled)
				throw new Exception("Something requested the current state to end. But couldn't find any");
		}

		/// <summary>
		/// Attempts to handle the event. If the event is not found it is given to the current State in the hope that it is relevant for them.
		/// </summary>
		/// <returns>if anything reacted to the event</returns>
		public override bool HandleEvent(string eventKey, object? eventData = null)
		{
			if (CurrentState == null)
				return false;

			var associatedTransitions = _transitions[CurrentState];

			if (!associatedTransitions.ContainsKey(eventKey))
				return CurrentState?.HandleEvent(eventKey, eventData) ?? false;

			SetStateInternal(associatedTransitions[eventKey]);
			return true;
		}
	}

	public class StateMachineBuilder
	{
		private readonly StateMachineConfiguration _configuration = new StateMachineConfiguration();

		public void AddState(IState state)
		{
			_configuration.States.Add(state.Key, state);
		}

		public void AddEventTransition(string eventKey, string from, string to)
		{
			if (!_configuration.EventTransitions.ContainsKey(from))
				_configuration.EventTransitions[from] = new HashSet<(string @event, string to)>();

			_configuration.EventTransitions[from].Add((eventKey, to));
		}

		public void AddTriggerTransition(string name, string from, string to, Func<bool> trigger)
		{
			if (!_configuration.TriggerTransitions.ContainsKey(from))
				_configuration.TriggerTransitions[from] = new List<(string name, Func<bool> trigger, string to)>();

			_configuration.TriggerTransitions[from].Add((name, trigger, to));
		}

		public IStateMachine Build(string stateMachineKey)
		{
			var states = _configuration.States;
			var transitions = new Dictionary<IState, Dictionary<string, IState>>();
			var triggerTransitions = new Dictionary<IState, List<TransitionTrigger>>();

			foreach (var state in states.Values)
			{
				transitions[state] = new Dictionary<string, IState>();
				triggerTransitions[state] = new List<TransitionTrigger>();
			}

			foreach (var eventTransitionConfiguration in _configuration.EventTransitions)
			{
				var from = states[eventTransitionConfiguration.Key];

				foreach (var (@event, toKey) in eventTransitionConfiguration.Value)
				{
					var to = states[toKey];
					transitions[from].Add(@event, to);
				}
			}

			foreach (var triggerTransitionConfiguration in _configuration.TriggerTransitions)
			{
				var from = states[triggerTransitionConfiguration.Key];

				foreach (var (name, trigger, toKey) in triggerTransitionConfiguration.Value)
				{
					var to = states[toKey];
					triggerTransitions[from].Add(new TransitionTrigger(name, to, trigger));
				}
			}

			return new StateMachine(stateMachineKey, states, transitions, triggerTransitions);
		}
	}

	public abstract class TimedStateBase : StateBase
	{
		protected float WaitedTime = 0f;
		protected readonly float Duration = 0f;

		public TimedStateBase(string key, float duration) : base(key)
		{
			Duration = duration;
		}

		public sealed override void OnEnter(object? @event)
		{
			WaitedTime = 0;
			OnEnterInternal(@event);
		}

		protected virtual void OnEnterInternal(object? @event)
		{
		}

		public sealed override void Update()
		{
			WaitedTime += Time.deltaTime;

			if (WaitedTime >= Duration)
				Parent!.LeaveCurrentState();

			UpdateInternal();
		}

		protected virtual void UpdateInternal()
		{
		}
	}

	public sealed class State : StateBase
	{
		private readonly Action _onUpdate;
		private readonly Action? _onEnter;
		private readonly Action? _onExit;

		public State(string key, Action onUpdate, Action? onEnter = null, Action? onExit = null) : base(key)
		{
			_onUpdate = onUpdate;
			_onEnter = onEnter;
			_onExit = onExit;
		}

		public override void Update()
		{
			_onUpdate();
		}

		public override void OnEnter(object? @event)
		{
			_onEnter?.Invoke();
		}

		public override void OnLeave()
		{
			_onExit?.Invoke();
		}
	}

	public sealed class TimedState : StateBase
	{
		private readonly Action _onUpdate;

		public TimedState(string key, Action onUpdate) : base(key)
		{
			_onUpdate = onUpdate;
		}

		public override void Update()
		{
			_onUpdate();
		}
	}

	public class StateMachineConfiguration
	{
		public Dictionary<string, IState> States = new Dictionary<string, IState>();
		public Dictionary<string, HashSet<(string @event, string to)>> EventTransitions = new Dictionary<string, HashSet<(string @event, string to)>>();

		public Dictionary<string, List<(string name, Func<bool> trigger, string to)>> TriggerTransitions =
			new Dictionary<string, List<(string name, Func<bool> @event, string to)>>();
	}

	public class TransitionTrigger
	{
		public string Name { get; }
		public IState To { get; }
		public Func<bool> Trigger { get; }

		public TransitionTrigger(string name, IState to, Func<bool> trigger)
		{
			To = to;
			Trigger = trigger;
			Name = name;
		}
	}
}