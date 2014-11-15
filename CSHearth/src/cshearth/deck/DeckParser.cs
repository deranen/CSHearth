using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSHearth
{
	public class DeckParser
	{
		readonly StreamReader _fileReader;

		public DeckParser( string path )
		{
			_fileReader = File.OpenText( path );
		}

		public List<string> ParseDeck()
		{
			List<string> cardList = new List<string>();

			string line = "";

			while ((line = _fileReader.ReadLine()) != null)
			{
				string[] tuple = line.Replace( " ", "" ).Split( new char[] {':'} );

				Debug.Assert( tuple.Length == 2 );

				int    cardCount = Convert.ToInt32( tuple[0] );
				string cardName  = tuple[1];

				Debug.Assert( cardCount >= 0 );

				for( int i = 0; i < cardCount; ++i ) {
					cardList.Add( cardName );
				}
			}

			return cardList;
		}
	}
}

