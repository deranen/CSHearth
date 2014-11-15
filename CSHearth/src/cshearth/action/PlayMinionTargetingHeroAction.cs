using System;

namespace CSHearth
{
	public class PlayMinionTargetingHeroAction : Action
	{
		readonly int _handPos;
		readonly int _boardPos;
		readonly PlayerTag _player;

		public PlayMinionTargetingHeroAction( int handPos, int boardPos, PlayerTag player )
		{
			_handPos = handPos;
			_boardPos = boardPos;
			_player = player;
		}

		public override void PerformAction( GameState gs )
		{
			Minion minion = (Minion) gs.Me.Hand.GetCard( _handPos );

			throw new NotImplementedException();
		}
	}
}

