using System;

namespace CSHearth
{
	public class AttackHeroWithMinionAction : Action
	{
		readonly int _attackerPos;

		public AttackHeroWithMinionAction( int attackerPos )
		{
			_attackerPos = attackerPos;
		}

		public override void PerformAction( GameState gs )
		{
			Minion attacker = gs.Board.GetMinion( gs.Me, _attackerPos );
			Hero opponentHero = gs.Opponent.Hero;

			var eventArgs = new AttackEventArgs {
				AttackerBoardPos  = _attackerPos,
				DefenderPlayerTag = gs.Opponent.Tag
			};
			gs.Events.OnAttack( gs, eventArgs );

			opponentHero.Health = opponentHero.Health - attacker.Attack;
			attacker.AttackCount++;
		}
	}
}

