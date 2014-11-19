using System;
using System.IO;

namespace CSHearth
{
	public class EventLogger
	{
		StreamWriter _file;

		public bool LogToFile    { get; set; }
		public bool LogToConsole { get; set; }

		public EventLogger(string fileName)
		{
			_file = new StreamWriter( fileName, false );

			LogToFile    = true;
			LogToConsole = true;

			RegisterToEvents();
		}

		~EventLogger()
		{
			_file.Close();
			Detach();
		}

		#region Logging methods

		public void Log( string s )
		{
			if( LogToFile ) {
				_file.Write( s );
			}

			if( LogToConsole ) {
				Console.Write( s );
			}
		}

		public void LogLine( string s )
		{
			if( LogToFile ) {
				_file.WriteLine( s );
			}

			if( LogToConsole ) {
				Console.WriteLine( s );
			}
		}

		#endregion

		void RegisterToEvents()
		{
			Events.TurnEnded  += TurnEndedLogger;
			Events.CardPlayed += CardPlayedLogger;
			Events.Attack     += AttackLogger;
		}

		void Detach()
		{
			Events.TurnEnded  -= TurnEndedLogger;
			Events.CardPlayed -= CardPlayedLogger;
			Events.Attack     -= AttackLogger;
		}

		void TurnEndedLogger(object sender, EventArgs e)
		{
			LogLine( "Turn ended." );
		}

		void CardPlayedLogger( object sender, CardPlayedEventArgs e )
		{
			throw new NotImplementedException();
		}

		void AttackLogger( object sender, AttackEventArgs e )
		{
			GameState gs = (GameState) sender;

			string attackerName;
			if( e.HeroAttacks() ) {
				attackerName = gs.GetPlayer( e.AttackerPlayerTag ).Hero.Name;
			} else {
				attackerName = gs.Board.GetMinion( gs.Me, (int) e.AttackerBoardPos ).Name;
			}

			string defenderName;
			if( e.HeroDefends() ) {
				defenderName = gs.GetPlayer( e.DefenderPlayerTag ).Hero.Name;
			} else {
				defenderName = gs.Board.GetMinion( gs.Opponent, (int) e.DefenderBoardPos ).Name;
			}

			LogLine( attackerName + " attacks " + defenderName + "." );
		}
	}
}

