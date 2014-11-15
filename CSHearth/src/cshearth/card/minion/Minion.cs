using System;

namespace CSHearth
{
	public abstract class Minion : Card
	{
		public MinionType MinionType { get; private set; }

		public int BaseAttack { get; private set; }
		public int BaseHealth { get; private set; }

		public int Attack    { get; set; }
		public int Health    { get; set; }
		public int MaxHealth { get; set; }

		public PlayerTag Controller    { get; set; }
		public int       BoardPosition { get; set; }

		public int       AttackCount    { get; set; }
		public bool      PlayedThisTurn { get; set; }

		protected Minion(
			CardClass  cardClass,
			CardTag    cardTag,
			MinionType minionType,
			int        cost,
			int        baseAttack,
			int        baseHealth)
			: base( CardType.Minion, cardClass, cardTag, cost )
		{
			MinionType = minionType;
			BaseAttack = baseAttack;
			BaseHealth = baseHealth;
			Attack     = baseAttack;
			Health     = baseHealth;
			MaxHealth  = baseHealth;

			Controller    = PlayerTag.None;
			BoardPosition = -1;

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

