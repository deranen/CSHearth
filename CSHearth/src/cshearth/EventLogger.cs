using System;
using System.IO;

namespace CSHearth
{
	public class EventLogger
	{
		GameEventHandler _eventHandler;
		StreamWriter _file;

		bool _enabled;
		bool _logActions;

		public bool LogToFile    { get; set; }
		public bool LogToConsole { get; set; }

		public bool Interactive { get; set; }

		const int    _boardWidth     = 40;
		const char   _separatorChar  = '-';
		const string _boardSeparator = "##";

		public EventLogger( GameEventHandler eventHandler, string fileName )
		{
			_eventHandler = eventHandler;
			_file = new StreamWriter( fileName, false );

			LogToFile    = true;
			LogToConsole = true;

			Interactive = true;

			_enabled    = true;
			_logActions = true;

			RegisterToEvents();
		}

		~EventLogger()
		{
			_file.Flush();
			_file.Close();

			if( _logActions ) {
				Detach();
			}
		}

		public bool Enabled {
			get {
				return _enabled;
			}
			set {
				if( !_enabled && value && _logActions ) {
					RegisterToEvents();
				} else if( _enabled && !value && _logActions ) {
					Detach();
				}

				_enabled = value;
			}
		}

		public bool LogActions {
			get {
				return _logActions;
			}
			set {
				if( !_logActions && value && _enabled ) {
					RegisterToEvents();
				} else if( _logActions && !value && _enabled ) {
					Detach();
				}

				_logActions = value;
			}
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

		public void LogLine()
		{
			if( LogToFile ) {
				_file.WriteLine();
			}

			if( LogToConsole ) {
				Console.WriteLine();
			}
		}

		#endregion

		void RegisterToEvents()
		{
			_eventHandler.TurnEnded  += TurnEndedLogger;
			_eventHandler.CardPlayed += CardPlayedLogger;
			_eventHandler.Attack     += AttackLogger;
		}

		void Detach()
		{
			_eventHandler.TurnEnded  -= TurnEndedLogger;
			_eventHandler.CardPlayed -= CardPlayedLogger;
			_eventHandler.Attack     -= AttackLogger;
		}

		void TurnEndedLogger(object sender, EventArgs e)
		{
			if( !_logActions )
				return;

			LogLine( "Turn ended." );
		}

		void CardPlayedLogger( object sender, CardPlayedEventArgs e )
		{
			if( !_logActions )
				return;

			GameState gs = (GameState) sender;

			string cardName = gs.Me.Hand.GetCard( (int) e.HandPos ).Name;

			if( e.PlayedMinion() ) {
				cardName += "[" + e.BoardPos + "]";
			}

			Log( "Plays " + cardName );

			if( e.TargetedMinion() ) {
				PlayerTag tag = e.MinionController;
				string minionName = gs.Board.GetMinion( tag, (int) e.MinionTarget ).Name;

				LogLine( " targeting " + minionName + "[" + e.MinionTarget + "]." );
			}
			if( e.TargetedHero() ) {
				if( e.HeroTarget == gs.Me.Tag ) {
					LogLine( "targeting himself." );
				} else {
					LogLine( "targeting enemy hero." );
				}
			} else {
				LogLine(".");
			}
		}

		void AttackLogger( object sender, AttackEventArgs e )
		{
			if( !_logActions )
				return;

			GameState gs = (GameState) sender;

			string attackerName;
			if( e.HeroAttacks() ) {
				attackerName = gs.Me.Hero.Name;
			} else {
				attackerName = gs.Board.GetMinion( gs.Me, (int) e.AttackerBoardPos ).Name;
				attackerName += "[" + e.AttackerBoardPos + "]";
			}

			string defenderName;
			if( e.HeroDefends() ) {
				defenderName = "the enemy hero";
			} else {
				defenderName = gs.Board.GetMinion( gs.Opponent, (int) e.DefenderBoardPos ).Name;
				defenderName += "[" + e.DefenderBoardPos + "]";
			}

			LogLine( attackerName + " attacks " + defenderName + "." );
		}

		#region GameStateLogging

		public void LogGameState( GameState gs )
		{
			if( !_enabled )
				return;

			if( Interactive ) {
				Console.Clear();
			}

			LogSeparator();
			LogHero( gs );
			LogSeparator();
			LogMana( gs );
			LogSeparator();
			LogBoard( gs );
			LogSeparator();
			LogHand( gs );
			LogSeparator();

			if( Interactive ) {
				Console.ReadKey();
			}
		}

		void LogHero( GameState gs )
		{
			string heroStringP1 = HeroStringByPlayer( PlayerTag.PlayerOne, gs );
			string heroStringP2 = HeroStringByPlayer( PlayerTag.PlayerTwo, gs );

			string heroStringFinal = ConstructBoardLine( heroStringP1, heroStringP2 );

			LogLine( heroStringFinal );
		}

		static string HeroStringByPlayer( PlayerTag playerTag, GameState gs )
		{
			Hero playerHero = gs.GetPlayer( playerTag ).Hero;

			string heroClass = playerHero.Tag.ToString();
			string heroHealth = playerHero.Health.ToString();
			string heroString = string.Format( "{0} ({1})", heroClass, heroHealth );

			if( gs.Me.Tag == playerTag ) {
				if( playerTag == PlayerTag.PlayerOne ) {
					heroString = heroString + " <==";
				} else {
					heroString = "==> " + heroString;
				}
			}

			return heroString;
		}

		void LogMana( GameState gs )
		{
			string manaStringP1 = ManaStringByPlayer( PlayerTag.PlayerOne, gs );
			string manaStringP2 = ManaStringByPlayer( PlayerTag.PlayerTwo, gs );

			string manaStringFinal = ConstructBoardLine( manaStringP1, manaStringP2 );

			LogLine( manaStringFinal );
		}

		string ManaStringByPlayer( PlayerTag playerTag, GameState gs )
		{
			Player player = gs.GetPlayer( playerTag );

			int mana = player.Mana;
			int maxMana = player.MaxMana;

			string manaString = string.Format( "({0}/{1}) ", mana, maxMana );

			for( int i = 0; i < maxMana; ++i ) {
				manaString += i < mana ? "[X]" : "[ ]";
			}

			return manaString;
		}

		void LogBoard( GameState gs )
		{
			string boardString = "";

			for( int i = 6; i >= 0; --i ) {
				boardString += LogBoardLine( i, gs ) + "\n";
			}

			LogLine( boardString );
		}

		string LogBoardLine( int i, GameState gs )
		{
			string boardLineStringP1 = BoardLineStringByPlayer( PlayerTag.PlayerOne, i, gs );
			string boardLineStringP2 = BoardLineStringByPlayer( PlayerTag.PlayerTwo, i, gs );

			string boardPosString = string.Format( "[{0}]", i );

			boardLineStringP1 = boardPosString + " " + boardLineStringP1;
			boardLineStringP2 = boardLineStringP2 + " " + boardPosString;

			string boardLineStringFinal = ConstructBoardLine( boardLineStringP1, boardLineStringP2 );

			return boardLineStringFinal;
		}

		string BoardLineStringByPlayer( PlayerTag playerTag, int i, GameState gs )
		{
			int minionCount = gs.Board.GetMinionCount( playerTag );

			if( i >= minionCount ) {
				return "";
			}

			Minion minion = gs.Board.GetMinion( playerTag, i );

			string minionString = string.Format( "({0}/{1}) {2}", minion.Attack, minion.Health, minion.Name );

			return minionString;
		}

		void LogHand( GameState gs )
		{
			string handString = "";

			int cardCountP1 = gs.Me.Hand.CardCount;
			int cardCountP2 = gs.Opponent.Hand.CardCount;

			int maxCardCount = Math.Max( cardCountP1, cardCountP2 );

			for( int i = 0; i < maxCardCount; ++i ) {
				handString += LogCardLine( i, gs ) + "\n";
			}

			LogLine( handString );
		}

		string LogCardLine( int i, GameState gs )
		{
			string cardLineStringP1 = CardLineStringByPlayer( PlayerTag.PlayerOne, i, gs );
			string cardLineStringP2 = CardLineStringByPlayer( PlayerTag.PlayerTwo, i, gs );

			string cardPosString = string.Format( "[{0}]", i );

			int cardCountP1 = gs.GetPlayer( PlayerTag.PlayerOne ).Hand.CardCount;
			int cardCountP2 = gs.GetPlayer( PlayerTag.PlayerTwo ).Hand.CardCount;

			if( i < cardCountP1 ) {
				cardLineStringP1 = cardPosString + " " + cardLineStringP1;
			}
			if( i < cardCountP2 ) {
				cardLineStringP2 = cardLineStringP2 + " " + cardPosString;
			}

			string boardLineStringFinal = ConstructBoardLine( cardLineStringP1, cardLineStringP2 );

			return boardLineStringFinal;
		}

		string CardLineStringByPlayer( PlayerTag playerTag, int i, GameState gs )
		{
			Hand hand = gs.GetPlayer( playerTag ).Hand;

			int cardCount = hand.CardCount;

			if( i >= cardCount ) {
				return "";
			}

			Card card = hand.GetCard( i );

			string cardString = string.Format( "({0}) {1}", card.Cost, card.Name );

			return cardString;
		}

		void LogSeparator()
		{
			string separatorString = ConstructBoardLine( "", "", _separatorChar );
			LogLine( separatorString );
		}

		static string ConstructBoardLine( string s1, string s2 )
		{
			return ConstructBoardLine( s1, s2, ' ' );
		}

		static string ConstructBoardLine( string s1, string s2, char padding )
		{
			return
				s1.PadRight( _boardWidth, padding ) +
			_boardSeparator +
			s2.PadLeft( _boardWidth, padding );
		}

		#endregion

	}
}

