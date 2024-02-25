// using UnityEngine;
// using Sirenix.OdinInspector;
//
// namespace KoryushkaSyndicate.Common.Components
// {
//     public class Tip : MonoBehaviour
//     {
//         [SerializeField]
//         private GameObjectVariable _compasLookAtObjRef;
//
//         [SerializeField]
//         private BoolVariable _compasIsActiveRef, _inputActive;
//
//         [SerializeField]
//         private FloatReference _cameraShowTime;
//
//         [SerializeField]
//         private GameObject _tipArrow;
//         
//         [SerializeField]
//         private Popup _tipPopup;
//
//         [SerializeField]
//         private MEvent _cameraSettingsEvent;
//
//         [ShowInInspector, ReadOnly]
//         private bool _isShown = false;
//
//         [Button]
//         public void ShowTip(bool withCamera = true, bool withCompass = true)
//         {
//             _isShown = true;
//             
//             _compasLookAtObjRef.Value = gameObject;
//
//             _compasIsActiveRef.Value = withCompass;
//
//             // _tipArrow.SetActive(true);
//             
//             StartCoroutine(HelperMethods.WaitAndDo(0.5f, () =>
//             {
//                 _tipPopup.Show();
//             }));
//
//             if (withCamera)
//             {
//                 //_inputActive.Value = false;
//                 
//                 ChangeCamTarget(transform);
//
//                 StartCoroutine(HelperMethods.WaitAndDo(_cameraShowTime.Value, () =>
//                 {
//                     ChangeCamTarget(null);
//                     
//                     //_inputActive.Value = true;
//                 }));
//             }
//         }
//
//         [Button]
//         public void HideTip()
//         {
//             _isShown = false;
//             
//             _compasIsActiveRef.Value = false;
//             
//             _tipPopup.Hide();
//
//             //_tipArrow.SetActive(false);
//         }
//
//         private void ChangeCamTarget(Transform target)
//         {
//             _cameraSettingsEvent.Invoke(target);
//         }
//
//         public bool IsShown => _isShown;
//
//         public float CameraShowTime => _cameraShowTime.Value;
//     }
// }
//
