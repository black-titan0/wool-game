using UnityEngine;
using UnityEngine.Serialization;

public class MoveUntilCrossing : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private GameObject player;
    [SerializeField] private LevelGenerator levelGenerator;

    private bool[,] _horizontalWalls, _verticalWalls;

    [SerializeField]
    private bool lockInput = false;

    private string _cantMoveTo;

    private int _currentX, _currentY;
    [SerializeField] private int goalX, goalY;
    private Vector2 _initialActualPosition;
    [SerializeField] private float epsilon = 0.001f;

    void Start()
    {
        _currentX = levelGenerator.initialPlayerX;
        _currentY = levelGenerator.initialPlayerY;
        goalX = _currentX;
        goalY = _currentY;
        _horizontalWalls = levelGenerator.horizantal;
        _verticalWalls = levelGenerator.vertical;
        var position = player.transform.position;
        position.x += _currentX;
        position.y += _currentY;
        _initialActualPosition = position;
        player.transform.position = position;
    }

    void Update()
    {
        
        GetDirectionInput();
        if (ShouldMove())
            MovePlayer();
    }

    private void CheckForNextPotentialMove()
    {
        if (!lockInput)
            return;
        int possibleMoves = 0;
        if (_cantMoveTo != "right" && !_horizontalWalls[_currentX + 1, _currentY])
            possibleMoves++;
        if (_cantMoveTo != "left" && !_horizontalWalls[_currentX, _currentY])
            possibleMoves++;
        if (_cantMoveTo != "up" && !_verticalWalls[_currentX, _currentY + 1])
            possibleMoves++;
        if (_cantMoveTo != "down" && !_verticalWalls[_currentX, _currentY])
            possibleMoves++;

        if (possibleMoves == 1)
        {
            if (_cantMoveTo != "right" && !_horizontalWalls[_currentX + 1, _currentY])
                goalX = GetNextPosition("right");
            else if (_cantMoveTo != "left" && !_horizontalWalls[_currentX, _currentY])
                goalX = GetNextPosition("left");
            else if (_cantMoveTo != "up" && !_verticalWalls[_currentX, _currentY + 1])
                goalY = GetNextPosition("up");
            else if (_cantMoveTo != "down" && !_verticalWalls[_currentX, _currentY])
                goalY = GetNextPosition("down");
            lockInput = true;
        }
    }

    public void GetDirectionInput(string direction = "None")
    {
        for (int i =0; i < 4; i++)
            player.transform.GetChild(i).gameObject.SetActive(false);
        if (lockInput)
            return;
        if (!_horizontalWalls[_currentX + 1, _currentY])
            player.transform.GetChild(0).gameObject.SetActive(true);
        if (!_horizontalWalls[_currentX, _currentY])
            player.transform.GetChild(1).gameObject.SetActive(true);
        if (!_verticalWalls[_currentX, _currentY + 1])
            player.transform.GetChild(2).gameObject.SetActive(true);
        if (!_verticalWalls[_currentX, _currentY])
            player.transform.GetChild(3).gameObject.SetActive(true);
        
        if (direction == "left" || Input.GetKeyDown(KeyCode.LeftArrow))
            goalX = GetNextPosition("left");

        if (direction == "right" || Input.GetKeyDown(KeyCode.RightArrow))
            goalX = GetNextPosition("right");

        if (direction == "up" || Input.GetKeyDown(KeyCode.UpArrow))
            goalY = GetNextPosition("up");

        if (direction == "down" || Input.GetKeyDown(KeyCode.DownArrow))
            goalY = GetNextPosition("down");
    }

    private int GetNextPosition(string direction)
    {
        int virtualX = _currentX, virtualY = _currentY;
        bool firstMove = true;
        if (direction == "left")
        {
            while (!_horizontalWalls[virtualX, _currentY] && (firstMove ||
                                                              _verticalWalls[virtualX, _currentY + 1] &&
                                                              _verticalWalls[virtualX, _currentY]))
            {
                _cantMoveTo = "right";
                virtualX--;
                firstMove = false;
            }

            return virtualX;
        }

        if (direction == "right")
        {
            while (!_horizontalWalls[virtualX + 1, _currentY] && (firstMove ||
                                                                  _verticalWalls[virtualX, _currentY + 1] &&
                                                                  _verticalWalls[virtualX, _currentY]))
            {
                _cantMoveTo = "left";
                virtualX++;
                firstMove = false;
            }

            return virtualX;
        }

        if (direction == "up")
        {
            _cantMoveTo = "down";
            while (!_verticalWalls[_currentX, virtualY + 1] && (firstMove ||
                                                                _horizontalWalls[_currentX, virtualY] &&
                                                                _horizontalWalls[_currentX + 1, virtualY]))
            {
                _cantMoveTo = "down";
                virtualY++;
                firstMove = false;
            }

            return virtualY;
        }

        if (direction == "down")
        {
            while (!_verticalWalls[_currentX, virtualY] && (firstMove ||
                                                            _horizontalWalls[_currentX, virtualY] &&
                                                            _horizontalWalls[_currentX + 1, virtualY]))
            {
                _cantMoveTo = "up";
                virtualY--;
                firstMove = false;
            }

            return virtualY;
        }

        return 0;
    }

    private bool ShouldMove()
    {
        var position = player.transform.position;
        bool condition =
            Mathf.Abs(position.x - (_initialActualPosition.x + goalX)) > speed * epsilon ||
            Mathf.Abs(position.y - (_initialActualPosition.x + goalY)) > speed * epsilon;
        if (!condition)
            player.transform.position = new Vector2(goalX, goalY);
        else
        {
            _currentX = goalX;
            _currentY = goalY;
        }
        
        if  (!condition)
            CheckForNextPotentialMove();
        condition = condition || _currentX != goalX || _currentY != goalY;
        lockInput = condition;
        return condition;
    }

    private void MovePlayer()
    {
        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = new Vector3(goalX, goalY, 0);
        player.transform.position = Vector3.Lerp(startPosition, endPosition, speed * Time.deltaTime);
    }
}