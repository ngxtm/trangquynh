using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API : MonoBehaviour
{
    
    static bool _isLocal = true;
    public static MasterData _masterData;
    public static PlayerData _playerData;

    public static MasterData MasterData { set => _masterData = value; get => _masterData; }
    public static PlayerData PlayerData { set => _playerData = value; get => _playerData; }


    public static IEnumerator LoadMasterData()
    {
        if (_isLocal)
        {
            _masterData = new MasterData();
            _masterData.piece = new MasterData.Piece
            {
                quan = new MasterData.Piece.Quan(),
                dan = new MasterData.Piece.Dan()
            };
            _masterData.piece.quan = new MasterData.Piece.Quan
            {
                score = 10
            };
            _masterData.piece.dan = new MasterData.Piece.Dan
            {
                score = 1
            };
        }
        else
        {
            
        }
        yield return null;
    }

    public static IEnumerator LoadPlayerData()
    {
        if (_isLocal)
        {
            _playerData = new PlayerData();
            _playerData.profile = new PlayerData.Profile
            {
                uid = 0,
                name = string.Empty,
            };
        }
        else
        {

        }

        yield return null;
    }

}
