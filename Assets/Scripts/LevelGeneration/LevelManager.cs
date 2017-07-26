using UnityEngine;

namespace Assets.Scripts.LevelGeneration {

    [System.Serializable]
    /// <summary> The levelManager Class handles the level generation for each level </summary>
    public class LevelManager : MonoBehaviour {
        /// <summary> The Board that is being created </summary>
        private Transform _boardHolder;
        public LevelOptions CurrentLevelOptions;

        /// <summary> The level that will be created </summary>
        public Level CurrentLevel;

        private void BoardSetup() {
            //Instantiate Board and set boardHolder to its transform.
            _boardHolder = new GameObject("Board").transform;
            // create and setup the level
            CurrentLevel = new Level();
            CurrentLevel.Generate(CurrentLevelOptions.MinChunks, CurrentLevelOptions.MaxChunks, CurrentLevelOptions.ChunkSize, CurrentLevelOptions.EnemyWeight, CurrentLevelOptions.LootWeight,CurrentLevelOptions.ObstaclesWeight);
            //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
            for (var x = 0; x < CurrentLevel.XRange.max; x++) {
                //Loop along y axis, starting from -1 to place floor or outerwall tiles.
                for (var y = 0; y < CurrentLevel.YRange.max; y++) {
                    //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                    GameObject toInstantiate = null;
                    GameObject subFlooring = null;

                    if (CurrentLevel.FeatureMap[x, y] == (int) LevelDecoration.Floor) {
                        toInstantiate = CurrentLevelOptions.Floors[Random.Range(0, CurrentLevelOptions.Floors.Length)];
                    }
                    //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                    if (CurrentLevel.FeatureMap[x, y] == (int) LevelDecoration.Wall) {
                        toInstantiate = CurrentLevelOptions.Walls[Random.Range(0, CurrentLevelOptions.Walls.Length)];
                    }
                    //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                    if (CurrentLevel.FeatureMap[x, y] == (int) LevelDecoration.Obstacles) {
                        toInstantiate = CurrentLevelOptions.Walls[Random.Range(0, CurrentLevelOptions.Walls.Length)];
                    }
                    //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                    if (CurrentLevel.FeatureMap[x, y] == (int) LevelDecoration.Enemy) {
                        subFlooring =  CurrentLevelOptions.Floors[Random.Range(0, CurrentLevelOptions.Floors.Length)];
                        toInstantiate = CurrentLevelOptions.Enemies[Random.Range(0, CurrentLevelOptions.Enemies.Length)];
                    }
                    //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                    if (CurrentLevel.FeatureMap[x, y] == (int) LevelDecoration.Loot) {
                        subFlooring =  CurrentLevelOptions.Floors[Random.Range(0, CurrentLevelOptions.Floors.Length)];
                        toInstantiate = CurrentLevelOptions.Loot[Random.Range(0, CurrentLevelOptions.Loot.Length)];
                    }

                    if(subFlooring){
                        //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                        var instance = Instantiate(subFlooring, new Vector2(x, y), Quaternion.identity);
                        //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                        instance.transform.SetParent(_boardHolder);
                    }
                    if (toInstantiate) {
                        //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                        var instance = Instantiate(toInstantiate, new Vector2(x, y), Quaternion.identity);
                        //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                        instance.transform.SetParent(_boardHolder);
                    }
                }
            }
        }

        /// <summary> Entry Point for level creation. Sets up the board </summary>
        public void SetupScene(int level) {
            //Creates the outer walls and floor.
            BoardSetup();
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.SetPositionAndRotation(
                new Vector2(CurrentLevel.FeatureMap.GetLength(0) / 2, CurrentLevel.FeatureMap.GetLength(0) / 2),
                Quaternion.identity);
        }
    }
}