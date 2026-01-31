using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PurchasingManager : MonoBehaviour, IStoreListener
{
    [SerializeField] private Button _buyCoinBtn; 
    private const string PRODUCT_COIN_2000 = "coin_2000"; 
    private const int COIN_VALUE = 2000; 
    private IStoreController storeController; 
    private IExtensionProvider extensionProvider;

    private void Start() 
    { 
        InitIAP(); 
        _buyCoinBtn.onClick.AddListener(OnBuyCoinClicked); 
    }


    private void InitIAP() { 
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance()); 
        builder.AddProduct(PRODUCT_COIN_2000, ProductType.Consumable); 
        UnityPurchasing.Initialize(this, builder); 
    }

    private void OnBuyCoinClicked() 
    { 
        if (storeController == null) 
        { 
            Debug.LogWarning("IAP not initialized"); 
            return; 
        } 
        Product product = storeController.products.WithID(PRODUCT_COIN_2000); 
        if (product != null && product.availableToPurchase) 
        { 
            storeController.InitiatePurchase(product); 
        } 
        else 
        { 
            Debug.LogWarning("Product not found or not available"); 
        } 
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) 
    { 
        storeController = controller; 
        extensionProvider = extensions; 
        Debug.Log("IAP Initialized SUCCESS");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
    { 
        if (args.purchasedProduct.definition.id == PRODUCT_COIN_2000) 
        { 
            PurchasingCompleted(); 
        } 
        return PurchaseProcessingResult.Complete; 
    }

    private void PurchasingCompleted() 
    { 
        MoneyService.Instance.PlusTotalCoin(COIN_VALUE); 
        SaveManager.SaveTotalCoin(MoneyService.Instance.GetTotalCoin()); 
        SaveManager.SaveData(); Debug.Log("Purchase completed, coin added!"); 
    }
    public void OnInitializeFailed(InitializationFailureReason error, string message = null) 
    { 
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new System.NotImplementedException();
    }
}


