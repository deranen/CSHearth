using System;
using System.Diagnostics;

namespace CSHearth
{
	public static class Session
	{
		static int _uniqueId;
		static int _seed;

		public static Random RNG { get; private set; }

		public static Stopwatch Stopwatch { get; private set; }

		public static int Seed {
			get {
				return _seed;
			}
			set {
				_seed = value;
				RNG = new Random( _seed );
			}
		}

		static Session()
		{
			_uniqueId = 0;

			TimeSpan t = DateTime.Now - new DateTime(2000, 1, 1, 0, 0, 0);
			_seed      = (int) t.TotalSeconds;
			RNG        = new Random( _seed );

			Stopwatch = new Stopwatch();
		}

		public static int GetUniqueId()
		{
			return _uniqueId++;
		}
	}
}

