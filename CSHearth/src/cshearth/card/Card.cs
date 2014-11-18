using System;
using System.Diagnostics;

namespace CSHearth
{
	public enum CardClass
	{
		Neutral, Warrior, Shaman, Rogue, Paladin, Hunter, Druid, Warlock, Mage, Priest
	}

	public enum Target { None, Needed, Forced }

	public abstract class Card
	{
		public int Id { get; private set; }

		public string    Name  { get; private set; }
		public CardClass Class { get; private set; }
		public int       Cost  { get; private set; }

		public Target Target { get; private set; }

		protected Card( string name, int cost)
		{
			Id    = Session.GetUniqueId();
			Name  = name;
			Class = CardClass.Neutral;
			Cost  = cost;

			Target = Target.None;
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

