using System;

namespace ImperitWASM.Shared.Data
{
	public sealed record Game
	{
		public enum State { Created, CountDown, Started, Finished }
		public long Id { get; private set; }
		public State Current { get; private set; }
		public DateTime StartTime { get; private set; }
		public DateTime FinishTime { get; private set; }
		public Game() => (Id, Current, StartTime, FinishTime) = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), State.Created, DateTime.MaxValue, DateTime.MaxValue);

		public Game CountDown(DateTime start) => this with { Current = State.CountDown, StartTime = start };
		public Game Start() => this with { Current = State.Started };
		public Game Finish() => this with { Current = State.Finished, FinishTime = DateTime.UtcNow };

		public bool Created => Current == State.Created;
		public bool CountingDown => Current == State.CountDown;
		public bool Started => Current == State.Started;
		public bool Finished => Current == State.Finished;
	}
}
