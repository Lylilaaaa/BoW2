using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class donotDestroyManager : MonoBehaviour
{
    private static donotDestroyManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            // ���ʵ�������ڣ��򽫸ö�����Ϊ���ᱻ���ٵĶ���
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���ʵ���Ѵ��ڣ������ٵ�ǰ�����Ա�ֻ֤��һ��ʵ������
            Destroy(gameObject);
        }
    }
}
