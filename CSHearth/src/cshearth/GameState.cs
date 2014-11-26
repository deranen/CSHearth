using System;
using System.Collections.Generic;

namespace CSHearth
{
	public class GameState
	{
		public Player Me       { get; private set; }
		public Player Opponent { get; private set; }
		public Board  Board    { get; private set; }

		public GameEventHandler EventsPersistent    { get; private set; }
		public GameEventHandler EventsNonPersistent { get; private set; }

		public GameEventHandlerHelper Events { get; private set; }

		public bool TurnEnded { get; set; }

		public List<Action> TurnActionList { get; private set; }

		public GameState(Player me, Player opponent)
		{
			Me       = me;
			Opponent = opponent;
			Board    = new Board();

			EventsPersistent    = new GameEventHandler();
			EventsNonPersistent = new GameEventHandler();

			Events = new GameEventHandlerHelper( this );

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

			clone.EventsNonPersistent = new GameEventHandler();
			clone.Events = new GameEventHandlerHelper( clone );

			foreach( Minion clonedMinion in clone.Board.GetMinions() ) {
				clonedMinion.RegisterToEvents( clone.EventsNonPersistent );
			}

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

		public class GameEventHandlerHelper
		{
			readonly GameState _gs;

			public GameEventHandlerHelper( GameState gs ) {
				_gs = gs;
			}

			public void OnTurnEnded( GameState gs, EventArgs e )
			{
				_gs.EventsPersistent.OnTurnEnded( gs, e );
				_gs.EventsNonPersistent.OnTurnEnded( gs, e );
			}

			public void OnAttack( GameState gs, AttackEventArgs e )
			{
				_gs.EventsPersistent.OnAttack( gs, e );
				_gs.EventsNonPersistent.OnAttack( gs, e );
			}

			public void OnCardPlayed( GameState gs, CardPlayedEventArgs e )
			{
				_gs.EventsPersistent.OnCardPlayed( gs, e );
				_gs.EventsNonPersistent.OnCardPlayed( gs, e );
			}
		}
	}
}

