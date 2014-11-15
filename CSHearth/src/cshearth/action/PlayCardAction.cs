using System;

namespace CSHearth
{
	public class PlayCardAction : Action
	{
		readonly int _handPos;

		public PlayCardAction( int handPos )
		{
			_handPos = handPos;
		}

		public override void PerformAction( GameState gs )
		{
			Card card = gs.Me.Hand.GetCard( _handPos );

			throw new NotImplementedException();
		}
	}
}

