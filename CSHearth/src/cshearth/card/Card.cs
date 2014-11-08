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

		public CardTag   Tag   { get; private set; }
		public CardClass Class { get; private set; }
		public CardType  Type  { get; private set; }

		public int Cost { get; private set; }

		public bool NeedsBoardPosition { get; private set; }
		public bool NeedsTarget        { get; private set; }
		public bool MustHaveTarget     { get; private set; }

		protected Card( CardTag tag, CardClass cardClass, CardType cardType, int cost)
		{
			Id    = Session.GetUniqueId();
			Tag   = tag;
			Class = cardClass;
			Type  = cardType;
			Cost  = cost;

			NeedsBoardPosition = false;
			NeedsTarget        = false;
			MustHaveTarget     = false;
		}

		public abstract Card Clone();

//		private void PayManaCost( GameState gs )
//		{
//			gs.Me.Mana = gs.Me.Mana - Cost;
//			Debug.Assert( gs.Me.Mana >= 0 );
//		}
//
//		private void RemoveFromHand( GameState gs )
//		{
//			gs.Me.Hand.RemoveCard( this );
//		}
//
//		protected abstract
//		void putIntoPlay( size_t boardPos, GameState gs );
//
//		void playCard( size_t boardPos, Player player, GameState gs )
//		{
//			payManaCost( gs );
//			removeFromHand( gs );
//			putIntoPlay( boardPos, gs );
//		}
//
//		void playCard( size_t boardPos, GameState gs )
//		{
//			logPlayCard();
//
//			payManaCost( gs );
//			removeFromHand( gs );
//			putIntoPlay( boardPos, gs );
//		}
//
		protected virtual bool CanTarget( Card card )
		{
			return false;
		}

		protected virtual bool CanTarget( Hero hero )
		{
			return false;
		}
	}
}

