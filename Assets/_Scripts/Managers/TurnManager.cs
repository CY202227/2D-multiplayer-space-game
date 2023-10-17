using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public Button SwitchTurn;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CaptionTurn()
    {
            GameManager.Instance.ChangeState(GameState.GunnerTurn);
            Debug.Log(GameManager.Instance.GameState);
    }
    public void GunnerTurn()
    {
            GameManager.Instance.ChangeState(GameState.ScannerTurn);
            Debug.Log(GameManager.Instance.GameState);
    }
    public void ScannerTurn()
    {
            GameManager.Instance.ChangeState(GameState.HeroesTurn);
            Debug.Log(GameManager.Instance.GameState);
    }

}
