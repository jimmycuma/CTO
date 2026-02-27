using UnityEngine;

namespace CursedDepths.Level
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Level Settings")]
        public RoomGenerator roomGenerator;
        public Transform levelParent;
        public float levelWidth = 100f;
        public float levelHeight = 100f;

        [Header("Camera")]
        public Camera mainCamera;
        public float cameraFollowSpeed = 5f;

        private Transform playerTransform;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitializeLevel();
            FindPlayer();
        }

        private void InitializeLevel()
        {
            if (roomGenerator != null)
            {
                roomGenerator.GenerateLevel();
            }
        }

        private void FindPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        private void Update()
        {
            FollowPlayer();
        }

        private void FollowPlayer()
        {
            if (mainCamera != null && playerTransform != null)
            {
                Vector3 targetPosition = new Vector3(
                    playerTransform.position.x,
                    playerTransform.position.y,
                    mainCamera.transform.position.z
                );

                mainCamera.transform.position = Vector3.Lerp(
                    mainCamera.transform.position,
                    targetPosition,
                    cameraFollowSpeed * Time.deltaTime
                );
            }
        }

        public void RegenerateLevel()
        {
            if (roomGenerator != null)
            {
                roomGenerator.GenerateLevel();
            }
        }

        public void ResetPlayerPosition()
        {
            if (roomGenerator != null && roomGenerator.playerSpawnPoint != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = roomGenerator.playerSpawnPoint.transform.position;
                }
            }
        }
    }
}
