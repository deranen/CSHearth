using System;
using System.Diagnostics;

namespace CSHearth
{
	public enum CardClass
	{
		Neutral, Warrior, Shaman, Rogue, Paladin, Hunter, Druid, Warlock, Mage, Priest
	}

	public enum Target { None, Needed, Forced }

	[Flags]
	public enum EventRegistration {
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

		protected EventRegistration EventsRegistered { get; set; }

		protected Card( string name, int cost)
		{
			Id    = Session.GetUniqueId();
			Name  = name;
			Class = CardClass.Neutral;
			Cost  = cost;

			EventsRegistered = EventRegistration.None;
			Target           = Target.None;
		}

		public virtual Card Clone()
		{
			Card card = (Card) MemberwiseClone();

			return card;
		}

		public void RegisterToEvents( GameEventHandler eh )
		{
			if( EventsRegistered.HasFlag( EventRegistration.TurnEndedEvent ) )
				eh.TurnEnded += HandleTurnEnded;
		}

		public void DeregisterFromEvents( GameEventHandler eh )
		{
			if( EventsRegistered.HasFlag( EventRegistration.TurnEndedEvent ) )
				eh.TurnEnded -= HandleTurnEnded;
		}

		protected virtual void HandleTurnEnded( object sender, EventArgs e )
		{
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

