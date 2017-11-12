using UnityEngine;
using System.Collections;

public enum Turn
{
  Player,
  AI,
  WaitingOnPlayer,
  Disengage
}

public class BattleSystem : MonoBehaviour 
{
  Player player;
  public Ship playerShip;
  public Ship AIShip;
  public Turn turn;
  public bool startBattle = false;
  public string battleLog = "";

  public AudioClip playerFireClip;
  public AudioClip aiFireClip;

  void Start ()
  {
    player = GameObject.Find( "PlayerShip" ).GetComponent<Player>();
    playerShip = player.GetComponent<Ship>();
  }

  public void PassAIToBattle ( Ship _AIShip )
  {
    AIShip = _AIShip;
    startBattle = true;
    turn = (Turn) Random.Range( 0, 2 );
  }

  bool Roll ( int value )
  {
    int x = Random.Range( 0, 101 );

    return x <= value ? true : false;
  }

  float time = 1.5f;
  public float currTime = 1.5f;
  void Battle ()
  {
    if (currTime >= 0.0f)
    {
      currTime -= 1.0f * Time.fixedDeltaTime;
    }
    else
    {
      switch (turn)
      {
        case Turn.Player:
          {
            int dmgDone = playerShip.DoDamage( AIShip );
            if (dmgDone == 0)
            {
              battleLog = "You fail to fire <color=\"#58DD0BFF\">" + dmgDone + " (no ordnance)</color>. Enemy next.";
            }
            else
            {
              AudioSource.PlayClipAtPoint( playerFireClip, Camera.main.transform.position, 100.0f );
              battleLog = "You fire for <color=\"#58DD0BFF\">" + dmgDone + "</color>. Enemy next.";
            }
            turn = Turn.WaitingOnPlayer;
          } break;

        case Turn.AI:
          {
            int dmgDone = AIShip.DoDamage( playerShip );
            AudioSource.PlayClipAtPoint( aiFireClip, Camera.main.transform.position, 100.0f );
            battleLog = "Enemy fires for <color=\"#FF5050FF\">" + dmgDone + "</color>. You next.";
            turn = Turn.Player;
          } break;

        case Turn.WaitingOnPlayer:
          {
            battleLog = "Waiting on player. Continue next turn.";
            return;
          }

        case Turn.Disengage:
          {
            if (Roll( 35 ))
            {
              battleLog = "Attempting disengage. Successful.";
              Destroy( AIShip.gameObject );
            }
            else
            {
              battleLog = "Attempting disengage. Failure.";
              turn = Turn.AI;
            }
          } break;

        default:
          {
            Debug.Log( "Something went wrong with the Battle System" );
          } break;
      }

      currTime = time;
    }

    if (AIShip == null)
    {
      startBattle = false;
      player.inBattle = false;
      return;
    }
  }

  void FixedUpdate ()
  {
    if (startBattle)
    {
      Battle();
    }
  }
}