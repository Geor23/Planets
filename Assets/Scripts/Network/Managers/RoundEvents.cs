using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


//All classes are per round
//We can make other classes for data that needs to persist across rounds like full game stats etc...



public class RoundEvents : MonoBehaviour {
    private RoundPlayerObjectManager pom;
    private RoundScoreManager sm;
    private PlayerManager pm;
    private PlanetsNetworkManager nm;

    public bool sandBox = false;
    public int timeUntilRespawn = 2;



    void Start(){
        pm = PlayerManager.singleton;
        pom = new RoundPlayerObjectManager();
        sm = new RoundScoreManager(pm.getPlayerDict());
        nm = (PlanetsNetworkManager)PlanetsNetworkManager.singleton;
        //If not sandbox, add to game stats
        if(!sandBox) GameStatsManager.singleton.addNewRoundDatas(pom, sm);

        //Handle messages from server such as end of round signal etc. act upon them

        if(NetworkClient.active) clientInit();
        if(NetworkServer.active) StartCoroutine(serverInit());
    }

    //These can be called inside the playercontroller mobile so that we can tap int othe object manager functionality
    public RoundPlayerObjectManager getRoundPlayerObjectManager(){
        return pom;
    }

    //Same as above, except for scores
    public RoundScoreManager getRoundScoreManager(){
        return sm;
    }

    /* CLIENT FUNCS PLZ */

    // void Update()
    // {
        //PLS no??
        // if (Input.GetKeyDown("m")){
        //     nm.sendScoresToPlayers();
        // }
    // }

    void clientInit(){
        nm.client.RegisterHandler(Msgs.killPlayer, OnClientDeath);
        nm.client.RegisterHandler(Msgs.spawnSelf, OnClientSpawnSelf);
        //Tell server we are ready to spawn
        nm.client.Send(Msgs.playerReadyToSpawn, new UniqueObjectMessage());
    }

    public IEnumerator respawnPlayer(int playerId){
        yield return new WaitForSeconds(timeUntilRespawn);
        PlayerSpawnMsg ps = new PlayerSpawnMsg();
        ps.playerId = playerId;
        Transform obsCamTrans = GameObject.FindGameObjectsWithTag("Observer")[0].transform.GetChild(0);
        ps.pos = obsCamTrans.transform.position; //Observer cam;
        if(pm.getTeam(playerId) == TeamID.TEAM_PIRATES) ps.pos += -obsCamTrans.right*5;
        else ps.pos += obsCamTrans.right*5;
        nm.client.Send(Msgs.spawnPlayer, ps);
    }

    public void registerKill(NetworkInstanceId netId, int playerKilledId, int playerKillerId){
        pom.killPlayerLocal(playerKilledId, playerKillerId);
        KillPlayer kp = new KillPlayer();
        kp.netId = netId;
        nm.client.Send(Msgs.killPlayer, kp);
        StartCoroutine(respawnPlayer(playerKilledId));

        if(sandBox) return;
        pm.addKill(playerKillerId);
        pm.addDeath(playerKilledId);

        Debug.Log("Player " + playerKilledId + " died");
    }

    public void OnClientDeath(NetworkMessage msg){
        //Find player object, and call function
        // deathText.enabled = true; //Causes timer for next spawn to occur
        // deathTimerText.enabled = true;
        // mainCamera.enabled = true;
        ClientScene.RemovePlayer(0);
    }

    public void OnClientSpawnSelf(NetworkMessage msg){
        ClientScene.AddPlayer(nm.client.connection, 0);
    }

    /* SERVER FUNCS PLZ */


    IEnumerator serverInit(){
        NetworkServer.RegisterHandler(Msgs.killPlayer, OnServerRegisterDeath);
        NetworkServer.RegisterHandler(Msgs.spawnPlayer, OnServerSpawnPlayer);
        Queue<int> spawnable = nm.getSpawnablePlayers();
        Queue<int> notChosenTeam = new Queue<int>();
        while(true){
            while(notChosenTeam.Count > 0){
                spawnable.Enqueue(notChosenTeam.Dequeue());
            }
            while(spawnable.Count > 0){
                int pId = spawnable.Dequeue();
                Player p = pm.getPlayer(pId);
                if(p.getPlayerTeam() == TeamID.TEAM_NEUTRAL){
                    //Team not yet picked, return player
                    notChosenTeam.Enqueue(pId);
                } else {
                    p.zeroRoundData();
                    Debug.Log("Spawning " + p.getConnValue());
                    NetworkServer.SendToClient(p.getConnValue(), Msgs.spawnSelf,  new UniqueObjectMessage()); 
                }
            }
            yield return new WaitForSeconds(1);
        }
        // yield return new WaitForSeconds(5);
        // foreach (KeyValuePair<int, Player> kp in pm.getPlayerDict()){
        //     Player p = kp.Value;

        //     if(p.getPlayerTeam() == TeamID.TEAM_OBSERVER){
        //         NetworkServer.SendToClient(p.getConnValue(), Msgs.spawnSelf,  new UniqueObjectMessage());
        //     }
        // }
        // yield return new WaitForSeconds(1);
        // foreach(KeyValuePair<int, Player> kp in pm.getPlayerDict()){
        //     Player p = kp.Value;
        //     if(p.getPlayerTeam() == TeamID.TEAM_OBSERVER) continue;
        //     NetworkServer.SendToClient(p.getConnValue(), Msgs.spawnSelf, new UniqueObjectMessage());
        //     yield return new WaitForSeconds(0.5f);
        // }
    }

    public void OnServerRegisterDeath(NetworkMessage msg){
        KillPlayer kp = msg.ReadMessage<KillPlayer>();
        GameObject obj = NetworkServer.FindLocalObject(kp.netId); // Relay msg to player
        if(obj == null) return;
        NetworkServer.SendToClientOfPlayer(obj, Msgs.killPlayer, kp);
    }

    public void OnServerSpawnPlayer(NetworkMessage msg){
        PlayerSpawnMsg ps = msg.ReadMessage<PlayerSpawnMsg>();
        GameObject player = Instantiate(nm.playerObjectType(ps.playerId), ps.pos, Quaternion.identity) as GameObject;
        player.GetComponent<PlayerControllerMobile>().dictId = ps.playerId;
        Debug.Log("Player " + ps.playerId + "being respawned");
        NetworkServer.AddPlayerForConnection(pm.getNetworkConnection(ps.playerId), player, 0);
    }


    //Extras junk

    public void relayDataToServer(){
        //Call functons inside ObjectManager/ScoreManager respectively to sync them together
        //Possibly called at the end of the round
    }
//Called by Observer upon collecting a resource. Calls score addition in RoundScoreManager
    
}

/// <summary>
//All code in here should have either Server/Client suffixed to it. Client indicates the code is called on a Client.
//Server indicates the code is called on a server.
//TODO means the function still needs doing.
//CHANGE means adjustements are made even if the function works as implemented
/// </summary>