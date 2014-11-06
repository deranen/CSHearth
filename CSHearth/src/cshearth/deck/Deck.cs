using System;
using System.Collections.Generic;

namespace CSHearth
{
	public class Deck
	{
		public List<Card> Cards { get; private set; }

		public Deck()
		{
			Cards.Capacity = 30;
		}

		public Deck Clone()
		{
			Deck deck = (Deck) MemberwiseClone();

			for( int i = 0; i < Cards.Count; ++i ) {
				deck.Cards[i] = Cards[i].Clone();
			}
		}

		public bool IsEmpty()
		{
			return (Cards.Count == 0);
		}

		public void AddCard( Card card )
		{
			Cards.Add( card );
		}

		public Card DrawCard()
		{
			Card card = Cards[Cards.Count - 1];
			Cards.RemoveAt[Cards.Count - 1];

			return card;
		}

		public void Shuffle()  
		{  
			Random rng = new Random();  
			int n = Cards.Count;

			while( n > 1 ) {
				int k = rng.Next( n );
				--n;
				Card card = Cards[k];
				Cards[k] = Cards[n];
				Cards[n] = card;
			}
		}
	}
}

