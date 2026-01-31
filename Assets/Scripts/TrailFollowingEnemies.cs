using UnityEngine;

public class TrailFollowingEnemies : MonoBehaviour {

    public Transform enemy;

    private Vector3 _lastEnemyPosition;

    private void Update() {
        transform.forward = -(enemy.position - _lastEnemyPosition);

        _lastEnemyPosition = enemy.position;
    }

}
