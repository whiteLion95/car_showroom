using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.General
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "IdleBarber/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Player")]
        [SerializeField] private float _playerWalkSpeed;
        [SerializeField] private int _playerInitialMoney;
        [Header("Purchases")]
        [SerializeField] private float _purchaseStartDelay;
        [SerializeField] private float _purchaseDuration;


        public float PlayerWalkSpeed => _playerWalkSpeed;
        public int PlayerInitialMoney => _playerInitialMoney;

        public float PurchaseStartDelay => _purchaseStartDelay;
        public float PurchaseDuration => _purchaseDuration;
    }

}