using System.Collections.Generic;

namespace CSHearth
{
	public class GameLogic
	{
		public double    BestScore     { get; private set; }
		public GameState BestGameState { get; private set; }

		public List<Action> TurnActionList { get; private set; }

		public int VariationsSimulated { get; private set; }

		public GameLogic()
		{
			BestScore     = double.NegativeInfinity;
			BestGameState = null;

			VariationsSimulated = 0;
		}

		public void StartOfTurn( GameState gs )
		{
			BestScore     = double.NegativeInfinity;
			BestGameState = null;

			VariationsSimulated = 0;
		}

		public void PlayTurn( GameState gs )
		{
			StartOfTurn( gs );
			PlayTurnRecursive( gs );
		}

		void PlayTurnRecursive( GameState gs )
		{
			if( gs.TurnEnded || gs.Me.IsDead() || gs.Opponent.IsDead() )
			{
				// TODO: Implement simple scoring function
				double score = 0.0; //AI.CalculateScore( gs );

				if( score > BestScore ) {
					BestScore     = score;
					BestGameState = gs;
				}

				++VariationsSimulated;

				return;
			}
			else {
				Action action = new EndTurnAction();

				PerformAction( action, gs );
			}

			int myMinionCount = gs.Board.GetMinionCount( gs.Me );

			for( int myMinionPos = 0; myMinionPos < myMinionCount; ++myMinionPos )
			{
				Minion myMinion = gs.Board.GetMinion( gs.Me, myMinionPos );

				if( !myMinion.CanAttack() ) {
					continue;
				}

				int oppoMinionCount = gs.Board.GetMinionCount( gs.Opponent );

				for( int oppoMinionPos = 0; oppoMinionPos < oppoMinionCount; ++oppoMinionPos )
				{
					Minion oppoMinion = gs.Board.GetMinion( gs.Opponent, oppoMinionPos );

					// TODO: Handle taunt

					Action action = new AttackMinionWithMinionAction( myMinionPos, oppoMinionPos );

					PerformAction( action, gs );
				}

				// TODO: Handle taunt

				{
					Action action = new AttackHeroWithMinionAction( myMinionPos );

					PerformAction( action, gs );
				}
			}

			int cardCount = gs.Me.Hand.CardCount;

			for( int handPos = 0; handPos < cardCount; ++handPos )
			{
				Card card = gs.Me.Hand.GetCard( handPos );

				if( !CanAffordToPlay( card, gs ) ) {
					continue;
				}

				if( card.NeedsTarget )
				{
					int availableTargets = 0;

					availableTargets += TargetMinionsOnBoard( card, handPos, gs.Me, gs );
					availableTargets += TargetMinionsOnBoard( card, handPos, gs.Opponent, gs );

					availableTargets += TargetHero( card, handPos, gs.Me, gs );
					availableTargets += TargetHero( card, handPos, gs.Opponent, gs );

					if( availableTargets == 0 && !card.MustHaveTarget )
					{
						PlayCardWithoutTarget( card, handPos, gs );
					}
				}
				else {
					PlayCardWithoutTarget( card, handPos, gs );
				}
			}
		}

		int TargetMinionsOnBoard( Card card, int cardHandPos, Player player, GameState gs )
		{
			int availableTargets = 0;

			int minionCount = gs.Board.GetMinionCount( player );

			for( int targetBoardPos = 0; targetBoardPos < minionCount; ++targetBoardPos )
			{
				Minion targetMinion = gs.Board.GetMinion( player, targetBoardPos );

				if( !card.CanTarget( targetMinion ) ) {
					continue;
				}

				++availableTargets;

				if( card is Minion )
				{
					int availablePosCount = gs.Board.GetFreeBoardPositionCount( gs.Me );

					for( int boardPos = 0; boardPos < availablePosCount; ++boardPos )
					{
						Action action = new PlayMinionTargetingMinionAction( cardHandPos, boardPos, targetBoardPos, player.Tag );

						PerformAction( action, gs );
					}
				}
				else {
					Action action = new PlayCardTargetingMinionAction( cardHandPos, targetBoardPos, player.Tag );

					PerformAction( action, gs );
				}
			}

			return availableTargets;
		}

		int TargetHero( Card card, int cardHandPos, Player player, GameState gs )
		{
			if( !card.CanTarget( player.Hero ) ) {
				return 0;
			}

			if( card is Minion )
			{
				int availablePosCount = gs.Board.GetFreeBoardPositionCount( gs.Me );

				for( int boardPos = 0; boardPos < availablePosCount; ++boardPos )
				{
					Action action = new PlayMinionTargetingHeroAction( cardHandPos, boardPos, player.Tag );
					PerformAction( action, gs );
				}
			}
			else {
				Action action = new PlayCardTargetingHeroAction( cardHandPos, player.Tag );
				PerformAction( action, gs );
			}

			return 1;
		}

		void PlayCardWithoutTarget( Card card, int cardHandPos, GameState gs )
		{
			if( card is Minion )
			{
				int availablePosCount = gs.Board.GetFreeBoardPositionCount( gs.Me );

				for( int boardPos = 0; boardPos < availablePosCount; ++boardPos )
				{
					Action action = new PlayMinionAction( cardHandPos, boardPos );
					PerformAction( action, gs );
				}
			}
			else {
				Action action = new PlayCardAction( cardHandPos );
				PerformAction( action, gs );
			}
		}

		void PerformAction( Action action, GameState gs )
		{
			GameState newState = gs.Clone();

			TurnActionList.Add( action );
			action.PerformAction( newState );

			PlayTurnRecursive( newState );
		}

		bool CanAffordToPlay( Card card, GameState gs )
		{
			return ( card.Cost <= gs.Me.Mana );
		}
	}
}

