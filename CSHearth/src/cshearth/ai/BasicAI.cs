using System;

namespace CSHearth
{
	public class BasicAI : IArtificalIntelligence
	{
		public double CalculateScore( GameState gs )
		{
			int oppoHealth = gs.Opponent.Hero.Health;

			if( oppoHealth <= 0 ) {
				return double.MaxValue;
			}

			int myHealth = gs.Me.Hero.Health;

			if( myHealth <= 0 ) {
				return double.MinValue;
			}

			int myMinionCount = gs.Board.GetMinionCount( gs.Me );
			int oppoMinionCount = gs.Board.GetMinionCount( gs.Opponent );

			double boardControlFactor = 1.0;
			if( myMinionCount > 0 && oppoMinionCount == 0 ) {
				boardControlFactor = 1.5;
			}

			double oppoMinionValue = 0.0;
			foreach( Minion minion in gs.Board.GetMinions( gs.Opponent ) ) {
				oppoMinionValue += minion.Cost + 1.0; // TODO: Use BaseCost here
			}

			double myMinionValue = 0.0;
			foreach( Minion minion in gs.Board.GetMinions( gs.Me ) ) {
				myMinionValue += minion.Cost + 1.0; // TODO: Use BaseCost here
			}

			double healthScore = 0.5 * (myHealth - oppoHealth);
			double minionScore = myMinionValue - oppoMinionValue;
			minionScore *= boardControlFactor;

			return healthScore + minionScore;
		}


	}
}

