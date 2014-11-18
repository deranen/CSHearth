using System;

namespace CSHearth
{
	public abstract class Minion : Card
	{
		public MinionType Type { get; protected set; }

		public int BaseAttack { get; private set; }
		public int BaseHealth { get; private set; }

		public int Attack    { get; set; }
		public int Health    { get; set; }
		public int MaxHealth { get; set; }

		public PlayerTag Controller    { get; set; }
		public int?      BoardPosition { get; set; }

		public int  AttackCount    { get; set; }
		public bool PlayedThisTurn { get; set; }

		protected Minion(
			string     name,
			int        cost,
			int        baseAttack,
			int        baseHealth)
			: base( name, cost )
		{
			Type       = MinionType.NoType;
			BaseAttack = baseAttack;
			BaseHealth = baseHealth;
			Attack     = baseAttack;
			Health     = baseHealth;
			MaxHealth  = baseHealth;

			Controller    = PlayerTag.None;
			BoardPosition = null;

			AttackCount    = 0;
			PlayedThisTurn = false;
		}

		public override Card Clone()
		{
			Minion clone = (Minion) base.Clone();

			return clone;
		}

		public bool IsControlledBy( Player player )
		{
			return ( Controller == player.Tag );
		}

		public bool CanAttack()
		{
			// TODO: Take into account windfury
			return ( !PlayedThisTurn && AttackCount == 0 );
		}

		public bool IsDead()
		{
			return ( Health <= 0 );
		}
	}
}

