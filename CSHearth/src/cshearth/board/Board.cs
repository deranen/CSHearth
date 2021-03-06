﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;

namespace CSHearth
{
	public class Board
	{
		List<Minion>   _orderPlayedList;
		List<Minion>[] _positionList;

		public Board()
		{
			_orderPlayedList = new List<Minion>();
			_positionList    = new List<Minion>[2];

			_positionList[0] = new List<Minion>();
			_positionList[1] = new List<Minion>();

			_orderPlayedList.Capacity = 2 * 7;
			_positionList[0].Capacity = 7;
			_positionList[1].Capacity = 7;
		}

		public Board Clone()
		{
			Board clone = (Board) MemberwiseClone();

			clone._orderPlayedList = new List<Minion>( _orderPlayedList );

			clone._positionList    = new List<Minion>[2];
			clone._positionList[0] = new List<Minion>( _positionList[0] );
			clone._positionList[1] = new List<Minion>( _positionList[1] );

			for( int i = 0; i < _orderPlayedList.Count; ++i ) {
				Minion minion = _orderPlayedList[i];

				Minion clonedMinion = (Minion) minion.Clone();

				int idx = (int) clonedMinion.Controller;
				int pos = _positionList[idx].IndexOf( minion );

				clone._orderPlayedList[i]     = clonedMinion;
				clone._positionList[idx][pos] = clonedMinion;
			}

			return clone;
		}

		public override int GetHashCode()
		{
			int hash = Hasher.InitialHash();

			foreach( Minion minion in _orderPlayedList ) {
				hash = Hasher.CombineHash( hash, minion.GetHashCode() );
			}

			foreach( Minion minion in _positionList[0] ) {
				hash = Hasher.CombineHash( hash, minion.GetHashCode() );
			}

			foreach( Minion minion in _positionList[1] ) {
				hash = Hasher.CombineHash( hash, minion.GetHashCode() );
			}

			return hash;
		}

		public List<Minion> GetMinions()
		{
			return _orderPlayedList;
		}

		public List<Minion> GetMinions( Player player )
		{
			int idx = (int) player.Tag;
			return _positionList[idx];
		}

		public Minion GetMinion( int minionId )
		{
			Minion minion = _orderPlayedList.Find( m => m.Id == minionId );

			Debug.Assert( minion != null );

			return minion;
		}

		public Minion GetMinion( Player player, int minionPos )
		{
			return GetMinion( player.Tag, minionPos );
		}

		public Minion GetMinion( PlayerTag playerTag, int minionPos )
		{
			Debug.Assert( playerTag != PlayerTag.None );

			int idx = (int) playerTag;
			return _positionList[idx][minionPos];
		}

		public int GetMinionCount( Player player )
		{
			return GetMinionCount( player.Tag );
		}

		public int GetMinionCount( PlayerTag playerTag )
		{
			Debug.Assert( playerTag != PlayerTag.None );

			int idx = (int) playerTag;
			return _positionList[idx].Count;
		}

		public void PutMinion( Minion minion, int boardPos, Player player, GameEventHandler eh )
		{
			minion.Controller = player.Tag;
			int idx = (int) player.Tag;

			_positionList[idx].Insert( boardPos, minion );
			_orderPlayedList.Add( minion );

			minion.RegisterToEvents( eh );
		}

		public void RemoveMinion( Minion minion, GameEventHandler eh )
		{
			int idx = (int) minion.Controller;

			_positionList[idx].Remove( minion );
			_orderPlayedList.Remove( minion );

			minion.DeregisterFromEvents( eh );
		}

		public int GetFreeBoardPositionCount( Player player )
		{
			int idx = (int) player.Tag;
			int count = _positionList[idx].Count;

			if( count == 7 ) {
				return 0;
			}

			return count + 1;
		}
	}
}

