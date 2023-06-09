using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class tempCurSorOutlineWood : MonoBehaviour
{
    private GameObject outlineGbj;
    public bool mouseEnter;
    public bool _canDisappear = true;
    public bool mergeFirst;
    public bool mergeSecond;
    public bool finishmerge = false;
    public bool finish = false;
    public bool finish1 = false;

    public GameObject tempMergePenal;
    // Start is called before the first frame update
    void Start()
    {
        finishmerge = false;
        mouseEnter = false;
        _canDisappear = true;
        outlineGbj = FindChildWithTag(transform, "outline").gameObject;
        tempMergePenal.SetActive(false);
    }

    private void Update()
    {
        if (GlobalVar._instance.finishMerge == true && finish1 == false)
        {
            tempMergePenal.SetActive(false);
            GlobalVar._instance.showMergable = false;
            GlobalVar._instance.canDeleteWoodMerge = true;
            finishmerge = true;
            finish1 = true;
        }
        if (mouseEnter == false && _canDisappear == true)
        {
            outlineGbj.SetActive(false);
        }

        if (mouseEnter == true)
        {
            if (Input.GetMouseButtonDown(0) && GlobalVar._instance.showMergable == false)
            {
                GlobalVar._instance.ChangeState("MergeTowerUI");
                GlobalVar._instance.tempMerge1 = true;
                mergeFirst = true;
            }
        }

        if (GlobalVar._instance.showMergable == true && mergeFirst != true)
        {
            tempMergePenal.SetActive(true);
            mergeSecond = true;

            //merge 成功
            if (mouseEnter == true && Input.GetMouseButtonDown(0) && finish == false)
            {
                ContractInteraction._instance.EditMergeTower();
                
                //commont when build!!!
                // tempMergePenal.SetActive(false);
                // GlobalVar._instance.showMergable = false;
                // GlobalVar._instance.canDeleteWoodMerge = true;
                // finishmerge = true;
                
                finish = true;
                // GlobalVar._instance.tempMerge2 = true;
            }
            //enable mergeable
        }
        if (mouseEnter == true && mergeSecond == true && finishmerge == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                GlobalVar._instance.tempMerge2 = true;
                GlobalVar._instance.ChangeState("MergeTowerUI");
            }
        }
        if (GlobalVar._instance.canDeleteWoodMerge == true && mergeSecond == false)
        {
            transform.parent.GetComponent<FieldInit>().woodType = 0;
            Destroy(gameObject);
        }
    }

    private Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }

            Transform result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
    private void OnMouseEnter()
    {
        mouseEnter = true;

        outlineGbj.SetActive(true);

    }
    private void OnMouseExit()
    {
        mouseEnter = false;
    }

    public void CloseMerge()
    {
        
    }

}
