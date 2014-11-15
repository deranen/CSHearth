using System;

namespace CSHearth
{
	public class AttackMinionWithMinionAction : Action
	{
		readonly int _attackerPos;
		readonly int _defenderPos;

		public AttackMinionWithMinionAction( int attackerPos, int defenderPos )
		{
			_attackerPos = attackerPos;
			_defenderPos = defenderPos;
		}

		public override void PerformAction( GameState gs )
		{
			Minion attacker = gs.Board.GetMinion( gs.Me,       _attackerPos );
			Minion defender = gs.Board.GetMinion( gs.Opponent, _defenderPos );

			defender.Health -= attacker.Attack;
			attacker.Health -= defender.Attack;

			attacker.AttackCount++;

			if( defender.IsDead() ) {
				gs.Board.RemoveMinion( defender );

				// TODO: Trigger deathrattle
			}

			if( attacker.IsDead() ) {
				gs.Board.RemoveMinion( attacker );

				// TODO: Trigger deathrattle
			}
		}
	}
}

