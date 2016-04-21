using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class RoundManager {
	private int roundId = 0;
	private int maxRounds = 3;
	List<Round> rounds = new List<Round>();

	private int hasFinishedState = 0;

	public RoundManager() {

		Round round1 = new Round();
		Round round2 = new Round();
		Round round3 = new Round();

		rounds.Add(round1);
		rounds.Add(round2);
		rounds.Add(round3);


		rounds[0].changeState(Const.NOTSTARTED); // update state of all rounds to not started
		rounds[1].changeState(Const.NOTSTARTED); // update state of all rounds to not started
		rounds[2].changeState(Const.NOTSTARTED); // update state of all rounds to not started

	}

	public RoundScores getFinalScores() {
	
		RoundScores sc = new RoundScores();

		sc.pirateScore.Add(rounds[0].getPiratesFinalScore());
		sc.pirateScore.Add(rounds[1].getPiratesFinalScore());
		sc.pirateScore.Add(rounds[2].getPiratesFinalScore());

		sc.superCorpScore.Add(rounds[0].getSuperCorpFinalScore());
		sc.superCorpScore.Add(rounds[1].getSuperCorpFinalScore());
		sc.superCorpScore.Add(rounds[2].getSuperCorpFinalScore());

		return sc;
	}

	public void changeRound() {

		if (roundId == 0) {
			// game starts now

			Debug.Log("[RoundManager] : Starting game...");
			roundId = 1 ;

			if (rounds[roundId-1].getState() != Const.NOTSTARTED) {
				Debug.LogError("ERROR[RoundManager-ChangeRound]: Cannot start round " + roundId);
			} else {

				rounds[roundId-1].changeState(Const.RUNNING); // update state of new round to running

			}

		}  else if (roundId != maxRounds) {
			// as long as the game is not finishing
			Debug.Log("[RoundManager] : Changing Round...");

			if (rounds[roundId-1].getState() != Const.RUNNING) {

				Debug.LogError("ERROR[RoundManager-ChangeRound]: The round " + roundId + " is not running so cannot be finished");

			} else {

				rounds[roundId-1].changeState(Const.FINISHED); // update state of current round to finished
				roundId ++;

				if (rounds[roundId-1].getState() != Const.NOTSTARTED) {
					Debug.LogError("ERROR[RoundManager-ChangeRound]: Cannot start round " + roundId);
				} else {
					rounds[roundId-1].changeState(Const.RUNNING); // update state of new round to running
				}
			}
		} else {
			// when the game finishes
			Debug.Log("[RoundManager] : Finishing game...");

			if (rounds[roundId-1].getState() != Const.RUNNING) {

				Debug.LogError("ERROR[RoundManager-ChangeRound]: The round " + roundId + " is not running so cannot be finished");

			} else {	
				rounds[roundId-1].changeState(Const.FINISHED); // update state of current round to finished
				hasFinishedState = 1;
				}
			}

	}

	public int getRoundId() {

		return roundId;

	}

	public int getFinishedState() {
		return hasFinishedState;
	}

	public void finishRound(int scoreP, int scoreS) {
		if (rounds[roundId-1].getState() != Const.RUNNING) {
			Debug.LogError("ERROR[RoundManager-ChangeRound]: The round " + roundId + " is not running so cannot be finished");
		}
		else {
			rounds[roundId-1].finishRound(scoreP,scoreS);
		}
	}
}

[System.Serializable]
public class Round {

	private int state ; // 0 = not started, 1 = running, -1 = finished
	private int finalScoreTeamPirates = 0;
	private int finalScoreTeamSuperCorp = 0;

	public void changeState (int newState) {

		state = newState;

	}

	public void finishRound(int scoreP, int scoreS) {
		Debug.Log("Finishing Scores");
		finalScoreTeamPirates = scoreP ;
		finalScoreTeamSuperCorp = scoreS ;
		
	}

	public int getPiratesFinalScore() {

		return finalScoreTeamPirates;

	} 

	public int getSuperCorpFinalScore() {

		return finalScoreTeamSuperCorp;

	} 

	public int getState() {
		return state;
	}



}
