using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitManager : MonoBehaviour {
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;
    public BaseHero SelectedHero;
    public int dice;

    void Awake() {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();

    }

    public void SpawnHeroes() {
        var heroCount = 1;

        for (int i = 0; i < heroCount; i++) {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero);
        }

        GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        var enemyCount = 1;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();
            randomSpawnTile.SetUnit(spawnedEnemy);
        }

        GameManager.Instance.ChangeState(GameState.HeroesTurn);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedHero(BaseHero hero) {
        SelectedHero = hero;
        MenuManager.Instance.ShowSelectedHero(hero);
    }

    public void Defense()
    {
        GameManager.Instance.HasScanned = 2;
        if (GameManager.Instance.MP_Value >= 5)
        {
            dice = Random.Range(1, 10);
            if (dice % 2 == 0 && dice >= 5)
            {
                GameManager.Instance.MP_Value -= 5;
                GameManager.Instance.HP_Value += 10;
                GameManager.Instance.ScanMemo.SetText("Defend Success, HP recover 5");
            }
            else if (dice % 2 == 1 && dice < 5)
            {
                GameManager.Instance.MP_Value -= 10;
                GameManager.Instance.ScanMemo.SetText("Defend Failed, MP decrease 10");
            }
            else
            {
                GameManager.Instance.HP_Value -= 10;
                GameManager.Instance.ScanMemo.SetText("Defead Failed, HP decrease 10");
            }
        }
        else { GameManager.Instance.MP_Value -= 10; }
    }
}
