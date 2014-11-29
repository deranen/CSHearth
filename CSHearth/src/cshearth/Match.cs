using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Timers;

namespace CSHearth
{
	public class Match
	{
		readonly Player _playerOne;
		readonly Player _playerTwo;

		readonly GameLogic _gameLogic;

		public Match( Player playerOne, Player playerTwo, IArtificalIntelligence ai )
		{
			_playerOne = playerOne;
			_playerTwo = playerTwo;

			_gameLogic = new GameLogic( ai );
		}

		public void StartGame()
		{
			_playerOne.Deck.Shuffle();
			_playerTwo.Deck.Shuffle();

			Player goesFirst;
			Player goesSecond;

			// Decide who starts and who gets The Coin

			int flip = Session.RNG.Next( 2 );
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

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			List<Action> gameActionList = PlayGame( gameState.Clone() );

			stopwatch.Stop();

			Console.WriteLine( "Simulation time: " + stopwatch.Elapsed );

			//StartReplay( gameState.Clone(), gameActionList );
		}

		List<Action> PlayGame( GameState gs )
		{
			var gameActionList = new List<Action>();

			EventLogger simulationLogger = new EventLogger( gs.Events, "SimulationLog.txt" );
			simulationLogger.LogActions = false;
			simulationLogger.Enabled = false;

			simulationLogger.LogLine( "The seed for this match is: " + Session.Seed );

			int turnsTaken = 0;

			simulationLogger.LogGameState( gs );

			while( true )
			{
				_gameLogic.PlayTurn( gs );

				++turnsTaken;

				Debug.Assert( turnsTaken < 200 );

				Console.WriteLine("Variations simulated: " + _gameLogic.VariationsSimulated);

				gs = _gameLogic.BestGameState;

				Debug.Assert( gs.TurnActionList.Count > 0 );

				simulationLogger.LogGameState( gs );

				gameActionList.AddRange( gs.TurnActionList );
				gs.TurnActionList.Clear();

				bool p1IsDead = gs.GetPlayer(_playerOne.Tag).IsDead();
				bool p2IsDead = gs.GetPlayer(_playerTwo.Tag).IsDead();

				if( p1IsDead || p2IsDead ) {
					break;
				}

				gs.SwitchTurns();
			}

			return gameActionList;
		}

		int StartReplay( GameState gs, List<Action> actionList )
		{
			EventLogger eventLogger = new EventLogger( gs.Events, "GameLog.txt" );

			eventLogger.LogLine( "The seed for this match is: " + Session.Seed );

			eventLogger.LogGameState( gs );

			_gameLogic.StartOfTurn( gs );

			eventLogger.LogGameState( gs );

			foreach( Action action in actionList )
			{
				action.PerformAction( gs );

				if( eventLogger.Interactive ) {
					Console.ReadKey();
				}

				bool p1IsDead = gs.GetPlayer(_playerOne.Tag).IsDead();
				bool p2IsDead = gs.GetPlayer(_playerTwo.Tag).IsDead();

				if( p1IsDead || p2IsDead )
				{
					eventLogger.LogGameState( gs );

					if( p1IsDead && p2IsDead ) {
						eventLogger.LogLine( "The game is a draw!" );
						return 0;
					}
					else if( p2IsDead ) {
						eventLogger.LogLine( "Player one wins!" );
						return 1;
					}
					else if( p1IsDead ) {
						eventLogger.LogLine( "Player two wins!" );
						return 2;
					}
				}

				if( gs.TurnEnded )
				{
					gs.SwitchTurns();

					_gameLogic.StartOfTurn( gs );
				}

				eventLogger.LogGameState( gs );
			}

			Debug.Assert( false );

			return 0;
		}
	}
}

