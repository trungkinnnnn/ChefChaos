using UnityEngine;

public class ContentTextUI : MonoBehaviour
{
    public static ContentTextUI Instance;

    [SerializeField] Transform _parentTransform;
    [SerializeField] GameObject _pricePrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    // ==================== Service ======================

    public void CreatePriceText(Vector3 position, string textMesh, string textBonus = "")
    {
        var obj = PoolManager.Instance.Spawner(_pricePrefab, position, Quaternion.identity);
        obj.transform.SetParent(_parentTransform);

        if(obj.TryGetComponent<ContentText>(out ContentText priceText))
        {
            priceText.InitMoneyUI(textMesh, textBonus);
        }
    }

    public void CreateInfoText(Vector3 position, string textMesh)
    {
        var obj = PoolManager.Instance.Spawner(_pricePrefab, position, Quaternion.identity);
        obj.transform.SetParent(_parentTransform);

        if (obj.TryGetComponent<ContentText>(out ContentText priceText))
        {
            priceText.InitTextInfoUI(textMesh);
        }
    }

}
