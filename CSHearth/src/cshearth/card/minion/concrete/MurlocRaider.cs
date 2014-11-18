using System;

namespace CSHearth
{
	public class MurlocRaider : Minion
	{
		public MurlocRaider()
			: base( "Murloc Raider", 1, 2, 1 )
		{
			Type = MinionType.Murloc;
		}
	}
}

