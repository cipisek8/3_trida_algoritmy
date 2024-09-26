using System.Drawing;

class Program
{
    static readonly Point[] availableDirections = new Point[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };

    //Size of matrix
    const int sizeX = 41, sizeY = 41;

    static void Main(string[] args)
    {
        bool[,] matrix = new bool[sizeX, sizeY];
        bool[,] searched = new bool[sizeX, sizeY];

        //Generating maze
        new Program().FillMatrixWithMaze(new Point(0, 0), matrix);

        //Solving maze
        List<Point> result = new Program().FindPathRecursively(new Point(0, 0), matrix, searched);

        //Marking correct cells
        bool[,] correct = new bool[sizeX, sizeY];
        if (result != null)
            for (int i = result.Count - 1; i >= 0; i--)
            {
                correct[result[i].X, result[i].Y] = true;
            }

        //Print matrix
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (correct[x, y])
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(matrix[x, y] ? "██" : "  ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
        }

        //For text result
        /*
        if (result == null)
            Console.WriteLine("Path does not exist!");
        else
        {
            Console.WriteLine("Path:");
            for (int i = result.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(result[i].X + ", " + result[i].Y);
            }
        }*/
    }

    //Generates a maze uzing randomized BFS
    void FillMatrixWithMaze(Point startPosition, bool[,] matrix)
    {
        Stack<Point> stack = new Stack<Point>();
        stack.Push(startPosition);
        matrix[startPosition.X, startPosition.Y] = true;
        Random rnd = new Random();

        while (stack.Count > 0)
        {
            Point currentPoint = stack.Pop();

            List<Point> shuffledAvailableDirections = new List<Point>(availableDirections);
            ShuffleList(shuffledAvailableDirections, rnd);

            //Trying random direction
            foreach (Point direction in shuffledAvailableDirections)
            {
                Point newPosition = new Point(currentPoint.X + direction.X * 2, currentPoint.Y + direction.Y * 2);
                if (newPosition.X < 0 || newPosition.Y < 0 || newPosition.X > sizeX - 1 ||
                    newPosition.Y > sizeY - 1 || matrix[newPosition.X, newPosition.Y] == true)
                    continue;

                matrix[newPosition.X, newPosition.Y] = true;
                matrix[newPosition.X - direction.X, newPosition.Y - direction.Y] = true;
                stack.Push(currentPoint);
                stack.Push(newPosition);
                break;
            }
        }
    }

    //Shuffles the list
    public void ShuffleList<T>(IList<T> list, Random rnd)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    //Find the path using DFS
    List<Point> FindPathRecursively(Point currentPoint, bool[,] matrix, bool[,] searched)
    {
        //Check if I am already in finish
        if (currentPoint.X == sizeX - 1 && currentPoint.Y == sizeY - 1)
            return new List<Point>() { currentPoint };
        else
        {
            //Try all directions
            for (int i = 0; i < availableDirections.Length; i++)
            {
                //Check if this direction is not out of matrix, it is not already searched and it is possible to go there
                Point newPoint = new Point(currentPoint.X + availableDirections[i].X, currentPoint.Y + availableDirections[i].Y);
                if (newPoint.X >= 0 && newPoint.Y >= 0 && newPoint.X < sizeX && newPoint.Y < sizeY &&
                    !searched[newPoint.X, newPoint.Y] && matrix[newPoint.X, newPoint.Y])
                {
                    //Mark as searched
                    searched[newPoint.X, newPoint.Y] = true;
                    List<Point> path = FindPathRecursively(newPoint, matrix, searched);
                    if (path != null)
                    {
                        path.Add(currentPoint);
                        return path;
                    }
                }
            }
            return null;
        }
    }
}