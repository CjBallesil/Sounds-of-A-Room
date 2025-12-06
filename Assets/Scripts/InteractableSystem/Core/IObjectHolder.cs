using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectHolder
{
    void AddObject(GameObject obj);
    void ReleaseObject(GameObject obj);
}
