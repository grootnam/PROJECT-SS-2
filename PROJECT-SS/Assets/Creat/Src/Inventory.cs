﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public bool inventoryActivated = false;
    private int money = 0;    // 화폐 보유량
    private Slot[] slots;   // 인벤토리 슬롯의 배열

    [SerializeField]
    private GameObject go_InventoryBase;    // 인벤토리 창

    [SerializeField]
    private GameObject Inventory_panel;     // 블랙 패널

    [SerializeField]
    private GameObject go_SlotsParent;      // 인벤토리 슬롯들의 컨테이너

    [SerializeField]
    private Text go_jewerlyText;            // 화페 UI


    //other UI checking variable
    GameObject upgrade;

    // [화폐획득량 증가] 강화 적용을 위한 변수
    public int getMoreMoney;



    void Start()
    {
        // 인벤토리에 각 슬롯을 저장한다.
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        upgrade =  GameObject.Find("upgrade_base");
        upgrade.SetActive(false);

        getMoreMoney = 0;
    }

    void Update()
    {
        TryOpenInventory();
        money = int.Parse(go_jewerlyText.text);
    }

    // * 인벤토리를 여는 조건을 검사하는 함수
    private void TryOpenInventory()
    {
        // 'I'를 누르면,
        if ((Input.GetKeyDown(KeyCode.I) || (inventoryActivated && Input.GetKeyDown(KeyCode.Escape))) && upgrade.activeSelf == false )
        {
            // 열린 건 닫고, 닫힌 건 열어준다.
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                Time.timeScale = 0f;
                OpenInventory();
            }
            else
            {
                Time.timeScale = 1f;
                CloseInventory();
            }
            
        }
        
    }

    // * 인벤토리 창 여는 함수
    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
        Inventory_panel.SetActive(true);
    }

    // * 인벤토리 창 닫는 함수
    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        Inventory_panel.SetActive(false);
    }

    // * 아이템을 획득(ItemController에서 사용) 함수
    public void AcquireItem(Item _item, int _count = 1)
    {
        // 획득한 Item이 화폐면, 
        if(Item.ItemType.Money == _item.itemType)
        {
            // 보유금액 올려주고, UI에 반영
            money += _item.itemCost + getMoreMoney;
            go_jewerlyText.text = money.ToString();
            return;
        }

        // 획득한 Item이 장비가 아니면,
        if(Item.ItemType.Equipment != _item.itemType)
        {
            money -= _item.itemCost*_count;
            go_jewerlyText.text = money.ToString();
            for (int i = 0; i < slots.Length; i++)
            {
                // 인벤토리에 이미 같은 item이 있을 때
                if(slots[i].item != null)
                {
                    // Count만 증가시킨다.
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
              
            }
        }
        // 빈 칸이 있다면 아이템을 넣어준다.
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
