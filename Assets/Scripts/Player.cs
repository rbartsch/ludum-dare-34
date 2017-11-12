using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Player : MonoBehaviour 
{
  public int nPlanetsExplored = 0;
  public int nMoonsExplored = 0;

  //Tween tween;

  Events events;
  GameUI gameUI;

  public GameObject target;
  public AudioClip ambientClip;
  PlanetarySystem planetarySystem;
  Ship ship;

  public bool inBattle = false;
  public bool won = false;
  public bool lost = false;

  //void MovementComplete ()
  //{
  //  tween.Kill();
  //  transform.localEulerAngles = new Vector3( 0, 0, 0 );
  //}

  void Start ()
  {
    nPlanetsExplored++;
    planetarySystem = GameObject.Find( "PlanetarySystem" ).GetComponent<PlanetarySystem>();
    ship = GetComponent<Ship>();
    events = GameObject.Find( "Events" ).GetComponent<Events>();
    gameUI = GameObject.Find( "GameUI" ).GetComponent<GameUI>();
    //tween = transform.DOMove( new Vector3( 45, 80, 0 ), 15, true ).SetSpeedBased( true );
    //tween.OnComplete( MovementComplete );
    AudioSource audioSrc = GetComponent<AudioSource>();

    audioSrc.clip = ambientClip;
    audioSrc.Play();
  }

  bool move = false;
  bool inPlanetRange = false;
  public void DoMove ()
  {
    if (ship.fuel > 0)
    {
      transform.parent = null;
      move = true;
    }
    else
    {
      Debug.Log( "Not enough fuel." );
    }
  }

  public void DoResupply ()
  {
    if (planet != null && planet.explored && inPlanetRange)
    {
      ship.fuel += planet.fuelCompounds;
      ship.ordnance += planet.ordnanceCompounds;
      ship.repairEquipment += planet.structuralCompounds;
      ship.food += planet.edibleFood;

      planet.fuelCompounds = 0;
      planet.ordnanceCompounds = 0;
      planet.structuralCompounds = 0;
      planet.edibleFood = 0;

      if (ship.repairEquipment > 0)
      {
        if (( ship.shields < ship.maxShields ) && ( ship.shields + ship.repairEquipment > ship.maxShields ))
        {
          int leftOver = ship.shields + ship.repairEquipment;
          leftOver = leftOver - ship.maxShields;
          ship.shields = ship.maxShields;
          ship.repairEquipment -= ( ship.repairEquipment - leftOver );
        }
        else if (( ship.shields < ship.maxShields ) && ( ship.shields + ship.repairEquipment < ship.maxShields ))
        {
          ship.shields += ship.repairEquipment;
          ship.repairEquipment -= ship.repairEquipment;
        }

        if (( ship.armor < ship.maxArmor ) && ( ship.armor + ship.repairEquipment > ship.armor ))
        {
          int leftOver = ship.armor + ship.repairEquipment;
          leftOver = leftOver - ship.maxArmor;
          ship.armor = ship.maxArmor;
          ship.repairEquipment -= ( ship.repairEquipment - leftOver );
        }
        else if (( ship.armor < ship.maxArmor ) && ( ship.armor + ship.repairEquipment < ship.maxArmor ))
        {
          ship.armor += ship.repairEquipment;
          ship.repairEquipment -= ship.repairEquipment;
        }

        if (( ship.hull < ship.maxHull ) && ( ship.hull + ship.repairEquipment > ship.maxHull ))
        {
          int leftOver = ship.hull + ship.repairEquipment;
          leftOver = leftOver - ship.maxHull;
          ship.hull = ship.maxHull;
          ship.repairEquipment -= ( ship.repairEquipment - leftOver );
        }
        else if (( ship.hull < ship.maxHull ) && ( ship.hull + ship.repairEquipment < ship.maxHull ))
        {
          ship.hull += ship.repairEquipment;
          ship.repairEquipment -= ship.repairEquipment;
        }
      }
    }
  }

  void Update ()
  {
    if (nPlanetsExplored == planetarySystem.nPlanets)
    {
      won = true;
      lost = false;
    }
    if (ship.destroyed || ship.fuel <= 0 || ship.food <= 0)
    {
      lost = true;
      won = false;
    }
    if (move)
    {
      Vector3 moveDirection = transform.position - new Vector3( target.transform.position.x, target.transform.position.y, 0 );
      float angle = Mathf.Atan2( moveDirection.y, moveDirection.x ) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis( angle + 90, new Vector3( 0, 0, 1 ) );
      transform.position = Vector3.MoveTowards( transform.position, new Vector3( target.transform.position.x, target.transform.position.y, 0 ), 1f );
    }
  }

  float fuelTime = 1.0f;
  float fuelCurrTime = 1.0f;
  float foodTime = 2.0f;
  float foodCurrTime = 2.0f;
  void FixedUpdate ()
  {
    if (move)
    {
      if (fuelCurrTime >= 0.0f)
      {
        fuelCurrTime -= 1.0f * Time.fixedDeltaTime;
      }
      else
      {
        ship.fuel -= 1;
        fuelCurrTime = fuelTime;
      }

      if (foodCurrTime >= 0.0f)
      {
        foodCurrTime -= 1.0f * Time.fixedDeltaTime;
      }
      else
      {
        ship.food -= 1;
        foodCurrTime = foodTime;
      }
    }
  }

  Planet planet;
  void OnTriggerEnter2D ( Collider2D col )
  {
    if (col.tag == "Planet" && col.gameObject == target)
    {
      if (col.GetComponent<Planet>() != null && !col.GetComponent<Planet>().explored)
      {
        int randomEvent = Random.Range( 0, events.events.Count );
        events.events[ randomEvent ].Popup( gameUI );
        planet = col.GetComponent<Planet>();
        if (events.events[ randomEvent ] is Event.Exploration)
        {
          planet.explored = true;
          nPlanetsExplored++;
          col.transform.FindChild( "Canvas/HomeImage" ).GetComponent<Image>().color = new Color( 255, 255, 255, 255 );
        }
        else
        {
          inBattle = true;
        }
        Debug.Log( "Entered planet" );
        transform.parent = planet.transform;
        move = false;
      }
      else
      {
        planet = col.GetComponent<Planet>();
        Debug.Log( "Already explored" );
      }
    }
  }

  void OnTriggerStay2D ( Collider2D col )
  {
    inPlanetRange = true;
    if (!inBattle && planet != null && !planet.explored)
    {
      planet.explored = true;
      nPlanetsExplored++;
      col.transform.FindChild( "Canvas/HomeImage" ).GetComponent<Image>().color = new Color( 255, 255, 255, 255 );
    }
    if (col.tag == "Planet" && col.gameObject == target && planet != null && planet.explored)
    {
      gameUI.planetTypeStr = planet.type.ToString() + " Type";
      gameUI.fuelCompoundsStr = "Fuel Compounds\t\t\t" + planet.fuelCompounds.ToString();
      gameUI.structuralCompoundsStr = "Structural Compounds\t" + planet.structuralCompounds.ToString();
      gameUI.ordnanceCompoundsStr = "Ordnance Compounds\t" + planet.ordnanceCompounds.ToString();
      gameUI.edibleFoodStr = "Edible Food\t\t\t\t\t" + planet.edibleFood.ToString();
    }
  }

  void OnTriggerExit2D ( Collider2D col )
  {
    inPlanetRange = false;
  }

  //void OnTriggerExit2D ( Collider2D col )
  //{
  //  gameUI.targetStr = "Select a target";
  //  gameUI.planetTypeStr = planet.type.ToString() + " Type";
  //  gameUI.fuelCompoundsStr = "Fuel Compounds\t\t\t" + planet.fuelCompounds.ToString();
  //  gameUI.structuralCompoundsStr = "Structural Compounds\t" + planet.structuralCompounds.ToString();
  //  gameUI.ordnanceCompoundsStr = "Ordnance Compounds\t" + planet.ordnanceCompounds.ToString();
  //  gameUI.edibleFoodStr = "Edible Food\t\t\t\t\t" + planet.edibleFood.ToString();
  //}
}
