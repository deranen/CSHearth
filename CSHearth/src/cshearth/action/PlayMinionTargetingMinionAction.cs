using System;

namespace CSHearth
{
	public class PlayMinionTargetingMinionAction : Action
	{
		readonly int _handPos;
		readonly int _boardPos;
		readonly int _targetMinionPos;
		readonly PlayerTag _targetMinionController;

		public PlayMinionTargetingMinionAction(
			int handPos, int boardPos, int targetMinionPos, PlayerTag targetMinionController )
		{
			_handPos = handPos;
			_boardPos = boardPos;
			_targetMinionPos = targetMinionPos;
			_targetMinionController = targetMinionController;
		}

		public override void PerformAction( GameState gs )
		{
			Minion minion = (Minion) gs.Me.Hand.GetCard( _handPos );

			throw new NotImplementedException();
		}
	}
}

