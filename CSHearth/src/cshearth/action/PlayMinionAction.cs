using System;
using System.Diagnostics;

namespace CSHearth
{
	public class PlayMinionAction : Action
	{
		readonly int _handPos;
		readonly int _boardPos;

		public PlayMinionAction( int handPos, int boardPos )
		{
			_handPos = handPos;
			_boardPos = boardPos;
		}

		public override void PerformAction( GameState gs )
		{
			Minion minion = (Minion) gs.Me.Hand.GetCard( _handPos );

			var eventArgs = new CardPlayedEventArgs {
				HandPos = _handPos,
				BoardPos = _boardPos
			};
			gs.Events.OnCardPlayed( gs, eventArgs );

			gs.Me.Mana -= minion.Cost;

			Debug.Assert( gs.Me.Mana >= 0 );

			gs.Me.Hand.RemoveCard( _handPos );
			gs.Board.PutMinion( minion, _boardPos, gs.Me, gs.Events );

			minion.PlayedThisTurn = true;
		}
	}
}

