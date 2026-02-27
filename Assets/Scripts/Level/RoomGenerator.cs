using UnityEngine;
using System.Collections.Generic;

namespace CursedDepths.Level
{
    public class RoomGenerator : MonoBehaviour
    {
        [Header("Room Settings")]
        public int minRooms = 5;
        public int maxRooms = 10;
        public float roomSize = 10f;
        public int gridWidth = 20;
        public int gridHeight = 20;

        [Header("Prefabs")]
        public GameObject playerSpawnPoint;
        public GameObject[] enemyPrefabs;
        public GameObject[] pickupPrefabs;
        public GameObject[] wallPrefabs;
        public GameObject[] floorPrefabs;

        private List<GameObject> spawnedEnemies = new List<GameObject>();
        private bool[,] roomGrid;

        public void GenerateLevel()
        {
            ClearLevel();
            InitializeGrid();
            GenerateRooms();
            SpawnPlayer();
        }

        private void ClearLevel()
        {
            foreach (GameObject enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
            
            spawnedEnemies.Clear();
        }

        private void InitializeGrid()
        {
            roomGrid = new bool[gridWidth, gridHeight];
        }

        private void GenerateRooms()
        {
            int roomCount = Random.Range(minRooms, maxRooms + 1);
            
            for (int i = 0; i < roomCount; i++)
            {
                int x = Random.Range(1, gridWidth - 1);
                int y = Random.Range(1, gridHeight - 1);
                
                if (!roomGrid[x, y])
                {
                    roomGrid[x, y] = true;
                    CreateRoom(x, y);
                }
            }
        }

        private void CreateRoom(int gridX, int gridY)
        {
            Vector2 roomPosition = new Vector2(gridX * roomSize, gridY * roomSize);
            
            CreateFloor(roomPosition);
            CreateWalls(roomPosition);
            
            if (Random.value < 0.7f)
            {
                SpawnEnemy(roomPosition);
            }

            if (Random.value < 0.3f)
            {
                SpawnPickup(roomPosition);
            }
        }

        private void CreateFloor(Vector2 position)
        {
            if (floorPrefabs != null && floorPrefabs.Length > 0)
            {
                GameObject floor = Instantiate(floorPrefabs[Random.Range(0, floorPrefabs.Length)]);
                floor.transform.position = position;
            }
        }

        private void CreateWalls(Vector2 position)
        {
            if (wallPrefabs != null && wallPrefabs.Length > 0)
            {
                Vector2[] wallOffsets = new Vector2[]
                {
                    new Vector2(-roomSize/2, 0),
                    new Vector2(roomSize/2, 0),
                    new Vector2(0, -roomSize/2),
                    new Vector2(0, roomSize/2)
                };

                foreach (Vector2 offset in wallOffsets)
                {
                    GameObject wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]);
                    wall.transform.position = position + offset;
                    wall.layer = LayerMask.NameToLayer("Wall");
                }
            }
        }

        private void SpawnEnemy(Vector2 roomPosition)
        {
            if (enemyPrefabs != null && enemyPrefabs.Length > 0)
            {
                GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
                enemy.transform.position = roomPosition + Random.insideUnitCircle * (roomSize / 3);
                spawnedEnemies.Add(enemy);
            }
        }

        private void SpawnPickup(Vector2 roomPosition)
        {
            if (pickupPrefabs != null && pickupPrefabs.Length > 0)
            {
                GameObject pickup = Instantiate(pickupPrefabs[Random.Range(0, pickupPrefabs.Length)]);
                pickup.transform.position = roomPosition + Random.insideUnitCircle * (roomSize / 3);
            }
        }

        private void SpawnPlayer()
        {
            if (playerSpawnPoint != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = playerSpawnPoint.transform.position;
                }
            }
        }

        public int GetRoomCount()
        {
            int count = 0;
            foreach (bool room in roomGrid)
            {
                if (room) count++;
            }
            return count;
        }
    }
}
