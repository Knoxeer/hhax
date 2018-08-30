using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using hhax.Extensions;
using UnityEngine;

namespace hhax
{
    public class Menu : MonoBehaviour
    {
        public uLink.NetworkView[] NetworkView;
        private bool _frameAimKey;
        public AnimalStatManagerClient[] Animals;
        public FPSInputController FpsInputController;
        public HashSet<GenericDeviceUsableInterfaceClient> LootCrates = new HashSet<GenericDeviceUsableInterfaceClient>();
        public NetworkEntityManagerPlayerOwner ManagerPlayerOwner;
        public NetworkEntityManagerPlayerProxy[] Players;
        public DestroyInTime[] ResourceNodes;
        public HashSet<GenericDeviceUsableInterfaceClient> Stakes = new HashSet<GenericDeviceUsableInterfaceClient>();
        public NetworkEntityManagerPlayerProxy Target;
        public GenericDeviceUsableInterfaceClient[] UsableItems;
        public HashSet<GenericDeviceUsableInterfaceClient> Wrecks = new HashSet<GenericDeviceUsableInterfaceClient>();


        private void Start()
        {
            StartCoroutine(UpdateNetworkView());
            //
            StartCoroutine(UpdatePlayers());
            StartCoroutine(UpdateAnimals());
            BaseSettings.GetSettings.IsDebug = true;
        }

        private void Update()
        {
            #region FindingObjects

            if (ManagerPlayerOwner == null)
                ManagerPlayerOwner = FindObjectOfType<NetworkEntityManagerPlayerOwner>();

            if (FpsInputController == null)
                FpsInputController = FindObjectOfType<FPSInputController>();

            #endregion

            #region GUIKeys

            if (Input.GetKeyDown(KeyCode.Insert))
                BaseSettings.GetSettings.ShowEspMenu = !BaseSettings.GetSettings.ShowEspMenu;

            #endregion

            #region Dodaj znajomych

            #endregion


            if (Input.GetMouseButton(1) && BaseSettings.GetSettings.AimBotSettings.IsEnabled)
                if (_frameAimKey && Target != null)
                    HuamnAimbot(Target);
                else
                    HuamnAimbot();
            else
                _frameAimKey = false;
           

            if (Input.GetMouseButton(1))
            {
                HuamnAimbot();
            }
        }
        

        private void OnGUI()
        {
            try
            {
                #region Drawing

                if (BaseSettings.GetSettings.ShowEspMenu)
                    Drawing.DrawEspWindow();


                if (BaseSettings.GetSettings.EspSettings.IsEnabled)
                {
                    if (BaseSettings.GetSettings.EspSettings.DrawOwnershipStakes && NetworkView != null)
                        DrawChunkNetworkView("OwnershipStake");
                    if (BaseSettings.GetSettings.EspSettings.DrawWrecks && NetworkView != null)
                    {
                        DrawChunkNetworkView("RoachProxy");
                        DrawChunkNetworkView("GoatProxy");
                        DrawChunkNetworkView("KangaProxy");
                    }
                    if (BaseSettings.GetSettings.EspSettings.DrawPlayers && NetworkView != null)
                        DrawChunkNetworkView("PlayerProxy");
                }


                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    // вкл
                    SetStructLodDist(BaseSettings.GetSettings.EspSettings.StructManLodDist = 0);

                    // Simple BunnyHop Don't work
                   /* while (true)
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            SendKeys.Send("{space}");
                        }
                        Thread.Sleep(180);
                    }
                    */
                }

                if (Input.GetKey(KeyCode.RightAlt))
                {
                    // выкл
                    SetStructLodDist(BaseSettings.GetSettings.EspSettings.StructManLodDist = 600); 
                }


                #endregion

                #region RaycastHit

                if (Input.GetMouseButtonDown(4))
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hit))
                    {
                        print("Dumping objects!");
                        foreach (var component in hit.transform.GetComponents<Component>())
                            print(component);
                    }
                }

                #endregion
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        private int GetKeyState(char v)
        {
            throw new NotImplementedException();
        }

        private void DrawChunkNetworkView(string chunk = "")
        {
            foreach (var nv in NetworkView)
                try
                {
                    if (nv == null || !nv.isActiveAndEnabled)
                        continue;

                    if (chunk == "all") goto all;

                    if (chunk == "" || !nv.name.ToLower().Contains(chunk.ToLower()))
                        continue;

                    all:

                    var distance = Vector3.Distance(ManagerPlayerOwner.transform.position, nv.transform.position);

                    if (distance > BaseSettings.GetSettings.EspSettings.Range)
                        continue;

                    var wtsPlayer = Camera.main.WorldToScreenPoint(nv.transform.position);
                    if (wtsPlayer.z < 0.0)
                        continue;

                    string shortName = nv.name;
                    Color color = Color.white;
                    if (nv.name == "OwnershipStakeDynamicConstructedProxy(Clone)")
                    {
                        shortName = "Тотем";
                        color = Color.black;
                    }
                    else if (nv.name == "RoachProxy(Clone)")
                    {
                        shortName = "Машина";
                        color = Color.red;
                    }
                    else if (nv.name == "GoatProxy(Clone)")
                    {
                        shortName = "Квадбайк";
                        color = Color.red;
                    }
                    else if (nv.name == "KangaProxy(Clone)")
                    {
                        shortName = "Канга";
                        color = Color.red;
                    }
                    else if (nv.name.Contains("PlayerProxy"))
                    {
                        shortName = nv.GetComponent<DisplayProxyName>().Name + "(P)";
                        color = Color.yellow;
                    }
                    Drawing.DrawString(new Vector2(wtsPlayer.x, Screen.height - wtsPlayer.y), color, Drawing.TextFlags.TEXT_FLAG_CENTERED, $"[{shortName} - {Math.Round(distance)}m]");
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                }
        }


        private void HuamnAimbot(NetworkEntityManagerPlayerProxy myTarget = null)
        {
            try
            {
                var target = myTarget ?? AimExtensions.UpdateTargetSelector(Players, ManagerPlayerOwner);

                if (target != null && target.IsNullOrDestroyed() == false)
                {
                    var weapon = ManagerPlayerOwner.GetWeaponCode();
                    Vector3 bodyPosition;

                    switch (weapon)
                    {
                        case ItemCode.Shotgun:
                            bodyPosition = target.GetBone(EHitboxItem.Chest).transform.position;
                            break;
                        case ItemCode.Spear:
                            bodyPosition = target.GetBone(EHitboxItem.Heart).transform.position;
                            break;
                        default:
                            bodyPosition = target.GetBone(EHitboxItem.Head).transform.position;
                            break;
                    }

                    if (weapon != ItemCode.None)
                        AimExtensions.AimCorrection(target, ref bodyPosition, Vector3.Distance(bodyPosition, ManagerPlayerOwner.transform.position), ManagerPlayerOwner.GetWeaponBulletSpeed());


                    FpsInputController.AimAtVec3(bodyPosition);
                    _frameAimKey = true;
                    Target = target;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }


        private void UpdateCrates()
        {
            foreach (var crate in LootCrates)
                if (crate.IsNullOrDestroyed())
                    LootCrates.Remove(crate);


            foreach (var item in UsableItems)
            {
                if (item == null) continue;
                if (LootCrates.Contains(item)) continue;
                if (item.name.ToLower().Contains("loot"))
                    LootCrates.Add(item);
            }
        }

        private void FindWrecks()
        {
            Wrecks.Clear();
            foreach (var item in Wrecks)
            {
                if (item == null) continue;
                if (item.name.ToLower().Contains("goat") || item.name.ToLower().Contains("roach") || item.name.ToLower().Contains("kanga")) Wrecks.Add(item);
            }
        }

        private void FindStakes()
        {
            Stakes.Clear();
            foreach (var stake in Stakes)
            {
                if (stake == null) continue;
                if (stake.name.ToLower().Contains("ship")) Stakes.Add(stake);
            }
        }

        #region Whiles

        public IEnumerator UpdateNetworkView()
        {
            if (BaseSettings.GetSettings.IsDebug)
                Debug.Log("UpdateNetworkView is running");
            while (true)
            {
                try
                {
                    NetworkView = UnityEngine.Object.FindObjectsOfType<uLink.NetworkView>();
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in UpdateNetworkView! " + e);
                }
                yield return new WaitForSeconds(2f);
            }
        }

        public IEnumerator UpdatePlayers()
        {
            if (BaseSettings.GetSettings.IsDebug)
                Debug.Log("UpdateManagerPlayerProxy is running");
            while (true)
            {
                try
                {
                    if (BaseSettings.GetSettings.EspSettings.IsEnabled && BaseSettings.GetSettings.EspSettings.DrawPlayers)
                        Players = FindObjectsOfType<NetworkEntityManagerPlayerProxy>();
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in UpdateManagerPlayerProxy! " + e);
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public IEnumerator UpdateAnimals()
        {
            if (BaseSettings.GetSettings.IsDebug)
                Debug.Log("UpdateAnimals is running");
            while (true)
            {
                try
                {
                    if (BaseSettings.GetSettings.EspSettings.IsEnabled && BaseSettings.GetSettings.EspSettings.DrawAnimals)

                        Animals = FindObjectsOfType<AnimalStatManagerClient>();
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in UpdateAnimals! " + e);
                }
                yield return new WaitForSeconds(5f);
            }
        }

        #endregion

        #region Draw

        private void SetStructLodDist(float Dist)
        {
            StructureManagerLod[] lods = UnityEngine.Object.FindObjectsOfType<StructureManagerLod>();
            foreach (StructureManagerLod lod in lods)
                if (lod.isActiveAndEnabled) lod.LodDistance = Dist;
        }

        private void EnableStructColliders() => DisableStructColliders(false);
        private void DisableStructColliders(bool disable = true)
        {
            StructureManagerCollider[] colliders = UnityEngine.Object.FindObjectsOfType<StructureManagerCollider>();
            foreach (StructureManagerCollider collider in colliders)          
                collider.enabled = !disable;          
        }

        #endregion
    }

    internal class SendKeys
    {
        internal static void Send(string v)
        {
            throw new NotImplementedException();
        }
    }
}
