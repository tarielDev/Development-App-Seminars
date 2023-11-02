using System.Collections;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var pathFinder = new Pathfinder();
pathFinder.Start();

public class Pathfinder
{

    static int[,] labirynth = new int[,]
    {
        { 1, 1, 1, 0, 1, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0 },
        { 1, 0, 1, 1, 1, 0, 1 },
        { 0, 0, 0, 0, 1, 0, 2 },
        { 1, 1, 0, 0, 1, 1, 1 },
        { 1, 1, 1, 1, 1, 1, 1 },
        { 1, 0, 1, 1, 1, 1, 1 },

    };

    enum Direction
    {
        Forward = 0,
        Backward = 1,
        Left = 2,
        Right = 3
    }
    enum Place
    {
        HorizontalUp = 0,
        HorizontalDown = 1,
        VerticalLeft = 2,
        VerticalRight = 3,
    }

    Stack tracker = new Stack();
    int[] stack_container = new int[3];
    int[] old_stack_container = new int[3];
    Dictionary<Enum, int> array_direction = new Dictionary<Enum, int>();
    public void Start()
    {
        Enum y; int x; int coordX; int coordY;
        (int, Enum, int, int)[] z;
        int[] num1 = Enumerable.Range(1, labirynth.GetUpperBound(0) - 1).ToArray();
        int[] num2 = new int[] { 0, labirynth.GetUpperBound(0) };
        z = Entrances(num1, num2);
        int cnt = 0;

        for (int i = 0; i < z.Length; i++)
        {
            if (!(z[i].Item3 == 0 && z[i].Item4 == 0))
            {
                cnt++;
            }
        }

        Dictionary<int, (int, int)> dct = new Dictionary<int, (int, int)>();
        (int, int) tuple;
        for (int i = 0; i < z.Length; i++)
        {
            if (!(z[i].Item3 == 0 && z[i].Item4 == 0))
            {
                tuple = (z[i].Item3, z[i].Item4);
                dct.Add(i, tuple);
            }
        }

        bool val = HasExit(z, dct);
    }
    public (int, Enum, int, int)[] Entrances(int[] num1, int[] num2)
    {
        (int, Enum, int, int)[] directionHV = new (int, Enum, int, int)[28];
        (int, Enum, int, int) dir = new();

        int cnt = 0;
        // поиск входов с горизонтальных сторон
        Console.WriteLine("Входы с горизонтальных сторон:");

        foreach (var i in num2)
        {
            foreach (int j in num1)
            {
                if (labirynth[i, j] == 0)
                {
                    if (i == num2[0])
                    {
                        dir = (labirynth[i, j], Place.HorizontalUp, i, j);
                        directionHV[cnt] = dir;
                        cnt++;
                    }
                    else
                    {
                        dir = new(labirynth[i, j], Place.HorizontalDown, i, j);
                        directionHV[cnt] = dir;
                        cnt++;
                    }

                    Console.WriteLine($"labirinth[{i},{j}] = 0");
                }

            }
        }
        Console.WriteLine("");

        // поиск входов с вертикальных сторон
        Console.WriteLine("Входы с вертикальных сторон:");

        foreach (var i in num1)
        {
            foreach (int j in num2)
            {
                if (labirynth[i, j] == 0)
                {
                    if (i == num1[0])
                    {
                        dir = (labirynth[i, j], Place.VerticalLeft, i, j);
                        directionHV[cnt] = dir;
                        cnt++;
                    }
                    else
                    {
                        dir = new(labirynth[i, j], Place.VerticalRight, i, j);
                        directionHV[cnt] = dir;
                        cnt++;
                    }
                    Console.WriteLine($"labirinth[{i},{j}] = 0");
                }

            }
        }
        Console.WriteLine("");


        return directionHV;

    }

    public Enum ChangeDirection(Dictionary<Enum, int> arr_direction, Enum backdirection)
    {

        var new_direction = new Direction();
        foreach (var item in arr_direction)
        {

            if (item.Value == 0 && !item.Key.Equals(backdirection) && !item.Key.Equals(Direction.Backward))
            {
                new_direction = (Direction)item.Key;

            }

        }
        array_direction[new_direction] = 1;

        return new_direction;
    }

    public bool IsEntrance(Dictionary<int, (int, int)> dct, int coordX, int coordY)
    {
        bool yes = false;

        foreach (var item in dct.Values)
        {
            if (!(item.Item1 == coordX && item.Item2 == coordY)) { yes = true; }

            else { yes = false; }
        }
        return yes;
    }

    public bool HasExit((int, Enum, int, int)[] dir, Dictionary<int, (int, int)> dct)
    {
        Enum plc; int val; int coordX; int coordY; Enum direction, backdirection, old_direction;
        bool yes = false;
        int[] nextStep = new int[3];

        (val, plc, coordX, coordY) = dir[0];
        array_direction.Add(Direction.Forward, 0);
        array_direction.Add(Direction.Backward, 0);
        array_direction.Add(Direction.Left, 0);
        array_direction.Add(Direction.Right, 0);

        old_stack_container[0] = coordX;
        old_stack_container[1] = coordY;
        old_stack_container[2] = val;

        direction = Direction.Forward;
        array_direction[direction] = 1;

        tracker.Push(Convert.ToInt32((Enum)direction));
        tracker.Push(coordY);
        tracker.Push(coordX);


        //nextStep = TestAndGoDirection(direction, plc, coordX, coordY);

        while (true)
        {
            yes = IsEntrance(dct, coordX, coordY);

            if (yes)
            {
                nextStep = TestAndGoDirection(direction, plc, coordX, coordY);
                if (nextStep[2] == 1)
                {
                    tracker.Push(Convert.ToInt32((Enum)direction));
                    tracker.Push(coordY);
                    tracker.Push(coordX);
                }
            }
            else
            {
                Console.WriteLine("Проход есть, но это вход в другом месте!");
                Console.WriteLine("Begining\n");
                Enum direction1 = direction; //Right

                coordX = (int)tracker.Pop();
                coordY = (int)tracker.Pop();
                old_direction = (Direction)tracker.Pop();
                array_direction[old_direction] = 1;
                direction = ChangeDirection(array_direction, old_direction);
                array_direction[old_direction] = 0;


                nextStep = TestAndGoDirection(direction, plc, coordX, coordY);
                if (nextStep[2] == 1)
                {
                    tracker.Push(Convert.ToInt32((Enum)direction));
                    tracker.Push(coordY);
                    tracker.Push(coordX);
                }

                //break;
            }

            labirynth[coordX, coordY] = 8;

            switch (nextStep[2])
            {
                case 2:
                    // Выход найден
                    Console.WriteLine("Выход найден");
                    labirynth[coordX, coordY] = 8;
                    //labirynth[coordX, coordY] = 8;
                    showLabirinth();

                    return true;

                case 1:
                    // Это стена, надо вернуться назад и попробовать другое направление
                    Console.WriteLine("Впереди стена");
                    labirynth[coordX, coordY] = 8;
                    showLabirinth();

                    if (old_stack_container[0] == coordX && old_stack_container[1] == coordY)
                    {
                        // Замуровать этот вход
                        Console.WriteLine("Вход замурован");
                        labirynth[coordX, coordY] = 4;
                        showLabirinth();

                        return false;

                    }
                    else
                    {
                        ;
                        Console.WriteLine("Пробую другое направление!\n");
                        //Пробую другое направление
                        backdirection = direction;
                        old_direction = direction;

                        switch (direction)
                        {
                            case Direction.Forward:
                                backdirection = Direction.Backward; break;

                            case Direction.Left:
                                backdirection = Direction.Right; break;

                            case Direction.Right:
                                backdirection = Direction.Left; break;

                        }

                        yes = IsEntrance(dct, coordX, coordY);

                        if (yes)
                        {
                            nextStep = TestAndGoDirection(backdirection, plc, coordX, coordY);
                            if (nextStep[2] == 1)
                            {
                                tracker.Push(Convert.ToInt32((Enum)direction));
                                tracker.Push(coordY);
                                tracker.Push(coordX);
                            }

                            direction = ChangeDirection(array_direction, backdirection);
                            array_direction[old_direction] = 0;
                        }
                        else
                        {
                            Console.WriteLine("Проход есть, но это вход в другом месте!");
                            Console.WriteLine("1\n");
                            break;
                        }

                        break;
                    }

                case 0:

                    ;

                    yes = IsEntrance(dct, coordX, coordY);

                    if (yes)
                    {
                        Console.WriteLine("Проход есть");
                        coordX = nextStep[0];
                        coordY = nextStep[1];
                        showLabirinth();

                    }
                    else
                    {
                        Console.WriteLine("Проход есть, но это вход в другом месте!");
                        Console.WriteLine("0");
                        break;
                    }

                    break;

                case 8:
                    // Тут мы уже были
                    Console.WriteLine("Тут мы уже были");
                    backdirection = direction;
                    old_direction = direction;

                    switch (direction)
                    {
                        case Direction.Forward:
                            backdirection = Direction.Backward; break;

                        case Direction.Left:
                            backdirection = Direction.Right; break;

                        case Direction.Right:
                            backdirection = Direction.Left; break;

                    }

                    yes = IsEntrance(dct, coordX, coordY);

                    if (yes)
                    {
                        nextStep = TestAndGoDirection(backdirection, plc, coordX, coordY);
                        if (nextStep[2] == 1)
                        {
                            tracker.Push(Convert.ToInt32((Enum)backdirection));
                            tracker.Push(coordY);
                            tracker.Push(coordX);
                        }

                        direction = ChangeDirection(array_direction, backdirection);
                        array_direction[old_direction] = 0;
                    }
                    else
                    {
                        Console.WriteLine("Проход есть, но это вход в другом месте!");
                        Console.WriteLine("8");
                        break;
                    }

                    break;

                case 4:
                    // Тут мы уже были там тупик
                    Console.WriteLine("Тут мы уже были");
                    showLabirinth();
                    break;

            }

        }

        return true;
    }

    public int[] TestAndGoDirection(Enum direction, Enum place, int coordX, int coordY)
    {
        int ret_value = 0;
        switch (place)
        {
            case Place.HorizontalUp:

                switch (direction)
                {
                    case Direction.Forward:
                        ret_value = labirynth[coordX + 1, coordY];
                        stack_container[0] = coordX + 1;
                        stack_container[1] = coordY;
                        break;

                    case Direction.Backward:
                        ret_value = labirynth[coordX - 1, coordY];
                        stack_container[0] = coordX - 1;
                        stack_container[1] = coordY;
                        break;

                    case Direction.Left:
                        ret_value = labirynth[coordX, coordY + 1];
                        stack_container[0] = coordX;
                        stack_container[1] = coordY + 1;
                        break;

                    case Direction.Right:
                        ret_value = labirynth[coordX, coordY - 1];
                        stack_container[0] = coordX;
                        stack_container[1] = coordY - 1;
                        break;

                }

                break;

            case Place.HorizontalDown:

                switch (direction)
                {
                    case Direction.Forward:
                        ret_value = labirynth[coordX - 1, coordY];
                        stack_container[0] = coordX - 1;
                        stack_container[1] = coordY;
                        break;

                    case Direction.Backward:
                        ret_value = labirynth[coordX + 1, coordY];
                        stack_container[0] = coordX + 1;
                        stack_container[1] = coordY;
                        break;

                    case Direction.Left:
                        ret_value = labirynth[coordX, coordY - 1];
                        stack_container[0] = coordX;
                        stack_container[1] = coordY - 1;
                        break;

                    case Direction.Right:
                        ret_value = labirynth[coordX, coordY + 1];
                        stack_container[0] = coordX;
                        stack_container[1] = coordY + 1;
                        break;

                }
                break;

            case Place.VerticalLeft:

                switch (direction)
                {
                    case Direction.Forward:
                        ret_value = labirynth[coordX, coordY + 1];
                        stack_container[0] = coordX;
                        stack_container[1] = coordY + 1;
                        break;

                    case Direction.Backward:
                        ret_value = labirynth[coordX, coordY - 1];
                        stack_container[0] = coordX;
                        stack_container[1] = coordY - 1;
                        break;

                    case Direction.Left:
                        ret_value = labirynth[coordX - 1, coordY];
                        stack_container[0] = coordX - 1;
                        stack_container[1] = coordY;
                        break;

                    case Direction.Right:
                        ret_value = labirynth[coordX + 1, coordY];
                        stack_container[0] = coordX + 1;
                        stack_container[1] = coordY;
                        break;

                }

                break;

            case Place.VerticalRight:

                switch (direction)
                {
                    case Direction.Forward:
                        ret_value = labirynth[coordX, coordY - 1];
                        stack_container[0] = coordX;
                        stack_container[1] = coordY - 1;
                        break;

                    case Direction.Backward:
                        ret_value = labirynth[coordX, coordY + 1];
                        stack_container[0] = coordX;
                        stack_container[1] = coordY + 1;
                        break;

                    case Direction.Left:
                        ret_value = labirynth[coordX + 1, coordY];
                        stack_container[0] = coordX + 1;
                        stack_container[1] = coordY;
                        break;

                    case Direction.Right:
                        ret_value = labirynth[coordX - 1, coordY];
                        stack_container[0] = coordX - 1;
                        stack_container[1] = coordY;
                        break;

                }
                break;

        }

        stack_container[2] = ret_value;
        return stack_container;
    }

    public void showLabirinth()
    {
        int ordinate = labirynth.GetUpperBound(0) + 1;
        int abscissa = labirynth.GetUpperBound(1) + 1;
        Console.WriteLine();
        for (int i = 0; i < ordinate; i++)
        {
            for (int j = 0; j < abscissa; j++)
            {
                Console.Write($"{labirynth[i, j]} ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.ReadLine();
    }
}