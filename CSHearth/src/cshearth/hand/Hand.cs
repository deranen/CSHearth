
using System.Diagnostics;
using System.Collections.Generic;

namespace CSHearth
{
	public class Hand
	{
		public List<Card> Cards { get; private set; }

		public Hand()
		{
			Cards.Capacity = 10;
		}

		public Hand Clone()
		{
			Hand hand = (Hand) MemberwiseClone();

			for( int i = 0; i < Cards.Count; ++i ) {
				hand.Cards[i] = Cards[i].Clone();
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

		public Card GetCard( int index )
		{
			return Cards[index];
		}

		public void RemoveCard( int index )
		{
			Cards.RemoveAt( index );
		}

		public void RemoveCard( Card card )
		{
			bool cardRemoved = false;

			for( int i = 0; i < Cards.Count; ++i )
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

