using System;

namespace CSHearth
{
	public abstract class Action
	{
		public Action Clone()
		{
			Action clone = (Action) MemberwiseClone();

			return clone;
		}

		public abstract void PerformAction( GameState gs );
	}
}

