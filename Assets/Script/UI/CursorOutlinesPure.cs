using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorOutlinesPure : MonoBehaviour
{
    private GameObject outlineGbj;

    public bool mouseEnter;
    public bool _canDisappear = true;
    public bool cursorZoomIn=false;
    public GameObject previewLevelInfoPenal;
    
    // Start is called before the first frame update
    void Start()
    {
        mouseEnter = false;
        _canDisappear = true;
        outlineGbj = FindChildWithTag(transform, "outline").gameObject;
        previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(false);
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

    private void Update()
    {
        if (mouseEnter == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _canDisappear = false;
                cursorZoomIn = true;
                if (GlobalVar._instance.isPreViewing == false) //不能同时打开两个viewing //load viewing Scene传入数据 //改变Global node
                {
                    previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(true);
                    //CameraController._instance.camLock = true;
                    //SceneManager.LoadScene("ExhibExample", LoadSceneMode.Additive);
                    GlobalVar._instance.isPreViewing = true;
                }
            }
        }
        if (cursorZoomIn == true)
        {
            CameraController._instance.LookUpNode(transform);
            CameraController._instance.canMove = true;
           // StartCoroutine(ChangeVariableAfterDelay());
            cursorZoomIn = false;
        }
        

        if (mouseEnter == false && _canDisappear == true)
        {
            outlineGbj.SetActive(false);
        }
    }

    // Update is called once per frame
    private void OnMouseEnter()
    {
        mouseEnter = true;

        outlineGbj.SetActive(true);

    }
    private void OnMouseExit()
    {
        mouseEnter = false;
    }
}
