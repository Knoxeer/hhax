using System;
using System.Collections;
using hhax.Extensions;
using UnityEngine;

namespace hhax
{
    public class Menu : MonoBehaviour
    {
        public uLink.NetworkView[] NetworkView;
        private bool _frameAimKey;
        public FPSInputController FpsInputController;
        public NetworkEntityManagerPlayerOwner ManagerPlayerOwner;
        public NetworkEntityManagerPlayerProxy[] Players;
        public NetworkEntityManagerPlayerProxy Target;


        private void Start()
        {
            StartCoroutine(UpdateNetworkView());
            StartCoroutine(UpdatePlayers());
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
                    if (BaseSettings.GetSettings.EspSettings.StorageLocker && NetworkView != null)
                        DrawChunkNetworkView("StorageCrateDynamicConstructedProxy");
                    if (BaseSettings.GetSettings.EspSettings.FirePit && NetworkView != null)
                        DrawChunkNetworkView("FirepitDynamicConstructedProxy");
                    if (BaseSettings.GetSettings.EspSettings.LootCache && NetworkView != null)
                        DrawChunkNetworkView("LootCacheProxy");
                    if (BaseSettings.GetSettings.EspSettings.LogResourceNode && NetworkView != null)
                        DrawChunkNetworkView("LogResourceNode");
                    if (BaseSettings.GetSettings.EspSettings.FlintRock && NetworkView != null)
                        DrawChunkNetworkView("FlintRockResourceNode");
                    if (BaseSettings.GetSettings.EspSettings.Metal2Resource && NetworkView != null)
                        DrawChunkNetworkView("Metal2ResourceNode");
                    if (BaseSettings.GetSettings.EspSettings.Metal3Resource && NetworkView != null)
                        DrawChunkNetworkView("Metal3ResourceNode");
                    if (BaseSettings.GetSettings.EspSettings.Metal4Resource && NetworkView != null)
                        DrawChunkNetworkView("Metal4ResourceNode");
                    if (BaseSettings.GetSettings.EspSettings.IronRockResource && NetworkView != null)
                        DrawChunkNetworkView("IronRockResourceNode");
                    if (BaseSettings.GetSettings.EspSettings.CoalRockResource && NetworkView != null)
                        DrawChunkNetworkView("CoalRockResourceNode(Clone)");
                    if (BaseSettings.GetSettings.EspSettings.SandstoneResource && NetworkView != null)
                        DrawChunkNetworkView("SandstoneResourceNode(Clone)");
                    if (BaseSettings.GetSettings.EspSettings.AIShigiForest && NetworkView != null)
                        DrawChunkNetworkView("AIShigiForestProxy(Clone)");
                    if (BaseSettings.GetSettings.EspSettings.AIBorProxy && NetworkView != null)
                        DrawChunkNetworkView("AIBorProxy(Clone)");
                    if (BaseSettings.GetSettings.EspSettings.AIShigiProxy && NetworkView != null)
                        DrawChunkNetworkView("AIShigiProxy(Clone)");
                    if (BaseSettings.GetSettings.EspSettings.AIYetiForest && NetworkView != null)
                        DrawChunkNetworkView("AIYetiForestProxy(Clone)");
                    if (BaseSettings.GetSettings.EspSettings.AITokarProxy && NetworkView != null)
                        DrawChunkNetworkView("AITokarProxy(Clone)");
                    if (BaseSettings.GetSettings.EspSettings.WorkbenchDynamic && NetworkView != null)
                        DrawChunkNetworkView("WorkbenchDynamicConstructedProxy(Clone)");
                    if (BaseSettings.GetSettings.EspSettings.c4Dynamic && NetworkView != null)
                        DrawChunkNetworkView("C4DynamicObjectProxy(Clone)");
                    if (BaseSettings.GetSettings.EspSettings.SleeperLootCrate && NetworkView != null)
                        DrawChunkNetworkView("SleeperLootCrateProxy(Clone)");
                }


                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    SetStructLodDist(BaseSettings.GetSettings.EspSettings.StructManLodDist = 0);
                }

                if (Input.GetKey(KeyCode.RightAlt))
                {
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

                    if (distance > 500f)
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
                    else if (nv.name == "StorageCrateDynamicConstructedProxy(Clone)")
                    {
                        shortName = "Ящик";
                        color = Color.red;
                    }
                    else if (nv.name == "FirepitDynamicConstructedProxy(Clone)")
                    {
                        shortName = "Жаровня";
                        color = Color.red;
                    }
                    else if (nv.name == "LootCacheProxy(Clone)")
                    {
                        shortName = "Лут";
                        color = Color.red;
                    }
                    else if (nv.name == "LogResourceNode(Clone)")
                    {
                        shortName = "Дерево";
                        color = Color.red;
                    }
                    else if (nv.name == "FlintRockResourceNode(Clone)")
                    {
                        shortName = "Кремень";
                        color = Color.red;
                    }
                    else if (nv.name == "Metal2ResourceNode(Clone)")
                    {
                        shortName = "Краснуха";
                        color = Color.red;
                    }
                    else if (nv.name == "Metal3ResourceNode(Clone)")
                    {
                        shortName = "Зеленка";
                        color = Color.red;
                    }
                    else if (nv.name == "Metal4ResourceNode(Clone)")
                    {
                        shortName = "Синька";
                        color = Color.red;
                    }
                    else if (nv.name == "IronRockResourceNode(Clone)")
                    {
                        shortName = "Железо";
                        color = Color.red;
                    }
                    else if (nv.name == "CoalRockResourceNode(Clone)")
                    {
                        shortName = "Уголь";
                        color = Color.red;
                    }
                    else if (nv.name == "SandstoneResourceNode(Clone)")
                    {
                        shortName = "Глина";
                        color = Color.red;
                    }
                    else if (nv.name == "AIShigiForestProxy(Clone)")
                    {
                        shortName = "Олень";
                        color = Color.red;
                    }
                    else if (nv.name == "AIBorProxy(Clone)")
                    {
                        shortName = "Кабан";
                        color = Color.red;
                    }
                    else if (nv.name == "AIBorProxy(Clone)")
                    {
                        shortName = "Кабан";
                        color = Color.red;
                    }
                    else if (nv.name == "AIShigiProxy(Clone)")
                    {
                        shortName = "Заяц";
                        color = Color.red;
                    }
                    else if (nv.name == "AIYetiForestProxy(Clone)")
                    {
                        shortName = "Йети";
                        color = Color.red;
                    }
                    else if (nv.name == "AITokarProxy(Clone)")
                    {
                        shortName = "Токар";
                        color = Color.red;
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
                    else if (nv.name == "WorkbenchDynamicConstructedProxy(Clone)")
                    {
                        shortName = "Верстак";
                        color = Color.red;
                    }
                    else if (nv.name == "C4DynamicObjectProxy(Clone)")
                    {
                        shortName = "Цешка";
                        color = Color.red;
                    }
                    else if (nv.name == "SleeperLootCrateProxy(Clone)")
                    {
                        shortName = "Слипер";
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

        #region Whiles

        public IEnumerator UpdateNetworkView()
        {
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

        #endregion

        #region Draw
        // Дальность прорисовки текстур
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
}
