using System;

namespace CSHearth
{
	public class Session
	{
		static int _uniqueId;
		static Random _rng;

		static Session()
		{
			_uniqueId = 0;
			_rng = new Random();
		}

		public static int GetUniqueId()
		{
			return _uniqueId++;
		}
	}
}

