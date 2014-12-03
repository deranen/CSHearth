
namespace CSHearth
{
	public abstract class Minion : Card
	{
		public MinionType Type { get; protected set; }

		public int BaseAttack { get; private set; }
		public int BaseHealth { get; private set; }

		int _attack;
		int _health;
		int _maxHealth;

		PlayerTag _controller;

		int _attackCount;
		bool _playedThisTurn;

		public int Attack {
			get { return _attack; }
			set {
				HashUpToDate = false;
				_attack = value;
			}
		}

		public int Health {
			get { return _health; }
			set {
				HashUpToDate = false;
				_health = value;
			}
		}

		public int MaxHealth {
			get { return _maxHealth; }
			set {
				HashUpToDate = false;
				_maxHealth = value;
			}
		}

		public PlayerTag Controller {
			get { return _controller; }
			set {
				HashUpToDate = false;
				_controller = value;
			}
		}

		public int AttackCount {
			get { return _attackCount; }
			set {
				HashUpToDate = false;
				_attackCount = value;
			}
		}

		public bool PlayedThisTurn {
			get { return _playedThisTurn; }
			set {
				HashUpToDate = false;
				_playedThisTurn = value;
			}
		}

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

			Controller = PlayerTag.None;

			AttackCount    = 0;
			PlayedThisTurn = false;
		}

		public override Card Clone()
		{
			Minion clone = (Minion) base.Clone();

			return clone;
		}

		public override int GetHashCode()
		{
			if( HashUpToDate ) {
				return Hash;
			}

			int hash = Hasher.InitialHash();

			hash = Hasher.CombineHash( hash, base.GetHashCode() );

			hash = Hasher.CombineHash( hash, Type.GetHashCode() );
			hash = Hasher.CombineHash( hash, BaseAttack.GetHashCode() );
			hash = Hasher.CombineHash( hash, BaseHealth.GetHashCode() );
			hash = Hasher.CombineHash( hash, Attack.GetHashCode() );
			hash = Hasher.CombineHash( hash, Health.GetHashCode() );
			hash = Hasher.CombineHash( hash, MaxHealth.GetHashCode() );
			hash = Hasher.CombineHash( hash, Controller.GetHashCode() );
			hash = Hasher.CombineHash( hash, AttackCount.GetHashCode() );
			hash = Hasher.CombineHash( hash, PlayedThisTurn.GetHashCode() );

			Hash = hash;
			HashUpToDate = true;

			return hash;
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

