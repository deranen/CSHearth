﻿using System;
using System.Diagnostics;
using System.Collections;

namespace CSHearth
{
	public class Hero
	{
		public int Id { get; private set; }

		public HeroTag Tag  { get; private set; }
		public string  Name { get; private set; }

		public int MaxHealth { get; private set; }

		public int Health { get; set; }
		public int Armor  { get; set; }
		public int Attack { get; set; }

		public int AttackCount { get; set; }

		protected Hero (HeroTag tag, string name, int maxHealth)
		{
			Debug.Assert (maxHealth > 0);

			Id            = Session.GetUniqueId();
			Tag           = tag;
			Name          = name;
			MaxHealth     = maxHealth;
			Health        = maxHealth;
			Armor         = 0;
			Attack        = 0;
			AttackCount   = 0;
		}

		public Hero Clone ()
		{
			Hero clone = (Hero) MemberwiseClone();

			return clone;
		}

		public override int GetHashCode()
		{
			int hash = Hasher.InitialHash();

			hash = Hasher.CombineHash( hash, Health.GetHashCode() );
			hash = Hasher.CombineHash( hash, Armor.GetHashCode() );
			hash = Hasher.CombineHash( hash, Attack.GetHashCode() );
			hash = Hasher.CombineHash( hash, AttackCount.GetHashCode() );

			return hash;
		}

		public bool IsDead()
		{
			return (Health <= 0);
		}
	}
}

