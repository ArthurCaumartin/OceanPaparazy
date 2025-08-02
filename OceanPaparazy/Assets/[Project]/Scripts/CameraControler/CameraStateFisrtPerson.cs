// using System;
// using UnityEngine;

// [Serializable]
// public class CameraStateFisrtPerson : PlayerControlable
// {
//     [SerializeField] private float _speed = 5;
//     private Vector3 _lookRotation;


//     public override void EnterControler()
//     {
//         throw new NotImplementedException();
//     }

//     public override void UpdateControler(Vector2 inputDirection, Vector2 lookDelta)
//     {
//         transform.position =
//         Vector3.Lerp(transform.position, cameraControler.Target.position, Time.deltaTime * 10);
//         LookAround(lookDelta);
//     }

//     public override void AbilityFisrt()
//     {
//         throw new NotImplementedException();
//     }

//     public override void AbilitySecond()
//     {
//         throw new NotImplementedException();
//     }

//     public override void ExitControler()
//     {
//         throw new NotImplementedException();
//     }


//     private void LookAround(Vector2 inputDiirection)
//     {
//         _lookRotation.x += -inputDiirection.y * _speed;
//         _lookRotation.x = Mathf.Clamp(_lookRotation.x, -90, 90);
//         _lookRotation.y += inputDiirection.x * _speed;

//         transform.rotation =
//         Quaternion.Slerp(transform.rotation, Quaternion.Euler(_lookRotation), Time.deltaTime * _speed * 10);
//     }
// }
