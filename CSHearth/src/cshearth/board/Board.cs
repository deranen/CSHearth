using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CSHearth
{
	public class Board
	{
		readonly List<Minion>   _orderPlayedList;
		readonly List<Minion>[] _positionList;

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

			for( int i = 0; i < _orderPlayedList.Count; ++i ) {
				Minion minion = _orderPlayedList[i];

				Minion clonedMinion = (Minion) minion.Clone();

				int idx = (int) clonedMinion.Controller;
				int pos = (int) clonedMinion.BoardPosition;

				clone._orderPlayedList[i]     = clonedMinion;
				clone._positionList[idx][pos] = clonedMinion;
			}

			return clone;
		}

		public List<Minion> GetMinions( Player player )
		{
			int idx = (int) player.Tag;
			return _positionList[idx];
		}

		public Minion GetMinion( Player player, int minionPos )
		{
			int idx = (int) player.Tag;
			return _positionList[idx][minionPos];
		}

		public int GetMinionCount( Player player )
		{
			int idx = (int) player.Tag;
			return _positionList[idx].Count;
		}

		public void PutMinion( Minion minion, int boardPos, Player player )
		{
			minion.Controller = player.Tag;
			int idx = (int) player.Tag;

			_positionList[idx].Insert( boardPos, minion );
			_orderPlayedList.Add( minion );

			minion.BoardPosition = boardPos;
		}

		public void RemoveMinion( Minion minion )
		{
			int idx = (int) minion.Controller;
			int pos = (int) minion.BoardPosition;

			_positionList[idx].RemoveAt( pos );
			_orderPlayedList.RemoveAt( pos );
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

