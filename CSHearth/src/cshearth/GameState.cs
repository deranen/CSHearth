using System;

namespace CSHearth
{
	public class GameState
	{
		public Player Me       { get; private set; }
		public Player Opponent { get; private set; }
		public Board  Board    { get; private set; }

		public bool TurnEnded { get; set; }

		public GameState(Player me, Player opponent)
		{
			Me       = me;
			Opponent = opponent;
			Board    = new Board();

			TurnEnded = false;
		}

		public GameState Clone()
		{
			GameState clone = (GameState) MemberwiseClone();

			clone.Me       = Me.Clone();
			clone.Opponent = Opponent.Clone();
			clone.Board    = Board.Clone();

			return clone;
		}

		public Player GetPlayer( PlayerTag tag )
		{
			return (tag == Me.Tag ? Me : Opponent);
		}

		public void SwitchTurns()
		{
			Player temp = Me;
			Me = Opponent;
			Opponent = temp;
		}
	}
}

