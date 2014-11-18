using System;

namespace CSHearth
{
	public class BasicAI : IArtificalIntelligence
	{
		public double CalculateScore( GameState gs )
		{
			int oppoMaxHealth = gs.Opponent.Hero.MaxHealth;
			int oppoHealth    = gs.Opponent.Hero.Health;
			int myMinionCount = gs.Board.GetMinionCount( gs.Me );

			return (oppoMaxHealth - oppoHealth) + myMinionCount;
		}


	}
}

