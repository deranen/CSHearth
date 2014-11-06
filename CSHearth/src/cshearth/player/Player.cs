using System;

namespace CSHearth
{
	public class Player
	{
		public Hero Hero { get; private set; }
		public Deck Deck { get; private set; }
		public Hand Hand { get; private set; }

		public PlayerTag Tag { get; private set; }

		public int Mana    { get; set; }
		public int MaxMana { get; set; }

		public Player( PlayerTag playerTag, Hero hero, Deck deck )
		{
			Tag  = playerTag;
			Hero = hero;
			Deck = deck;
			Hand = new Hand();

			Mana    = 0;
			MaxMana = 0;
		}

		public Player Clone()
		{
			Player clone = (Player) MemberwiseClone();
			clone.Hero = Hero.Clone();
			clone.Deck = Deck.Clone();
			clone.Hand = Hand.Clone();
		}

		public void IncreaseMaxMana()
		{
			if( MaxMana < 10 ) {
				++MaxMana;
			}
		}

		public void RestoreMana()
		{
			Mana = MaxMana;
		}

		public void DrawCard()
		{
			if( Deck.IsEmpty() ) {
				// TODO: Fatigue
			}
			else {
				Card card = Deck.DrawCard();

				// TODO: Handle full hand

				Hand.AddCard( card );
			}
		}

		public bool IsDead()
		{
			return Hero.IsDead();
		}
	}
}

