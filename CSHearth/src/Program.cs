using System;
using System.Collections.Generic;

namespace CSHearth
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Hero heroJaina = new JainaProudmoore();
			Hero heroUther = new UtherLightbringer();

			DeckParser deckParser = new DeckParser( "ExampleDeck0.txt" );
			List<string> cardList = deckParser.ParseDeck();

			Deck deck0 = new Deck( cardList );

			deckParser = new DeckParser( "ExampleDeck0.txt" );
			cardList = deckParser.ParseDeck();

			Deck deck1 = new Deck( cardList );

			Player playerOne = new Player(PlayerTag.PlayerOne, heroJaina, deck0);
			Player playerTwo = new Player(PlayerTag.PlayerTwo, heroUther, deck1);

			IArtificalIntelligence ai = new BasicAI();
			Match match = new Match(playerOne, playerTwo, ai);

			match.StartGame();
		}
	}
}
