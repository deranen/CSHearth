using System;

namespace CSHearth
{
	public class GameLogic
	{
		public double    BestScore     { get; private set; }
		public GameState BestGameState { get; private set; }

		public int VariationsSimulated { get; private set; }

		public GameLogic()
		{
			BestScore     = double.NegativeInfinity;
			BestGameState = null;

			VariationsSimulated = 0;
		}

		public void PlayTurn( GameState gs )
		{
			BestScore     = double.NegativeInfinity;
			BestGameState = null;

			VariationsSimulated = 0;

			PlayTurnRecursive( gs );
		}

		private void PlayTurnRecursive( GameState gs )
		{
			if( gs.TurnEnded || gs.Me.IsDead || gs.Opponent.IsDead )
			{
				double score = AI.CalculateScore( gs );

				if( score > BestScore ) {
					BestScore     = score;
					BestGameState = gs;
				}

				++VariationsSimulated;

				return;
			}
			else {
				AbstractAction action = new EndTurnAction();

				performAction( action );
			}

			int myMinionCount = Board.GetMinionCount( gs.Me );

//			for( int myMinionPos = 0; myMinionPos < myMinionCount; ++myMinionPos )
//			{
//
//			}

			foreach( size_t attackingMinionPos; 0..myMinionCount )
			{
				Minion attackingMinion = _board.getMinion( _me, attackingMinionPos );

				if( !attackingMinion.canAttack() ) {
					continue;
				}

				immutable bool   oppoHasTauntedMinions = _board.hasTauntedMinions( _opponent );
				immutable size_t oppoMinionCount       = _board.getMinionCount   ( _opponent );

				foreach( size_t defendingMinionPos; 0..oppoMinionCount )
				{
					Minion defendingMinion = _board.getMinion( _opponent, defendingMinionPos );

					if( oppoHasTauntedMinions && !defendingMinion.hasTaunt() ) {
						continue;
					}

					AbstractAction action =
						new AttackMinionWithMinionAction( attackingMinionPos, defendingMinionPos );

					performAction( action );
				}

				if( !oppoHasTauntedMinions )
				{

					AbstractAction action =
						new AttackHeroWithMinionAction( attackingMinionPos );

					performAction( action );
				}
			}

			immutable size_t cardCount = _me.hand.cardCount;

			foreach( size_t handPos; 0..cardCount )
			{
				Card card = _me.hand.getCard( handPos );

				if( !canAffordToPlay( card ) ) {
					continue;
				}

				if( card.needsTarget )
				{
					size_t availableTargets = 0;

					availableTargets += targetMinionsOnBoard( card, handPos, _me );
					availableTargets += targetMinionsOnBoard( card, handPos, _opponent );

					availableTargets += targetHero( card, handPos, _me );
					availableTargets += targetHero( card, handPos, _opponent );

					if( availableTargets == 0 && !card.mustHaveTarget )
					{
						playCardWithoutTarget( card, handPos );
					}
				}
				else {
					playCardWithoutTarget( card, handPos );
				}
			}
		}
	}
}

