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

			return myHealth + myMinionCount - oppoHealth;
		}


	}
}

