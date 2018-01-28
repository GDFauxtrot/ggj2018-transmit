using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSmartEnemyAI : MonoBehaviour {
    public GameObject player;
    
    // DO NOT LEAVE THIS AS TRUE IF YOU ARE NOT DEBUGGING. ALL OF THE DRAWLINE CALLS
    // WILL TRY TO TURN YOUR COMPUTER INTO A GRIDDLE.
    private bool drawDebugGrid = false;

    private float[,] tilesmap = new float[10, 10];
    private PathFind.Grid grid;
    private List<PathFind.Point> enemyPath;
    private Rigidbody2D enemyRB;

    void Start() {
        enemyRB = gameObject.GetComponent<Rigidbody2D>();
        // This for-loop is meant to set all cells in tilesmap to 1f, meaning
        // that the enemy can walk through it. This is a necessary primer for the 'makeNewTileMap' function.
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                tilesmap[i, j] = 1f;
            }
        }
        grid = new PathFind.Grid(10, 10, tilesmap);
    }

    void FixedUpdate()
    {
        if (drawDebugGrid)
        {
            foreach (PathFind.Node node in grid.nodes)
            {
                Physics2D.BoxCast(new Vector2(node.gridX, node.gridY), new Vector2(.5f, .5f), 0f, Vector2.zero, 0f);
                if (node.walkable == false)
                {
                    Debug.DrawLine(new Vector3(node.gridX - .5f, node.gridY - .5f, 0f), new Vector3(node.gridX - .5f, node.gridY + .5f, 0f), Color.red, 1f);
                    Debug.DrawLine(new Vector3(node.gridX - .5f, node.gridY - .5f, 0f), new Vector3(node.gridX + .5f, node.gridY - .5f, 0f), Color.red, 1f);
                    Debug.DrawLine(new Vector3(node.gridX - .5f, node.gridY + .5f, 0f), new Vector3(node.gridX + .5f, node.gridY + .5f, 0f), Color.red, 1f);
                    Debug.DrawLine(new Vector3(node.gridX + .5f, node.gridY - .5f, 0f), new Vector3(node.gridX + .5f, node.gridY + .5f, 0f), Color.red, 1f);
                }
                else if (node.penalty == 1.0f)
                {
                    Debug.DrawLine(new Vector3(node.gridX - .5f, node.gridY - .5f, 0f), new Vector3(node.gridX - .5f, node.gridY + .5f, 0f), Color.green, 1f);
                    Debug.DrawLine(new Vector3(node.gridX - .5f, node.gridY - .5f, 0f), new Vector3(node.gridX + .5f, node.gridY - .5f, 0f), Color.green, 1f);
                    Debug.DrawLine(new Vector3(node.gridX - .5f, node.gridY + .5f, 0f), new Vector3(node.gridX + .5f, node.gridY + .5f, 0f), Color.green, 1f);
                    Debug.DrawLine(new Vector3(node.gridX + .5f, node.gridY - .5f, 0f), new Vector3(node.gridX + .5f, node.gridY + .5f, 0f), Color.green, 1f);
                }
                else if (node.penalty == 2.0f)
                {
                    Debug.DrawLine(new Vector3(node.gridX - .5f, node.gridY - .5f, 0f), new Vector3(node.gridX - .5f, node.gridY + .5f, 0f), Color.yellow, 1f);
                    Debug.DrawLine(new Vector3(node.gridX - .5f, node.gridY - .5f, 0f), new Vector3(node.gridX + .5f, node.gridY - .5f, 0f), Color.yellow, 1f);
                    Debug.DrawLine(new Vector3(node.gridX - .5f, node.gridY + .5f, 0f), new Vector3(node.gridX + .5f, node.gridY + .5f, 0f), Color.yellow, 1f);
                    Debug.DrawLine(new Vector3(node.gridX + .5f, node.gridY - .5f, 0f), new Vector3(node.gridX + .5f, node.gridY + .5f, 0f), Color.yellow, 1f);
                }

            }
            if (enemyPath != null)
            {
                if (enemyPath.Count != 0)
                {
                    for (int i = 0; i < enemyPath.Count - 1; i++)
                    {
                        Debug.DrawLine(new Vector3(enemyPath[i].x, enemyPath[i].y, 0f), new Vector3(enemyPath[i + 1].x, enemyPath[i + 1].y, 0f), Color.cyan, .1f);
                    }
                }
            }
        }
    }

    void Update() {
        // The X and Y positions of both the player and the enemy are either rounded up or down.
        // These are necessary in making the AI's path to the player's location in the grid as accurate as possible.
        float playerX, playerY, enemyX, enemyY;
        if (player.transform.position.x > (int)player.transform.position.x + .49)
        {
            playerX = Mathf.Ceil(player.transform.position.x);
        }
        else
        {
            playerX = Mathf.Floor(player.transform.position.x);
        }
        if (player.transform.position.y > (int)player.transform.position.y + .49)
        {
            playerY = Mathf.Ceil(player.transform.position.y);
        }
        else
        {
            playerY = Mathf.Floor(player.transform.position.y);
        }
        if (transform.position.y > (int)transform.position.y + .49)
        {
            enemyY = Mathf.Ceil(transform.position.y);
        }
        else
        {
            enemyY = Mathf.Floor(transform.position.y);
        }
        if (transform.position.x > (int)transform.position.x + .49)
        {
            enemyX = Mathf.Ceil(transform.position.x);
        }
        else
        {
            enemyX = Mathf.Floor(transform.position.x);
        }

        // The reason why int casting is done here instead of in the if/else block above
        // is because I had some other code that did a different thing and didn't change this.
        PathFind.Point playerPos = new PathFind.Point((int)playerX, (int)playerY);
        PathFind.Point enemyPos = new PathFind.Point((int)enemyX, (int)enemyY);

        // Have to make a new tilemap, which is a 2D array of floats, where every index corresponds to
        // the penalty an enemy path would incur, should the enemy cross that way.
        makeNewTileMap(10, 10);
        grid = new PathFind.Grid(10, 10, tilesmap);

        // Find the shortest path to the player. If the count isn't 0, move towards player.
        enemyPath = PathFind.Pathfinding.FindPath(grid, enemyPos, playerPos);
        if (enemyPath.Count != 0)
        {
            enemyRB.MovePosition(Vector2.LerpUnclamped(new Vector2(transform.position.x, transform.position.y), new Vector2(enemyPath[0].x, enemyPath[0].y), .08f));
        }
    }

    private void makeNewTileMap(int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                RaycastHit2D hitInfo = Physics2D.BoxCast(new Vector2(i, j), new Vector2(1f, 1f), 0f, Vector2.zero, 0f);
                // This if statement functions by setting any cell that has an object that isn't a wall to some value indicating that it is a "walkable" cell,
                // and any cell that a wall is in as an "unwalkable" cell. On first loop, any cells that are legititmately passable are left that way, and any
                // cell with a wall in it is made unpassable. Any loop after that won't have an unpassable cell become passable.

                // NOTE: IF ANYTHING THAT HAS NO TAG, BUT IS NOT MEANT MAKE A CELL UNPASSABLE MUST HAVE A TAG!!!
                // SECOND NOTE: THIS IS FUCKING FINICKY. SOMETHING MAY BREAK AND IT MAY NOT MAKE SENSE WHY. LIKE EVERYTHING ELSE IN PROGRAMMING.
                if (hitInfo.collider == null || hitInfo.collider.tag == "Player" || hitInfo.collider.tag == "Enemy")
                {
                    if (tilesmap[i, j] != 0f)
                        tilesmap[i, j] = 1f;
                }
                else
                {
                    tilesmap[i, j] = 0f;
                }
            }
        }
    }
}
