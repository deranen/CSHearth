
using System.Collections.Generic;

namespace CSHearth
{
	public class GameState
	{
		public Player Me       { get; private set; }
		public Player Opponent { get; private set; }
		public Board  Board    { get; private set; }

		public GameEventHandler Events { get; private set; }

		public bool TurnEnded { get; set; }

		public List<Action> TurnActionList { get; private set; }

		public GameState(Player me, Player opponent)
		{
			Me       = me;
			Opponent = opponent;
			Board    = new Board();

			Events = new GameEventHandler();

			TurnEnded = false;

			TurnActionList = new List<Action>();
		}

		public GameState Clone()
		{
			GameState clone = (GameState) MemberwiseClone();

			clone.Me       = Me.Clone();
			clone.Opponent = Opponent.Clone();
			clone.Board    = Board.Clone();

			clone.TurnActionList = new List<Action>( TurnActionList );

			for( int i = 0; i < TurnActionList.Count; ++i ) {
				clone.TurnActionList[i] = TurnActionList[i].Clone();
			}

			clone.Events = Events.Clone();

			foreach( Minion oldMinion in Board.GetMinions() ) {
				oldMinion.DeregisterFromEvents( clone.Events );
			}

			// TODO: Deregister secrets

			foreach( Minion newMinion in clone.Board.GetMinions() ) {
				newMinion.RegisterToEvents( clone.Events );
			}

			// TODO: Register secrets

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

