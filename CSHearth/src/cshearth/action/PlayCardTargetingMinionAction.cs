using System;

namespace CSHearth
{
	public class PlayCardTargetingMinionAction : Action
	{
		readonly int _handPos;
		readonly int _minionPos;
		readonly PlayerTag _controllerTag;

		public PlayCardTargetingMinionAction( int handPos, int minionPos, PlayerTag controllerTag )
		{
			_handPos = handPos;
			_minionPos = minionPos;
			_controllerTag = controllerTag;
		}

		public override void PerformAction( GameState gs )
		{
			Card card = gs.Me.Hand.GetCard( _handPos );

			throw new NotImplementedException();
		}
	}
}

