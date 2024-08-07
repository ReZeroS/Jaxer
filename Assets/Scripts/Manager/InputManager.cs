// using UnityEngine;
// using UnityEngine.InputSystem;
//
// // // 使用示例
// // public class PlayerController : MonoBehaviour
// // {
// //     private void Update()
// //     {
// //         if (InputManager.Instance.GetJumpKeyDown())
// //         {
// //             Debug.Log("Jump started!");
// //         }
// //
// //         if (InputManager.Instance.GetJumpKeyUp())
// //         {
// //             Debug.Log("Jump ended!");
// //         }
// //
// //         Vector2 movement = InputManager.Instance.GetMoveInput();
// //         transform.Translate(movement * Time.deltaTime * 5f);
// //     }
// // }
// public class InputManager : MonoBehaviour
// {
//     private static InputManager _instance;
//     public static InputManager Instance
//     {
//         get
//         {
//             if (_instance == null)
//             {
//                 _instance = FindObjectOfType<InputManager>();
//                 if (_instance == null)
//                 {
//                     GameObject go = new GameObject("InputManager");
//                     _instance = go.AddComponent<InputManager>();
//                 }
//             }
//             return _instance;
//         }
//     }
//
//     private PlayerInput playerInput;
//     private InputAction jumpAction;
//     private InputAction moveAction;
//
//     private bool isJumpKeyDown;
//     private bool isJumpKeyUp;
//     private Vector2 moveInput;
//
//     private void Awake()
//     {
//         if (_instance != null && _instance != this)
//         {
//             Destroy(this.gameObject);
//             return;
//         }
//
//         _instance = this;
//         DontDestroyOnLoad(this.gameObject);
//
//         playerInput = GetComponent<PlayerInput>();
//         jumpAction = playerInput.actions["Jump"];
//         moveAction = playerInput.actions["Move"];
//
//         jumpAction.performed += ctx => isJumpKeyDown = true;
//         jumpAction.canceled += ctx => isJumpKeyUp = true;
//         moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
//         moveAction.canceled += ctx => moveInput = Vector2.zero;
//     }
//
//     private void LateUpdate()
//     {
//         // 重置一次性状态
//         isJumpKeyDown = false;
//         isJumpKeyUp = false;
//     }
//
//     public bool GetJumpKeyDown()
//     {
//         return isJumpKeyDown;
//     }
//
//     public bool GetJumpKeyUp()
//     {
//         return isJumpKeyUp;
//     }
//
//     public bool GetJumpKey()
//     {
//         return jumpAction.IsPressed();
//     }
//
//     public Vector2 GetMoveInput()
//     {
//         return moveInput;
//     }
// }
//
