
using System.Diagnostics;
using System.Collections.Generic;

namespace CSHearth
{
	public class Hand
	{
		List<Card> _cards;

		public Hand()
		{
			_cards = new List<Card>();
			_cards.Capacity = 10;
		}

		public Hand Clone()
		{
			Hand hand = (Hand) MemberwiseClone();

			hand._cards = new List<Card>( _cards );

			for( int i = 0; i < _cards.Count; ++i ) {
				hand._cards[i] = _cards[i].Clone();
			}

			return hand;
		}

		public bool Empty
		{
			get { return (_cards.Count == 0); }
		}

		public int CardCount
		{
			get { return _cards.Count; }
		}

		public void AddCard( Card card )
		{
			_cards.Add( card );
		}

		public Card GetCard( int handPos )
		{
			return _cards[handPos];
		}

		public void RemoveCard( int handPos )
		{
			_cards.RemoveAt( handPos );
		}

		public void RemoveCard( Card card )
		{
			bool cardRemoved = false;

			for( int i = 0; i < _cards.Count; ++i )
			{
				Card handCard = GetCard( i );

				if( card.Id == handCard.Id ) {
					RemoveCard( i );
					cardRemoved = true;
					break;
				}
			}

			Debug.Assert( cardRemoved );
		}
	}
}

