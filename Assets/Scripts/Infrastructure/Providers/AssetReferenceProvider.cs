using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Providers
{
    [Serializable]
    public class AssetReferenceProvider
    {
        [field: SerializeField] public AssetReferenceGameObject DebugRootAssetReference { get; private set; }
    }
}