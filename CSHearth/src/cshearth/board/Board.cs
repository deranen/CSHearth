using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CSHearth
{
	public class Board
	{
		private readonly List<Minion>   _orderPlayedList;
		private readonly List<Minion>[] _positionList;

		private int _orderPlayedCounter;

		public Board()
		{
			_orderPlayedList = new List<Minion>();
			_positionList    = new List<Minion>[2]();

			_orderPlayedList.Capacity = 2 * 7;
			_positionList[0].Capacity = 7;
			_positionList[1].Capacity = 7;

			_orderPlayedCounter = 0;
		}

		public Board Clone()
		{
			Board clone = (Board) MemberwiseClone();

			for( int i = 0; i < _orderPlayedList.Count; ++i ) {
				Minion minion = _orderPlayedList[i];

				Minion clonedMinion = minion.Clone();

				PlayerTag tag = clonedMinion.Controller;
				int       pos = clonedMinion.BoardPosition;

				clone._orderPlayedList[i]     = clonedMinion;
				clone._positionList[tag][pos] = clonedMinion;
			}
		}

		public List<Minion> GetMinions( Player player )
		{
			PlayerTag tag = player.Tag;
			return _positionList[tag];
		}

		public Minion GetMinion( Player player, int minionPos )
		{
			PlayerTag tag = player.Tag;
			return _positionList[tag][minionPos];
		}

		public int GetMinionCount( Player player )
		{
			PlayerTag tag = player.Tag;
			return _positionList[tag].Count;
		}

		public void PutMinion( Minion minion, int boardPos, Player player )
		{
			PlayerTag tag = player.Tag;
			minion.Controller = tag;

			_positionList[tag].Insert( boardPos, minion );
			_orderPlayedList[tag].Add( minion );
		}

		public void RemoveMinion( Minion minion )
		{
			PlayerTag tag = minion.Controller;

			_positionList[tag].Remove( minion );
			_orderPlayedList.Remove( minion );
		}

		public int GetFreeBoardPositionCount( Player player )
		{
			PlayerTag tag = player.Tag;

			int count = _positionList[tag].Count;

			if( count == 7 ) {
				return 0;
			}
			else {
				return count + 1;
			}
		}
	}
}

