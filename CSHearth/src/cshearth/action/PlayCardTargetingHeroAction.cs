using System;

namespace CSHearth
{
	public class PlayCardTargetingHeroAction : Action
	{
		readonly int       _handPos;
		readonly PlayerTag _playerTag;

		public PlayCardTargetingHeroAction( int handPos, PlayerTag playerTag )
		{
			_handPos = handPos;
			_playerTag = playerTag;
		}

		public override void PerformAction( GameState gs )
		{
			Card card = gs.Me.Hand.GetCard( _handPos );

			throw new NotImplementedException();
		}
	}
}

