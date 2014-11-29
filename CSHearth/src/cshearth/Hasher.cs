using System;

namespace CSHearth
{
	public static class Hasher
	{
		public static int InitialHash()
		{
			return 17;
		}

		public static int CombineHash( int currentHash, int additionalHash )
		{
			return currentHash*31 + additionalHash;
		}
	}
}

