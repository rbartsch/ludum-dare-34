using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EventTypes
{
  Exploration,
  EnemyEngagement,
  ShipMishap
}

public interface IEvent
{
  string eventTitle { get; set; }
  string eventDescription { get; set; }
  EventTypes eventType { get; set; }

  void Popup ( GameUI gameUI );
  void ExecuteEvent ();
}

public class Event
{
  public class Exploration : IEvent
  {
    public string eventTitle { get; set; }
    public string eventDescription { get; set; }
    public EventTypes eventType { get; set; }

    public void Popup (GameUI gameUI)
    {
      gameUI.EventPopup( eventTitle, eventDescription, this );
    }

    public void ExecuteEvent ()
    {
      Debug.Log( "Executing Exploration event" );
    }

    public Exploration ( string _eventTitle, string _eventDescr, EventTypes _eventType )
    {
      eventTitle = _eventTitle;
      eventDescription = _eventDescr;
      eventType = _eventType;
    }
  }

  public class EnemyEngagement : IEvent
  {
    public string eventTitle { get; set; }
    public string eventDescription { get; set; }
    public EventTypes eventType { get; set; }
    GameObject AIShip;
    public Ship ship;
    public void Popup ( GameUI gameUI )
    {
      gameUI.EventPopup( eventTitle, eventDescription, this );
    }

    public void ExecuteEvent ()
    {
      BattleSystem battleSystem = GameObject.Find( "BattleSystem" ).GetComponent<BattleSystem>();
      GameObject player = GameObject.Find( "PlayerShip" );
      GameObject AIShipClone = (GameObject) Object.Instantiate( AIShip, new Vector3( player.transform.position.x + 25, player.transform.position.y, 0), Quaternion.identity );
      AIShipClone.transform.parent = player.transform;
      ship = AIShipClone.GetComponent<Ship>();
      ship.Initialize();
      battleSystem.PassAIToBattle( ship );
      Debug.Log( "Executing EnemyEngagement event" );
    }

    public EnemyEngagement ( string _eventTitle, string _eventDescr, EventTypes _eventType, GameObject _AIShip )
    {
      eventTitle = _eventTitle;
      eventDescription = _eventDescr;
      eventType = _eventType;
      AIShip = _AIShip;
    }
  }
}

public class Events : MonoBehaviour
{
  public List<IEvent> events = new List<IEvent>();
  public GameObject AIShip;
 
  void Start ()
  {
    events.Add( new Event.Exploration( "Security away team sent...", "Entering orbit around the planet for while engineering performs diagnostics while the science team prepares to send a probe to the planet.\n\nUpon launching the probe and entering the atmosphere it is shot down.\n\nYou order an away team to assemble and they shuttle down towards the planet. Upon arrival, their scouting and investigation has unveiled an abandoned small military base on the planet built by the Sleepers.\n\nIts automated defense system destroyed the probe, your away team successfully disables it and the later followed survey has unveiled the resources on the planet.", EventTypes.Exploration ) );
    events.Add( new Event.Exploration( "Survey from orbit...", "A standard survey operation begins to take place from orbit, they send multiple scans and probes over the hours that provide hours of data to work through.\n\nWhile the science team analyses the data with the help of the engineers performing the data processing, the medical team is attending to some crew members whom seem to have fallen ill from food poisoning.\n\nIt seems some of the edible food on the ship is not so edible after all. A few hours later, the survey operation has revealed resources on the planet.", EventTypes.Exploration ) );
    events.Add( new Event.Exploration( "Survey away team sent...", "Attempts to survey from orbit is pointless due to the strong magnetic field around the planet interfering with the scans and probe signals.\n\nA survey away team is assembled of scientists and engineers that shuttle down to the planet to perform their grueling survey on the surface and in the atmosphere.\n\nYou lose one crew member who successfully saved another crew member's life when a rock from the nearby cliff came tumbling down. The bravery and honour of this crew member will forever be remembered.\n\nIn the end the survery team revealed the resources on the planet.", EventTypes.Exploration ) );
    events.Add( new Event.Exploration( "Survey from orbit...", "The scans and probes sent to the planet immediately revealed a dangerous surface and hazardous atmosphere. Fortunately not much analysis and data processing was needed to reveal the resources on the planet.\n\nYou caught some crew members falling asleep on the job!", EventTypes.Exploration ) );
    events.Add( new Event.Exploration( "Survey away team sent...", "The scans and probes after an hour revealed that the atmosphere is hazardous.\n\nHowever, it is proving difficult to perform the survey from orbit. The survey away team of science and engineers equipped themselves in special suits to protect themselves.\n\nWhile on the surface an scientist's suit ruptured when they fainted from the grueling hours. The scientist while unconscious breathed in the hazardous atmosphere and died.\n\nThere wasn't enough time to get to the shuttle craft. In the end the survery revealed the planet's resources.", EventTypes.Exploration ) );
    events.Add( new Event.Exploration( "Security away team sent...", "While the survey away team was on the planet they suddenly came under attack by a group of Sleepers!\n\nThe survey away team attempted to hold position while the security away team made their way quickly to the planet's surface via a shuttle craft.\n\nUpon arrival of the security team, one engineer and scientist was killed including while one member of the security team suffered major injuries but survived.\n\nDespite this unfortunate event the survey revealed the planet's resources.", EventTypes.Exploration ) );
    events.Add( new Event.Exploration( "Survey away team sent...", "While on the planet's surface performing a survey the team encountered a strange species.\n\nThe species was harmless yet disruptive and mischievous causing the survey to take 3 hours longer than usual. You lost some survey equipment due to being stolen.\n\nYour survey team couldn't wait to get back onboard the ship and when they finally arrived their data and analysis revealed the planet's resources. The encountered species is not one of the resources.", EventTypes.Exploration ) );
    events.Add( new Event.Exploration( "Survey from orbit...", "The science and engineering team prepare to test new techniques and technologies devised while on the ship.\n\nAs a survey ought to be done on the planet in any case and they thought it'd be a better time than any to carry this out.\n\nA few hiccups occured with the new techniques and technologies that put the team into 12 hours of troubleshooting and refinement including data recapture and analysis.\n\nDespite this the planet's resources were ultimately revealed.", EventTypes.Exploration ) );
    events.Add( new Event.Exploration( "Survey", "A standard survey takes place with no hiccups and planet's resources are revealed.", EventTypes.Exploration ) );

    events.Add( new Event.EnemyEngagement( "Sleeper pursuit!", "Upon arrival at the planet an awaken sleeper sees you on their scanner and immediately pursues to engage you!", EventTypes.EnemyEngagement, AIShip ) );
    events.Add( new Event.EnemyEngagement( "Sleeper reveal!", "Upon maneuvering to orbit at the planet all seems quiet. Suddenly, a dormant sleeper whose signature was masked hiding them from your scanner reveals itself right in front of the bridge!", EventTypes.EnemyEngagement, AIShip ) );
    events.Add( new Event.EnemyEngagement( "Floating dormant Sleeper!", "Upon arriving at the planet you find a dormant Sleeper in physical view range of the bridge. As expected it is not revealed on your scanner.\n\nAs you maneuver to starboard in order to get your close-range weaponry in range to destroy the Sleeper it suddenly awakens and you find yourself in a battle!", EventTypes.EnemyEngagement, AIShip ) );
    events.Add( new Event.EnemyEngagement( "Upfront Sleeper!", "As you immediately arrive at the planet there is a sleeper awake right in front of you facing your ship head on with weapons pointed as if it was waiting for you!", EventTypes.EnemyEngagement, AIShip ) );
    events.Add( new Event.EnemyEngagement( "A Sleeper from the warp trails!", "After being in orbit for a few minutes suddenly a sleeper appears behind your ship!\n\nThe sleeper had been following you when it caught notice of your trajectory to this planet from the last planet you were at!", EventTypes.EnemyEngagement, AIShip ) );
    events.Add( new Event.EnemyEngagement( "Malfunctioning Sleeper!", "After being in orbit for an hour your scanner immediately picks up on a Sleeper signature. Then it disappers a few seconds later. This pattern repeats indefinitely.\n\n It seems that a sleeper is malfunctioning between dormant and awaken mode. After a few repeated patterns the malfunction ceases and stays awaken!", EventTypes.EnemyEngagement, AIShip ) );
    events.Add( new Event.EnemyEngagement( "Orbital patrol Sleeper!", "After being in orbit for half an hour, a Sleeper that seems to patrols the entire planet in orbit catches up with your orbital path incidentally. A battle ensues!", EventTypes.EnemyEngagement, AIShip ) );
    events.Add( new Event.EnemyEngagement( "Sleeper business!", "Upon arriving at the planet you witness just a glimpse of two Sleepers since one had just warped away the second you arrived.\n\nIt seemed there was a cargo trade or delivery of some kind pertaining to Sleeper operations. The remaining sleeper moves to engage you!", EventTypes.EnemyEngagement, AIShip ) );
    events.Add( new Event.EnemyEngagement( "Sleeper!", "A standard engagement with a Sleeper nearby the planet.", EventTypes.EnemyEngagement, AIShip ) );
  }
}