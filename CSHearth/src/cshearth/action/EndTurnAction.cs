using System;

namespace CSHearth
{
	public class EndTurnAction : Action
	{
		public override void PerformAction( GameState gs )
		{
			// TODO: Trigger end of turn stuff here

			Events.OnTurnEnded( gs, EventArgs.Empty );

			gs.TurnEnded = true;
		}
	}
}

