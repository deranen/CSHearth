using System;
using System.Diagnostics;

namespace CSHearth
{
	public class GameEventHandler
	{
		public event EventHandler TurnEnded;
		public event EventHandler<CardPlayedEventArgs> CardPlayed;
		public event EventHandler<AttackEventArgs>     Attack;

		public GameEventHandler Clone()
		{
			GameEventHandler clone = (GameEventHandler) MemberwiseClone();

			return clone;
		}

		public void OnTurnEnded( GameState gs, EventArgs e )
		{
			if( TurnEnded != null )
				TurnEnded( gs, e );
		}

		public void OnCardPlayed( GameState gs, CardPlayedEventArgs e )
		{
			if( CardPlayed != null )
				CardPlayed( gs, e );
		}

		public void OnAttack( GameState gs, AttackEventArgs e )
		{
			if( Attack != null )
				Attack( gs, e );
		}
	}

	public class CardPlayedEventArgs : EventArgs
	{
		public int? HandPos          { get; set; }
		public int? BoardPos         { get; set; }
		public int? MinionTarget     { get; set; }
		public PlayerTag MinionController { get; set; }
		public PlayerTag HeroTarget       { get; set; }

		public CardPlayedEventArgs()
		{
			HandPos      = null;
			BoardPos     = null;
			MinionTarget = null;
			MinionController = PlayerTag.None;
			HeroTarget       = PlayerTag.None;
		}

		public bool PlayedMinion()
		{
			return BoardPos != null;
		}

		public bool TargetedMinion()
		{
			if( MinionTarget != null ) {
				if( MinionController == PlayerTag.None ) {
					Debug.Assert( false );
				}
				return true;
			}
			return false;
		}

		public bool TargetedHero()
		{
			return HeroTarget != PlayerTag.None;
		}
	}

	public class AttackEventArgs : EventArgs
	{
		public PlayerTag AttackerPlayerTag { get; set; }
		public PlayerTag DefenderPlayerTag { get; set; }
		public int? AttackerBoardPos { get; set; }
		public int? DefenderBoardPos { get; set; }

		public AttackEventArgs()
		{
			AttackerPlayerTag = PlayerTag.None;
			DefenderPlayerTag = PlayerTag.None;
			AttackerBoardPos = null;
			DefenderBoardPos = null;
		}

		public bool HeroAttacks()
		{
			return AttackerPlayerTag != PlayerTag.None;
		}

		public bool HeroDefends()
		{
			return DefenderPlayerTag != PlayerTag.None;
		}

		public bool MinionAttacks()
		{
			return AttackerBoardPos != null;
		}

		public bool MinionDefends()
		{
			return DefenderBoardPos != null;
		}
	}
}

