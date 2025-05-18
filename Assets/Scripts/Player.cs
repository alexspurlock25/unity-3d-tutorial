using System;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float movementSpeed = 7f;
    private bool isWalking = false;
    private Vector3 lastInteractionMoveDirection;

    private void Update() {
        HandleMovement();
        HanleInteractions();
    }

    private void HanleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractionMoveDirection = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionMoveDirection, out RaycastHit raycastHit, interactDistance)) {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                clearCounter.Interact();
            }

        }
    }
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * 2f,
            0.7f,
            moveDir,
            movementSpeed * Time.deltaTime
        );
        if (!canMove) {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * 2f,
                0.7f,
                moveDirX,
                movementSpeed * Time.deltaTime
            );

            if (canMove) {
                moveDir = moveDirX;
            }
            else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * 2f,
                    0.7f,
                    moveDirZ,
                    movementSpeed * Time.deltaTime
                );

                if (canMove) {
                    moveDir = moveDirZ;
                }
                else {

                }
            }
        }
        if (canMove) {
            transform.position += moveDir * movementSpeed * Time.deltaTime;
        }

        isWalking = moveDir != Vector3.zero;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }
    public bool IsWalking() {
        return isWalking;
    }
}
