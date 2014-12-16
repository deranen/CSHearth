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

			double myBoardControlFactor = 1.0;
			double oppoBoardControlFactor = 1.0;
			if( myMinionCount > 0 && oppoMinionCount == 0 ) {
				myBoardControlFactor = 1.5;
			} else if( oppoMinionCount > 0 && myMinionCount == 0 ) {
				oppoBoardControlFactor = 1.5;
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
			double myMinionScore   = myBoardControlFactor * myMinionValue;
			double oppoMinionScore = oppoBoardControlFactor * oppoMinionValue;

			double minionScore = myMinionScore - oppoMinionScore;

			return healthScore + minionScore;
		}


	}
}

