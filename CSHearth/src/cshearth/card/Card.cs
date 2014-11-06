using System;

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

		public bool NeedsTarget    { get; private set; }
		public bool MustHaveTarget { get; private set; }

		private Card( CardTag tag, CardClass cardClass, CardType cardType, int cost)
		{
			Id    = Session.GetUniqueId();
			Tag   = tag;
			Class = cardClass;
			Type  = cardType;
			Cost  = cost;

			NeedsTarget    = false;
			MustHaveTarget = false;
		}

		public abstract Card Clone();

//		private void payManaCost( GameState gs )
//		{
//			gs.me.availableMana = gs.me.availableMana - cost;
//			assert( gs.me.availableMana >= 0 );
//		}
//
//		private void removeFromHand( GameState gs )
//		{
//			gs.me.hand.removeCard( this );
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
//		bool canTarget( Card card ) const
//		{
//			return false;
//		}
//
//		bool canTarget( Hero hero ) const
//		{
//			return false;
//		}
	}
}

