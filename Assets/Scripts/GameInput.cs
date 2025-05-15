using UnityEngine;

public class GameInput : MonoBehaviour {

    private PlayerInputActions playerActions;

    private void Awake() {
        playerActions = new PlayerInputActions();
        playerActions.Player.Enable();
    }
    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }
}
