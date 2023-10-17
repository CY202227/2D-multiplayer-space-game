using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public abstract class Tile : MonoBehaviour {
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;
    public int HasMoved;
    public int HasScan;
    public int EHP;
    private int dice;


    public virtual void Init(int x, int y)
    {
    }
    void Update()
    {
        EHP = GameManager.Instance.Enemy_HP_Value;  // Get the current enemy hit points from GameManager.

        if (EHP == 0)
        {
            GameManager.Instance.Enemy_HP_Value = -1;  // Set enemy hit points to -1 (flagging that the enemy is defeated).

            // Find and destroy the enemy on the tiles.
            Tile[] allTiles = FindObjectsOfType<Tile>();  // Find all the tiles in the scene.
            foreach (Tile tile in allTiles)
            {
                if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Enemy)
                {
                    Destroy(tile.OccupiedUnit.gameObject);  // Destroy the enemy's GameObject.
                    tile.OccupiedUnit = null;  // Clear the tile's occupied unit reference.
                }
            }
        }
        else
        {
            return;  // Exit the function early if enemy hit points are not zero.
        }
    }


    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    void OnMouseDown() {
        HasMoved = GameManager.Instance.HasMoved;
        HasScan = GameManager.Instance.HasScanned;

        //if (GameManager.Instance.GameState != GameState.HeroesTurn) return;
        if (OccupiedUnit != null) {
            if (OccupiedUnit.Faction == Faction.Hero && HasMoved == 0) UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
            else if (GameManager.Instance.GameState == GameState.ScannerTurn && HasScan <= 2)
            {
                GameManager.Instance.HasScanned += 1;
                dice = Random.Range(1, 10);
                
                if (dice % 2 == 0 && GameManager.Instance.temp == false)
                {
                    var enemy = (BaseEnemy)OccupiedUnit;
                    enemy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    GameManager.Instance.ScanMemo.SetText("Enemy has been Scanned");
                    GameManager.Instance.temp = true;

                }
                else
                {
                    if (HasScan == 2)
                    {
                        GameManager.Instance.ScanMemo.SetText("Scan failed");
                    }
                }
                Debug.Log("Scanned");
            }
        }
        else {
            if (UnitManager.Instance.SelectedHero != null && HasMoved == 0) {
                SetUnit(UnitManager.Instance.SelectedHero);
                UnitManager.Instance.SetSelectedHero(null);
                GameManager.Instance.HasMoved = 1;
            }
            else if (GameManager.Instance.GameState == GameState.ScannerTurn)
            {
                GameManager.Instance.HasScanned +=1;
                if (HasScan == 2)
                {
                    GameManager.Instance.ScanMemo.SetText("Scan failed");
                }
                Debug.Log("Scanned");
            }
        }

    }

    public void SetUnit(BaseUnit unit) {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}