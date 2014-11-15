using System;
using System.Diagnostics;

namespace CSHearth
{
	public enum CardType
	{
		Spell, Minion, Weapon
	}

	public enum CardClass
	{
		Neutral, Warrior, Shaman, Rogue, Paladin, Hunter, Druid, Warlock, Mage, Priest
	}

	public abstract class Card
	{
		public int Id { get; private set; }

		public CardType  Type  { get; private set; }
		public CardClass Class { get; private set; }
		public CardTag   Tag   { get; private set; }

		public int Cost { get; private set; }

		public bool NeedsTarget        { get; protected set; }
		public bool MustHaveTarget     { get; protected set; }

		protected Card( CardType cardType, CardClass cardClass, CardTag tag, int cost)
		{
			Id    = Session.GetUniqueId();
			Type  = cardType;
			Class = cardClass;
			Tag   = tag;
			Cost  = cost;

			NeedsTarget        = false;
			MustHaveTarget     = false;
		}

		public virtual Card Clone()
		{
			Card card = (Card) MemberwiseClone();

			return card;
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

