using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CSHearth
{
	public class Match
	{
		Player _playerOne;
		Player _playerTwo;

		public Match( Player playerOne, Player playerTwo )
		{
			_playerOne = playerOne;
			_playerTwo = playerTwo;
		}

		public void StartGame()
		{
			_playerOne.Deck.Shuffle();
			_playerTwo.Deck.Shuffle();

			Player goesFirst;
			Player goesSecond;

			// Decide who starts and who gets The Coin
			Random rng = new Random(); 

			int flip = rng.Next( 2 );
			if(flip == 0) {
				goesFirst  = _playerOne;
				goesSecond = _playerTwo;
			}
			else {
				goesFirst  = _playerTwo;
				goesSecond = _playerOne;
			}

			// The player who goes first draws three cards
			for( int i = 0; i < 3; i++ ) {
				goesFirst.DrawCard();
			}

			// The player who goes second draws four cards and The Coin
			for( int i = 0; i < 4; i++ ) {
				goesSecond.DrawCard();
			}

			// TODO: Implement TheCoin
			//goesSecond.getDeck().putCard( new TheCoin() );

			GameState gameState = new GameState(goesFirst, goesSecond);

			List<Action> gameActionList = PlayGame( gameState.Clone() );

			int result = StartReplay( gameState.Clone(), gameActionList );

			if( result == 0 ) {
				Console.WriteLine("The game is a draw!");
			}
			else if( result == 1 ) {
				Console.WriteLine("Player one wins!");
			}
			else if( result == 2 ){
				Console.WriteLine("Player two wins!");
			}
		}

		List<Action> PlayGame( GameState currentTurn )
		{
			var gameLogic = new GameLogic();
			var gameActionList = new List<Action>();

			int turnsTaken = 0;

			while( true )
			{
				gameLogic.PlayTurn( currentTurn );

				++turnsTaken;

				Console.WriteLine("Variations simulated: " + gameLogic.VariationsSimulated);

				GameState endOfTurn = gameLogic.BestGameState;

				Debug.Assert( gameLogic.TurnActionList.Count > 0 );

				gameActionList.AddRange( gameLogic.TurnActionList );

				bool p1IsDead = endOfTurn.GetPlayer(_playerOne.Tag).IsDead();
				bool p2IsDead = endOfTurn.GetPlayer(_playerTwo.Tag).IsDead();

				if( p1IsDead || p2IsDead ) {
					break;
				}

				currentTurn = endOfTurn;

				currentTurn.SwitchTurns();
			}

			return gameActionList;
		}

		int StartReplay( GameState gs, List<Action> actionList )
		{
			var gameLogic = new GameLogic();

			gameLogic.StartOfTurn( gs );

			foreach( Action action in actionList )
			{
				action.PerformAction( gs );

				bool p1IsDead = gs.GetPlayer(_playerOne.Tag).IsDead();
				bool p2IsDead = gs.GetPlayer(_playerTwo.Tag).IsDead();

				if( p1IsDead || p2IsDead )
				{
					if( p1IsDead && p2IsDead ) {
//						Log.Log( "Both players died. It's a draw." );
						return 0;
					}
					else if( p1IsDead ) {
//						Log.Log( "Player one died. Player two is victorious!" );
						return 1;
					}
					else if( p2IsDead ) {
//						Log.Log( "Player two died. Player one is victorious!" );
						return 2;
					}
				}

				if( gs.TurnEnded )
				{
					gs.SwitchTurns();

					gameLogic.StartOfTurn( gs );
				}
			}

			Debug.Assert( false );

			return 0;
		}
	}
}

