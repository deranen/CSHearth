using System;

namespace CSHearth
{
	public abstract class Minion : Card
	{
		public MinionType MinionType { get; private set; }

		public int BaseAttack { get; private set; }
		public int BaseHealth { get; private set; }

		public int Attack    { get; private set; }
		public int Health    { get; private set; }
		public int MaxHealth { get; private set; }

		public PlayerTag Controller     { get; private set; }
		public int       AttackCount    { get; private set; }
		public bool      PlayedThisTurn { get; private set; }

		protected Minion(
			CardTag    cardTag,
			CardClass  cardClass,
			CardType   cardType,
			int        cost,
			MinionType minionType,
			int        baseAttack,
			int        baseHealth)
			: base( cardTag, cardClass, cardType, cost )
		{
			MinionType = minionType;
			BaseAttack = baseAttack;
			BaseHealth = baseHealth;
			Attack     = baseAttack;
			Health     = baseHealth;
			MaxHealth  = baseHealth;

			Controller     = PlayerTag.None;
			AttackCount    = 0;
			PlayedThisTurn = false;

			base.NeedsBoardPosition = true;
		}

		public Minion Clone()
		{
			Minion clone = (Minion) MemberwiseClone();

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

