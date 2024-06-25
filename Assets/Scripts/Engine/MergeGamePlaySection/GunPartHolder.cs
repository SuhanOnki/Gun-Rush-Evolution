using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Engine.GameSections;
using NINESOFT.TUTORIAL_SYSTEM;
using UnityEngine;
using UnityEngine.UI;

namespace Engine.MergeGamePlaySection
{
    public class GunPartHolder : MonoBehaviour
    {
        public static GunPartHolder instance { get; private set; }

        [SerializeField] private ParticleSystem particleGun;
        [SerializeField] private LayerMask layerMaskItem;
        [SerializeField] private LayerMask layerMaskInventory;
        [SerializeField] private LayerMask layerMaskTransparentGun;
        private Quaternion originRotation;
        [SerializeField] private LayerMask layerMaskDecrement;
        [SerializeField] private GameObject[] gunPartsTransparent;
        private Vector3 lastestCellPosition;
        [SerializeField] private GameObject[] gunParts;
        [SerializeField] private Slider slider;
        public PowerVisualizer[] changePowers;
        public List<GunPartUpgradeData> partDatas;
        public GameObject o;
        public int partIndex;
        public int maxLevelPart;
        public bool canPut;
        public GameObject objectHold;
        public float rotateGunSpeed;
        private bool isRotating = false;
        private float startMousePosition;
        private SelectableObjectItem selectableObjectItem;
        private Transform selectableParentTransform;
        private bool putOnGunWork;
        private Cell cellSelected;
        private bool returnRotation;
        public List<GameObject> closestPlace;
        private bool tutorialFirstHand;

        private void Awake()
        {
            instance = this;
        }

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            originRotation = MergeGamePlayState.Instance.guns[0].transform.rotation;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                returnRotation = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit))
                {
                    if (rayHit.collider.CompareTag("Gun"))
                    {
                        isRotating = true;
                        startMousePosition = Input.mousePosition.x;
                        objectHold = rayHit.collider.gameObject;
                        return;
                    }
                    else
                    {
                        MouseButtonDown();
                    }
                }
            }

            if (Input.GetMouseButton(0) && selectableObjectItem)
            {
                MouseButtonPressed();
            }

            if (Input.GetMouseButtonUp(0))
            {
                isRotating = false;
                returnRotation = true;
                if (selectableObjectItem)
                {
                    MouseButtonUp();
                }
            }

            if (returnRotation && objectHold != null)
            {
                objectHold.transform.rotation =
                    Quaternion.Lerp(objectHold.transform.rotation, originRotation, Time.deltaTime * 10);
            }

            if (isRotating)
            {
                float currentMousePosition = Input.mousePosition.x;
                float mouseMovement = currentMousePosition - startMousePosition;
                objectHold.transform.Rotate(Vector3.forward, -mouseMovement * rotateGunSpeed * Time.deltaTime);
                startMousePosition = currentMousePosition;
            }
        }

        private void MouseButtonDown()
        {
            cellSelected = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f, layerMaskItem))
            {
                selectableParentTransform = hit.collider.transform.parent;
                if (MergeGamePlayState.Instance.isTutorial && selectableParentTransform != null)
                {
                    var inventory = selectableParentTransform.parent.gameObject.GetComponent<Cell>();
                    if (inventory != null)
                    {
                        if (Table.Instance.canMerge && inventory.index != 0)
                        {
                            return;
                        }
                    }
                }

                selectableObjectItem = hit.collider.GetComponent<SelectableObjectItem>();
                selectableObjectItem.Select();
                for (int i = 0; i < gunParts.Length; i++)
                {
                    GunPartLevelVisualizer lv = gunParts[i].GetComponent<GunPartLevelVisualizer>();
                    if (lv.GetLevel() < selectableObjectItem.level)
                    {
                        gunPartsTransparent[i].SetActive(true);
                    }
                }
            }
        }

        private void MouseButtonPressed()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance = 4f;

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 15f))
            {
                selectableParentTransform.position = hit.point;
                distance = hit.distance - .4f;
            }


            if (Physics.Raycast(ray, out hit, 10f, layerMaskInventory))
            {
                if (hit.collider.transform.childCount == 0)
                {
                    cellSelected?.Deselect();
                    cellSelected = hit.collider.transform.GetComponent<Cell>();
                }
                else if (hit.collider.transform.childCount == 1)
                {
                    SelectableObjectItem selectable2 = hit.collider.transform.GetChild(0).GetChild(0).GetComponent<SelectableObjectItem>();
                    if (selectableObjectItem.level == selectable2.level &&
                        ((gunParts[selectableObjectItem.level] != null &&
                          (hit.collider.transform != selectableObjectItem.parentItemTransform)) ||
                         (hit.collider.transform == selectableObjectItem.parentItemTransform)))
                    {
                        cellSelected?.Deselect();
                        cellSelected = hit.collider.transform.GetComponent<Cell>();
                    }
                }

                cellSelected?.Selected();
            }
        }

        private void MouseButtonUp()
        {
            if (cellSelected.isLocked)
            {
                return;
            }

            cellSelected?.Deselect();
            var distance = Vector3.Distance(cellSelected.transform.position, selectableObjectItem.transform.parent.position);

            if (distance > 1f)
            {
                PutOnGun();
            }
            else
            {
                if (cellSelected) PutOnGrid();
            }

            if (selectableObjectItem && selectableObjectItem.parentItemTransform.name == "Grid")
            {
                putOnGunWork = true;
                PutOnGun();
            }

            for (int i = 0; i < gunParts.Length; i++)
            {
                gunPartsTransparent[i].SetActive(false);
            }

            putOnGunWork = false;
            selectableObjectItem?.Deselect();
            selectableObjectItem = null;
        }

        private void PutOnGrid()
        {
            if (cellSelected.transform.childCount == 0)
            {
                if (MergeGamePlayState.Instance.isTutorial)
                {
                    return;
                }

                selectableObjectItem.parentItemTransform = cellSelected.transform;
                selectableObjectItem.transform.parent.localPosition = new Vector3(selectableObjectItem.transform.parent.localPosition.x,
                    selectableObjectItem.transform.parent.localPosition.y, 0.05f);
            }
            else if (cellSelected.transform.childCount == 1 &&
                     cellSelected.transform != selectableObjectItem.parentItemTransform)
            {
                SelectableObjectItem selectable2 = cellSelected.transform.GetChild(0).GetChild(0).GetComponent<SelectableObjectItem>();
                if (selectable2.transform.parent.gameObject.GetComponent<CellObject>())
                {
                    var item = selectable2.transform.parent.gameObject.GetComponent<CellObject>();
                    if (item.isWATCHED && item.isRewarded)
                    {
                        if (selectableObjectItem.level == selectable2.level && selectableObjectItem.level <= maxLevelPart)
                        {
                            if (MergeGamePlayState.Instance.isTutorial)
                            {
                                canPut = true;
                                Table.Instance.canMerge = false;
                                TutorialManager.Instance.StageCompleted(0, 5);
                            }


                            var obj = Table.Instance.CreatePrefab_Invoke(selectableObjectItem.level, cellSelected.transform);
                            obj.transform.parent.localPosition = new Vector3(obj.transform.parent.localPosition.x,
                                obj.transform.parent.localPosition.y,
                                obj.transform.parent.localPosition.z + 0.05f);
                            Destroy(cellSelected.transform.GetChild(0).gameObject);
                            Destroy(selectableParentTransform.gameObject);
                            selectableObjectItem = null;
                            Invoke("Timer", .2f);
                        }
                    }
                    else if (!item.isRewarded && !item.isWATCHED)
                    {
                        if (selectableObjectItem.level == selectable2.level && selectableObjectItem.level <= maxLevelPart)
                        {
                            if (MergeGamePlayState.Instance.isTutorial)
                            {
                                canPut = true;
                                Table.Instance.canMerge = false;
                                TutorialManager.Instance.StageCompleted(0, 5);
                            }


                            var obj = Table.Instance.CreatePrefab_Invoke(selectableObjectItem.level, cellSelected.transform);
                            obj.transform.parent.localPosition = new Vector3(obj.transform.parent.localPosition.x,
                                obj.transform.parent.localPosition.y,
                                obj.transform.parent.localPosition.z + 0.05f);
                            Destroy(cellSelected.transform.GetChild(0).gameObject);
                            Destroy(selectableParentTransform.gameObject);
                            selectableObjectItem = null;
                            Invoke("Timer", .2f);
                        }
                    }
                }
                else
                {
                    if (selectableObjectItem.level == selectable2.level && selectableObjectItem.level <= maxLevelPart)
                    {
                        if (MergeGamePlayState.Instance.isTutorial)
                        {
                            canPut = true;
                            Table.Instance.canMerge = false;
                            TutorialManager.Instance.StageCompleted(0, 5);
                        }


                        var obj = Table.Instance.CreatePrefab_Invoke(selectableObjectItem.level, cellSelected.transform);
                        obj.transform.parent.localPosition = new Vector3(obj.transform.parent.localPosition.x,
                            obj.transform.parent.localPosition.y,
                            obj.transform.parent.localPosition.z + 0.05f);
                        Destroy(cellSelected.transform.GetChild(0).gameObject);
                        Destroy(selectableParentTransform.gameObject);
                        selectableObjectItem = null;
                        Invoke("Timer", .2f);
                    }
                }
            }
        }


        private void PutOnGun()
        {
            if (selectableObjectItem != null)
            {
                if (MergeGamePlayState.Instance.isTutorial && tutorialFirstHand && Table.Instance.maxTouch == 2 && !canPut)
                {
                    return;
                }

                closestPlace = closestPlace
                    .OrderBy(o => Vector3.Distance(o.transform.position, selectableObjectItem.transform.parent.transform.position))
                    .ToList();
                /*
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 10f, layerMaskTransparentGun) || putOnGunWork)
                    {*/
                if (closestPlace.Count > 0)
                {
                    o = closestPlace[0];
                    HologramGunPart gunPart = o.GetComponent<HologramGunPart>();
                    if (gunPart)
                    {
                        if (MergeGamePlayState.Instance.isTutorial)
                        {
                            if (!tutorialFirstHand)
                            {
                                tutorialFirstHand = true;
                                Table.Instance.valueTouch = 0;
                                //Grid.Instance.inventories[1].isLocked = false;
                                Table.Instance.maxTouch = 2;
                                Table.Instance.isPressed = false;
                                TutorialManager.Instance.StageCompleted(0, 1);
                            }
                            else
                            {
                                TutorialManager.Instance.StageCompleted(0, 6);
                                MergeGamePlayState.Instance.isTutorial = false;
                                PlayerPrefs.SetInt("battleTutorial", 0);
                                PlayerPrefs.Save();
                            }
                        }

                        GunPartLevelVisualizer lv = gunPart.gunPartLevelVisualizer;
                        var transparentComponenet = closestPlace[0].GetComponent<HologramGunPart>();
                        var oldLevel = lv.level;
                        slider.value += (selectableObjectItem.level - lv.GetLevel());
                        lv.AddValue(selectableObjectItem.level);
                        var oldFireRate = changePowers[1].GetValue();
                        var oldFireRange = changePowers[2].GetValue();
                        for (var i = 0; i < changePowers.Length; i++)
                        {
                            changePowers[i].ChangeValue(true);
                        }

                        var Level = gunPart.gunPartLevelVisualizer.level;
                        var FireRate = changePowers[1].GetValue();
                        var FireRange = changePowers[2].GetValue();
                        Destroy(selectableParentTransform.gameObject);
                        selectableObjectItem = null;
                        Invoke("Timer", .2f);
                        closestPlace.Remove(transparentComponenet.gameObject);

                        DOVirtual.DelayedCall(0.2f, (() => { GunSaving(transparentComponenet); }));
                        if (MergeGamePlayState.Instance.preventShowNewItem)
                        {
                            return;
                        }

                        MergeGamePlayState.Instance.OpenNewItem(gunPart
                                .part[gunPart.gunPartLevelVisualizer.level - 1], Level, FireRate, FireRange, oldLevel, oldFireRate,
                            oldFireRange);
                    }
                }
            }
            //}
        }

        public void EquipPart()
        {
            foreach (var o1 in MergeGamePlayState.Instance.offPanel)
            {
                o1.SetActive(true);
            }

            MergeGamePlayState.Instance.newItemPanel.SetActive(false);
            HologramGunPart gunPart = o.GetComponent<HologramGunPart>();
            if (!gunPart.originGunPart.activeSelf)
            {
                gunPart.originGunPart.gameObject.SetActive(true);
                gunPart.level++;
                PlayerPrefs.SetInt($"GunPartLevel: {o.gameObject.name}", gunPart.level);
                PlayerPrefs.Save();
            }

            DOVirtual.DelayedCall(0.1f, (() =>
            {
                if (AudioEngine.audioEngine.isMusicPlay)
                {
                    AudioEngine.audioEngine.gunAudio.Play();
                }

                particleGun.Play();
            }));
        }

        private void Timer()
        {
            Table.SavingGrid?.Invoke();
        }

        private void GunSaving(HologramGunPart hologramGunPart)
        {
            GunPartUpgradeData gunPartUpgradeData = partDatas[hologramGunPart.gunLevel];
            var part = gunPartUpgradeData.partsUpgrade[hologramGunPart.index]; /*
        */

            /**/
            partDatas[hologramGunPart.gunLevel].partsUpgrade[hologramGunPart.index] =
                hologramGunPart.gunPartLevelVisualizer.level;

            PlayerPrefs.SetInt($"{hologramGunPart.gunLevel} Weapon part {hologramGunPart.index}",
                hologramGunPart.gunPartLevelVisualizer.level);
            PlayerPrefs.Save();
            //GameManagerMerge.SavingGun?.Invoke();
        }
    }
}