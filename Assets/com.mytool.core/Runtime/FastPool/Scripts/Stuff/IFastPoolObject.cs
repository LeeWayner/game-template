using UnityEngine;
using System.Collections;

namespace MyTool.Core.Runtime.Pool
{
    public interface IFastPoolItem
    {
        void OnFastInstantiate();
        void OnFastDestroy();
    }
}