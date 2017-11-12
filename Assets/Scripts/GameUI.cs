using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour 
{
  PlanetarySystem planetarySystem;
  Player player;
  Ship ship;
  BattleSystem battleSystem;

  // top panel
  Text systemSummaryTxt; string systemSummaryStr;
  Text shipSummaryTxt; string shipSummaryStr;

  Transform targetPanel;
  Text targetTxt; public string targetStr;
  Text planetTypeTxt; public string planetTypeStr;
  Text fuelCompoundsTxt; public string fuelCompoundsStr;
  Text structuralCompoundsTxt; public string structuralCompoundsStr;
  Text ordnanceCompoundsTxt; public string ordnanceCompoundsStr;
  Text edibleFoodTxt; public string edibleFoodStr;

  Transform shipPanel;
  Text yourShipTxt;
  Text yourShieldsTxt;
  Text yourArmorTxt; 
  Text yourHullTxt;
  Text yourDamageOutputTxt;

  Transform eventPopupPanel;
  Text eventTitleTxt;
  Text eventDescrTxt;

  Transform battlePanel;
  Text battleTitleTxt;
  Text battleLogTxt;
  Text battleTimerTxt;
  Image yourShipImg;
  Image AIShipImg;

  Transform shipTargetPanel;
  Text shipTargetShieldsTxt; public string shipTargetShieldsStr;
  Text shipTargetArmorTxt; public string shipTargetArmorStr;
  Text shipTargetHullTxt; public string shipTargetHullStr;
  Text shipTargetDamageOutputTxt; public string shipTargetDamageOutputStr;

  Transform gameOverPanel;
  Text gameOverTitleTxt;
  Text gameOverDescrTxt;

  Transform escapeMenuPanel;

  Transform plotPanel;

  void Start ()
  {
    planetarySystem = GameObject.Find( "PlanetarySystem" ).GetComponent<PlanetarySystem>();
    player = GameObject.Find( "PlayerShip" ).GetComponent<Player>();
    ship = GameObject.Find( "PlayerShip" ).GetComponent<Ship>();
    battleSystem = GameObject.Find( "BattleSystem" ).GetComponent<BattleSystem>();

    targetPanel = GameObject.Find( "TargetPanel" ).transform;
    targetTxt = targetPanel.FindChild( "TargetTxt" ).GetComponent<Text>();
    planetTypeTxt = targetPanel.FindChild( "PlanetTypeTxt" ).GetComponent<Text>();
    fuelCompoundsTxt = targetPanel.FindChild( "FuelCompoundsTxt" ).GetComponent<Text>();
    structuralCompoundsTxt = targetPanel.Find( "StructuralCompoundsTxt" ).GetComponent<Text>();
    ordnanceCompoundsTxt = targetPanel.Find( "OrdnanceCompoundsTxt" ).GetComponent<Text>();
    edibleFoodTxt = targetPanel.Find( "EdibleFoodTxt" ).GetComponent<Text>();
    targetStr = "Select a target";
    planetTypeStr = "Unknown Planet Type";
    fuelCompoundsStr = "Unknown Resource 0";
    structuralCompoundsStr = "Unknown Resource 0";
    ordnanceCompoundsStr = "Unknown Resource 0";
    edibleFoodStr = "Unknown Resource 0";

    shipPanel = GameObject.Find( "ShipPanel" ).transform;
    yourShipTxt = shipPanel.FindChild( "YourShipTxt" ).GetComponent<Text>();
    yourShieldsTxt = shipPanel.FindChild( "ShieldsTxt" ).GetComponent<Text>();
    yourArmorTxt = shipPanel.FindChild( "ArmorTxt" ).GetComponent<Text>();
    yourHullTxt = shipPanel.FindChild( "HullTxt" ).GetComponent<Text>();
    yourDamageOutputTxt = shipPanel.FindChild( "DamageOutputTxt" ).GetComponent<Text>();
    yourShipTxt.text = "Your Ship — \"" + ship.shipName + "\"";
    yourShieldsTxt.text = "Shields " + ship.shields + "/" + ship.maxShields;
    yourArmorTxt.text = "Armor " + ship.armor + "/" + ship.maxArmor;
    yourHullTxt.text = "Hull " + ship.hull + "/" + ship.maxHull;
    yourDamageOutputTxt.text = "Damage Output " + ship.damageOutput;

    eventPopupPanel = GameObject.Find( "EventPopupPanel" ).transform;
    eventTitleTxt = eventPopupPanel.FindChild( "EventTitleTxt" ).GetComponent<Text>();
    eventDescrTxt = eventPopupPanel.FindChild( "EventDescrTxt" ).GetComponent<Text>();
    eventPopupPanel.gameObject.SetActive( false );

    battlePanel = GameObject.Find( "BattlePanel" ).transform;
    battleTitleTxt = battlePanel.FindChild( "BattleTitleTxt" ).GetComponent<Text>();
    battleLogTxt = battlePanel.FindChild( "BattleLogTxt" ).GetComponent<Text>();
    battleTimerTxt = battlePanel.FindChild( "BattleTimerTxt" ).GetComponent<Text>();
    yourShipImg = battlePanel.FindChild( "YourShipImg" ).GetComponent<Image>();
    AIShipImg = battlePanel.Find( "AIShipImg" ).GetComponent<Image>();
    battlePanel.gameObject.SetActive( false );

    shipTargetPanel = GameObject.Find( "ShipTargetPanel" ).transform;
    shipTargetShieldsTxt = shipTargetPanel.FindChild( "ShieldsTxt" ).GetComponent<Text>();
    shipTargetArmorTxt = shipTargetPanel.FindChild( "ArmorTxt" ).GetComponent<Text>();
    shipTargetHullTxt = shipTargetPanel.FindChild( "HullTxt" ).GetComponent<Text>();
    shipTargetDamageOutputTxt = shipTargetPanel.FindChild( "DamageOutputTxt" ).GetComponent<Text>();
    shipTargetPanel.gameObject.SetActive( false );

    systemSummaryTxt = GameObject.Find( "TopPanel/SystemSummaryTxt" ).GetComponent<Text>();
    shipSummaryTxt = GameObject.Find( "TopPanel/ShipSummaryTxt" ).GetComponent<Text>();

    shipSummaryTxt.text = string.Format( "[ Ship Summary | Fuel: {0} — Ordnance: {1} — Food: {2} — Repair Equipment: {3} ]", ship.fuel, ship.ordnance, ship.food, ship.repairEquipment );
    systemSummaryStr = string.Format( "[ Explored | Planets: {0}/{1} ]", player.nPlanetsExplored, planetarySystem.nPlanets );

    gameOverPanel = GameObject.Find( "GameOverPanel" ).transform;
    gameOverTitleTxt = gameOverPanel.FindChild( "GameOverTitleTxt" ).GetComponent<Text>();
    gameOverDescrTxt = gameOverPanel.FindChild( "GameOverDescrTxt" ).GetComponent<Text>();
    gameOverPanel.gameObject.SetActive( false );

    escapeMenuPanel = GameObject.Find( "EscapeMenuPanel" ).transform;
    escapeMenuPanel.gameObject.SetActive( false );

    plotPanel = GameObject.Find( "PlotPanel" ).transform;
    plotPanel.gameObject.SetActive( true );
    Time.timeScale = 0.0f;
  }

  IEvent tmpEvent;
  public void EventPopup (string eventTitle, string eventDescr, IEvent _event)
  {
    tmpEvent = _event;
    eventPopupPanel.gameObject.SetActive( true );
    eventTitleTxt.text = eventTitle;
    eventDescrTxt.text = eventDescr;
    Time.timeScale = 0.0f;
  }

  public void OkayEventPopup ()
  {
    if (tmpEvent is Event.EnemyEngagement)
    {
      battlePanel.gameObject.SetActive( true );
    }
    tmpEvent.ExecuteEvent();
    eventPopupPanel.gameObject.SetActive( false );
    eventTitleTxt.text = "Event Title";
    eventDescrTxt.text = "Event Description (if this is showing it's a bug!)";
    Time.timeScale = 1.0f;
    if (tmpEvent is Event.EnemyEngagement)
    {
      BattleEventPopup();
    }
    tmpEvent = null;
  }

  public void BattleEventPopup ()
  {
    targetPanel.gameObject.SetActive( false );
    shipTargetPanel.gameObject.SetActive( true );
    Event.EnemyEngagement tmp = (Event.EnemyEngagement)tmpEvent;
    //Event.EnemyEngagement tmp = tmpEvent as Event.EnemyEngagement;
    battleTitleTxt.text = "\"" + player.GetComponent<Ship>().shipName + "\" vs. \"" + tmp.ship.shipName + "\"";
    AIShipImg.sprite = tmp.ship.GetComponent<SpriteRenderer>().sprite;
    AIShipImg.color = tmp.ship.GetComponent<SpriteRenderer>().color;
  }

  public void ExitBattlEventPopup ()
  {
    targetPanel.gameObject.SetActive( true );
    shipTargetPanel.gameObject.SetActive( false );
    battlePanel.gameObject.SetActive( false );
  }

  public void OkayPlot ()
  {
    plotPanel.gameObject.SetActive( false );
    Time.timeScale = 1.0f;
  }

  public void NextTurn ()
  {
    if (battleSystem.turn != Turn.WaitingOnPlayer)
    {
      Debug.Log( "Not your turn yet." );
      return;
    }
    battleSystem.turn = Turn.AI;
  }

  public void Disengage ()
  {
    if (battleSystem.turn == Turn.WaitingOnPlayer)
    {
      battleSystem.turn = Turn.Disengage;
    }
    else
    {
      Debug.Log( "Not your turn yet." );
    }
  }

  public void Retry ()
  {
    Time.timeScale = 1.0f;
    Application.LoadLevel( "Game" );
  }

  public void ExitGame ()
  {
    Time.timeScale = 1.0f;
    Application.LoadLevel( "MainMenu" );
  }

  bool showEscapeMenu = false;
  void EscapeMenu ()
  {
    escapeMenuPanel.gameObject.SetActive( true );
    Time.timeScale = 0.0f;
  }

  void Update ()
  {
    if (Input.GetKeyDown( KeyCode.Escape ))
    {
      showEscapeMenu = !showEscapeMenu;

      if (!showEscapeMenu)
      {
        Time.timeScale = 1.0f;
      }
    }

    if (showEscapeMenu)
    {
      EscapeMenu();
      return;
    }
    else
    {
      escapeMenuPanel.gameObject.SetActive( false );
    }

    shipSummaryTxt.text = string.Format( "[ Ship Summary | Fuel: {0} — Ordnance: {1} — Food: {2} — Repair Equipment: {3} ]", ship.fuel, ship.ordnance, ship.food, ship.repairEquipment );
    systemSummaryTxt.text = string.Format( "[ Explored | Planets: {0}/{1} ]", player.nPlanetsExplored, planetarySystem.nPlanets);
    targetTxt.text = targetStr;
    planetTypeTxt.text = planetTypeStr;
    fuelCompoundsTxt.text = fuelCompoundsStr;
    structuralCompoundsTxt.text = structuralCompoundsStr;
    ordnanceCompoundsTxt.text = ordnanceCompoundsStr;
    edibleFoodTxt.text = edibleFoodStr;

    yourShieldsTxt.text = "Shields " + ship.shields + "/" + ship.maxShields;
    yourArmorTxt.text = "Armor " + ship.armor + "/" + ship.maxArmor;
    yourHullTxt.text = "Hull " + ship.hull + "/" + ship.maxHull;
    yourDamageOutputTxt.text = "Damage Output " + ship.damageOutput;

    if (player.won)
    {
      Time.timeScale = 0.2f;
      gameOverPanel.gameObject.SetActive( true );
      gameOverTitleTxt.text = "Last-ditch voyage mission a success!";
      gameOverDescrTxt.text = "Your last-ditch voyage was successful, you kept your ship running, explored the system's planets and accumulated resources that your planet depends on. Your success mission reduced the growing concerns on your planet of conflict erupting again and for now it is peaceful.";
      battlePanel.gameObject.SetActive( false );
      shipTargetPanel.gameObject.SetActive( false );
      eventPopupPanel.gameObject.SetActive( false );
      targetPanel.gameObject.SetActive( false );
      return;
    }
    else if (player.lost)
    {
      Time.timeScale = 0.2f;
      gameOverPanel.gameObject.SetActive( true );
      gameOverTitleTxt.text = "Last-ditch voyage mission a failure!";
      gameOverDescrTxt.text = "Your last-ditch voyage ended in failure! You were unable to keep your ship running and explore the system's planets. The growing concerns on your planet reached its tipping point and world-wide conflict broke out once again.";
      battlePanel.gameObject.SetActive( false );
      shipTargetPanel.gameObject.SetActive( false );
      eventPopupPanel.gameObject.SetActive( false );
      targetPanel.gameObject.SetActive( false );
      return;
    }

    if (battleSystem.startBattle == false || battleSystem.AIShip == null)
    {
      ExitBattlEventPopup();
      return;
    }
    battleLogTxt.text = battleSystem.battleLog;
    battleTimerTxt.text = battleSystem.currTime.ToString( "0.0s" );
    shipTargetShieldsTxt.text = "Shields " + battleSystem.AIShip.shields + "/" + battleSystem.AIShip.maxShields;
    shipTargetArmorTxt.text = "Armor " + battleSystem.AIShip.armor + "/" + battleSystem.AIShip.maxArmor;
    shipTargetHullTxt.text = "Hull " + battleSystem.AIShip.hull + "/" + battleSystem.AIShip.maxHull;
    shipTargetDamageOutputTxt.text = "Damage Output " + battleSystem.AIShip.damageOutput;
  }
}