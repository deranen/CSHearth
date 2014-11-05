using System;

namespace CSHearth
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Hero heroJaina = new JainaProudmoore();
			Hero heroUther = new UtherLightbringer();

			DeckParser deckParser = new DeckParser( "../../src/dhearth/decks/ExampleDeck0.txt" );
			CardList cardList = deckParser.parseDeck();

			Deck deck0 = new Deck( cardList );

			deckParser = new DeckParser( "../../src/dhearth/decks/ExampleDeck0.txt" );
			cardList = deckParser.parseDeck();

			Deck deck1 = new Deck( cardList );

			Player playerOne = new Player(PlayerId.PLAYER_ONE, heroJaina, deck0);
			Player playerTwo = new Player(PlayerId.PLAYER_TWO, heroUther, deck1);

			Match match = new Match(playerOne, playerTwo);

			match.startGame();
		}
	}
}
