using System.Collections.Generic;

namespace CSHearth
{
	public class GameLogic
	{
		IArtificalIntelligence _ai;

		HashSet<int> _gameStateHashes;
		HashSet<int> _responseHashes;

		public double    BestScore     { get; private set; }
		public GameState BestGameState { get; private set; }

		public double BestResponseScore { get; private set; }

		public int VariationsSimulated { get; private set; }

		public GameLogic( IArtificalIntelligence ai )
		{
			_ai = ai;

			_gameStateHashes = new HashSet<int>();
			_responseHashes = new HashSet<int>();

			BestScore     = double.NegativeInfinity;
			BestGameState = null;

			VariationsSimulated = 0;
		}

		public void StartOfTurn( GameState gs )
		{
			if( !gs.SimulatingResponse )
			{
				BestScore = double.NegativeInfinity;
				BestGameState = null;

				_gameStateHashes.Clear();

				VariationsSimulated = 0;
			}

			gs.TurnEnded = false;

			gs.Me.IncreaseMaxMana();
			gs.Me.RestoreMana();

			foreach( Minion minion in gs.Board.GetMinions(gs.Me) ) {
				minion.PlayedThisTurn = false;
				minion.AttackCount = 0;
			}

			gs.Me.DrawCard();
		}

		public void PlayTurn( GameState gs )
		{
			StartOfTurn( gs );
			PlayTurnRecursive( gs );
		}

		void PlayTurnRecursive( GameState gs )
		{
			if( gs.TurnEnded || gs.Me.IsDead() || gs.Opponent.IsDead() )
			{
				if( !gs.SimulatingResponse &&
					!gs.Me.IsDead() && !gs.Opponent.IsDead() )
				{
					// Simulate opponents response
					double scoreBefore = BestScore;

					_responseHashes.Clear();

					GameState oppoTurn = gs.Clone();

					oppoTurn.SimulatingResponse = true;
					oppoTurn.SwitchTurns();
					PlayTurn( oppoTurn );

					_responseHashes.Clear();

					double scoreAfter = BestScore;

					if( scoreAfter > scoreBefore ) {
						BestGameState = gs;
					}
				} else {
					double score = _ai.CalculateScore( gs );

					// If we are simulating the response of the opponent
					// the score value is from the perspective of the opponent.
					// To get our score we need to negate its value
					if( gs.SimulatingResponse ) {
						score = -score;
					}

					if( score > BestScore )
					{
						BestScore = score;

						if( !gs.SimulatingResponse ) {
							BestGameState = gs;
						}
					}

					++VariationsSimulated;
				}

				return;
			}

			int hash = gs.GetHashCode();

			if( gs.SimulatingResponse ) {
				if( !_responseHashes.Add( hash ) ) {
					return;
				}
			} else {
				if( !_gameStateHashes.Add( hash ) ) {
					return;
				}
			}

			SimulateEndTurn( gs );

			SimulateAttackWithMinions( gs );

			// We don't simulate playing cards when simulating
			// the opponent's response to your ending board state
			if( !gs.SimulatingResponse ) {
				SimulatePlayCards( gs );
			}

		}

		void SimulateEndTurn( GameState gs )
		{
			Action action = new EndTurnAction();
			PerformAction( action, gs );
		}

		void SimulateAttackWithMinions( GameState gs )
		{
			int myMinionCount = gs.Board.GetMinionCount( gs.Me );

			for( int myMinionPos = 0; myMinionPos < myMinionCount; ++myMinionPos )
			{
				Minion myMinion = gs.Board.GetMinion( gs.Me, myMinionPos );

				if( !myMinion.CanAttack() ) {
					continue;
				}

				int oppoMinionCount = gs.Board.GetMinionCount( gs.Opponent );

				for( int oppoMinionPos = 0; oppoMinionPos < oppoMinionCount; ++oppoMinionPos )
				{
					Minion oppoMinion = gs.Board.GetMinion( gs.Opponent, oppoMinionPos );

					// TODO: Handle taunt

					Action action = new AttackMinionWithMinionAction( myMinionPos, oppoMinionPos );
					PerformAction( action, gs );
				}

				// TODO: Handle taunt

				{
					Action action = new AttackHeroWithMinionAction( myMinionPos );
					PerformAction( action, gs );
				}
			}
		}

		void SimulatePlayCards( GameState gs )
		{
			int cardCount = gs.Me.Hand.CardCount;

			for( int handPos = 0; handPos < cardCount; ++handPos )
			{
				Card card = gs.Me.Hand.GetCard( handPos );

				if( !CanAffordToPlay( card, gs ) ) {
					continue;
				}

				if( card.Target == Target.Needed )
				{
					int availableTargets = 0;

					availableTargets += TargetMinionsOnBoard( card, handPos, gs.Me, gs );
					availableTargets += TargetMinionsOnBoard( card, handPos, gs.Opponent, gs );

					availableTargets += TargetHero( card, handPos, gs.Me, gs );
					availableTargets += TargetHero( card, handPos, gs.Opponent, gs );

					if( availableTargets == 0 && card.Target != Target.Forced ) {
						PlayCardWithoutTarget( card, handPos, gs );
					}
				}
				else {
					PlayCardWithoutTarget( card, handPos, gs );
				}
			}
		}

		int TargetMinionsOnBoard( Card card, int cardHandPos, Player player, GameState gs )
		{
			int availableTargets = 0;

			int minionCount = gs.Board.GetMinionCount( player );

			for( int targetBoardPos = 0; targetBoardPos < minionCount; ++targetBoardPos )
			{
				Minion targetMinion = gs.Board.GetMinion( player, targetBoardPos );

				if( !card.CanTarget( targetMinion ) ) {
					continue;
				}

				++availableTargets;

				if( card is Minion )
				{
					int availablePosCount = gs.Board.GetFreeBoardPositionCount( gs.Me );

					for( int boardPos = 0; boardPos < availablePosCount; ++boardPos )
					{
						Action action = new PlayMinionTargetingMinionAction( cardHandPos, boardPos, targetBoardPos, player.Tag );

						PerformAction( action, gs );
					}
				}
				else {
					Action action = new PlayCardTargetingMinionAction( cardHandPos, targetBoardPos, player.Tag );

					PerformAction( action, gs );
				}
			}

			return availableTargets;
		}

		int TargetHero( Card card, int cardHandPos, Player player, GameState gs )
		{
			if( !card.CanTarget( player.Hero ) ) {
				return 0;
			}

			if( card is Minion )
			{
				int availablePosCount = gs.Board.GetFreeBoardPositionCount( gs.Me );

				for( int boardPos = 0; boardPos < availablePosCount; ++boardPos )
				{
					Action action = new PlayMinionTargetingHeroAction( cardHandPos, boardPos, player.Tag );
					PerformAction( action, gs );
				}
			}
			else {
				Action action = new PlayCardTargetingHeroAction( cardHandPos, player.Tag );
				PerformAction( action, gs );
			}

			return 1;
		}

		void PlayCardWithoutTarget( Card card, int cardHandPos, GameState gs )
		{
			if( card is Minion )
			{
				int availablePosCount = gs.Board.GetFreeBoardPositionCount( gs.Me );

				for( int boardPos = 0; boardPos < availablePosCount; ++boardPos )
				{
					Action action = new PlayMinionAction( cardHandPos, boardPos );
					PerformAction( action, gs );
				}
			}
			else {
				Action action = new PlayCardAction( cardHandPos );
				PerformAction( action, gs );
			}
		}

		void PerformAction( Action action, GameState gs )
		{
			GameState newState = gs.Clone();

			newState.TurnActionList.Add( action );
			action.PerformAction( newState );

			PlayTurnRecursive( newState );
		}

		bool CanAffordToPlay( Card card, GameState gs )
		{
			return ( card.Cost <= gs.Me.Mana );
		}
	}
}

