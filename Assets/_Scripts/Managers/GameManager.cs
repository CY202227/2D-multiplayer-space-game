using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BaseUnit OccupiedUnit;
    public GameState GameState;
    public TMP_Text Turn_Status;
    public TMP_Text Memo;
    public Button SwitchTurn;
    public TMP_Text HP;
    public int HP_Value;
    public TMP_Text MP;
    public int MP_Value;
    public TMP_Text Enemy_HP;
    public int Enemy_HP_Value;
    private int dice;
    private int selectedWeaponDamage;
    private int selectedWeaponMPCost;
    public int HasMoved;
    public int HasScanned;
    public TMP_Text ScanMemo;
    public bool temp;
    public Image lose;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }
    void Update()
    {
        HP.SetText("Player HP: " + HP_Value.ToString());
        MP.SetText("Player MP: " + MP_Value.ToString());
        if (HP_Value <= 0)
        {
            SceneManager.LoadScene(0);
        }
        
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnHeroes:
                UnitManager.Instance.SpawnHeroes();
                HP_Value = 100;
                MP_Value = 100;
                HP.SetText("Player HP: " + HP_Value.ToString());
                MP.SetText("Player MP: " + MP_Value.ToString());
                break;
            case GameState.SpawnEnemies:
                UnitManager.Instance.SpawnEnemies();
                Enemy_HP_Value = 100;
                Enemy_HP.SetText("Enemy HP: " + Enemy_HP_Value.ToString());
                break;
            case GameState.HeroesTurn:
                Turn_Status.SetText("Current Turn: Captain");
                HP_Value -= 10;
                HasMoved = 0;
                HasScanned = 1;
                SwitchTurn.interactable = true;
                break;
            case GameState.EnemiesTurn:
                break;
            case GameState.GunnerTurn:
                Turn_Status.SetText("Current Turn: Gunner");
                HasMoved = 1;
                HasScanned = 1;
                break;
            case GameState.ScannerTurn:
                Turn_Status.SetText("Current Turn: Scanner");
                ScanMemo.SetText("You can only chose one action");
                HasMoved = 1;
                HasScanned = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void SelectWeapon(int damage, int mpCost)
    {
        selectedWeaponDamage = damage;
        selectedWeaponMPCost = mpCost;
        if (MP_Value >= mpCost)
        {
            MP_Value -= selectedWeaponMPCost; // Decrease player MP
            MP.SetText("Player MP: " + MP_Value.ToString());

            dice = Random.Range(1, 10);
            if (dice % 2 == 0)
            {
                Enemy_HP_Value -= selectedWeaponDamage;
                Memo.SetText("Enemy take damage:" + selectedWeaponDamage);
            }
            else if (dice % 2 == 1 && dice < 5)
            {
                selectedWeaponDamage = damage / 2;
                Enemy_HP_Value -= selectedWeaponDamage;
                Memo.SetText("The shooter shot crooked, but enemy take damage: " + selectedWeaponDamage);
            }
            else
            {
                Memo.SetText("Gunner missed");
            }
        }
        else
        {
            Memo.SetText("Insufficient MP");
        }
        if (Enemy_HP_Value >= 0)
        {
            Enemy_HP.SetText("Enemy HP: " + Enemy_HP_Value.ToString());
        }
        else
        {
            Enemy_HP.SetText("Enemy HP: 0");
        }
        
    }

    public void EnemyDestroy()
    {
        if (Enemy_HP_Value <= 0)
        {
            var enemy = (BaseEnemy)OccupiedUnit;
            Destroy(enemy.gameObject);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    HeroesTurn = 3,
    EnemiesTurn = 4,
    GunnerTurn = 5,
    ScannerTurn = 6
}
