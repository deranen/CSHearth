using System;
using System.Diagnostics;
using System.Collections;

namespace CSHearth
{
	public enum CardClass
	{
		Neutral, Warrior, Shaman, Rogue, Paladin, Hunter, Druid, Warlock, Mage, Priest
	}

	public enum Target { None, Needed, Forced }

	[Flags]
	public enum EventTag {
		None            = 0,
		TurnEndedEvent  = 1,
		CardPlayedEvent = 2,
		AttackEvent     = 4,
	}

	public abstract class Card
	{
		public int Id { get; private set; }

		public string    Name  { get; private set; }
		public CardClass Class { get; private set; }
		public int       Cost  { get; private set; }

		public Target Target { get; private set; }

		protected EventTag RegisteredEvents { get; set; }

		protected Card( string name, int cost)
		{
			Id    = Session.GetUniqueId();
			Name  = name;
			Class = CardClass.Neutral;
			Cost  = cost;

			RegisteredEvents = EventTag.None;
			Target           = Target.None;
		}

		public virtual Card Clone()
		{
			Card card = (Card) MemberwiseClone();

			return card;
		}

		public override int GetHashCode()
		{
			int hash = Hasher.InitialHash();

			hash = Hasher.CombineHash( hash, Cost.GetHashCode() );
			hash = Hasher.CombineHash( hash, RegisteredEvents.GetHashCode() );

			return hash;
		}

		public void RegisterToEvents( GameEventHandler eh )
		{
			if( RegisteredEvents.HasFlag( EventTag.TurnEndedEvent ) )
				eh.TurnEnded += HandleTurnEnded;
			if( RegisteredEvents.HasFlag( EventTag.CardPlayedEvent ) )
				eh.CardPlayed += HandleCardPlayed;
			if( RegisteredEvents.HasFlag( EventTag.AttackEvent ) )
				eh.Attack += HandleAttack;
		}

		public void DeregisterFromEvents( GameEventHandler eh )
		{
			if( RegisteredEvents.HasFlag( EventTag.TurnEndedEvent ) )
				eh.TurnEnded -= HandleTurnEnded;
		}

		protected virtual void HandleTurnEnded( object sender, EventArgs e )
		{
			throw new NotImplementedException();
		}

		protected virtual void HandleCardPlayed( object sender, CardPlayedEventArgs e )
		{
			throw new NotImplementedException();
		}

		protected virtual void HandleAttack( object sender, AttackEventArgs e )
		{
			throw new NotImplementedException();
		}

		public virtual bool CanTarget( Card card )
		{
			return false;
		}

		public virtual bool CanTarget( Hero hero )
		{
			return false;
		}
	}
}

