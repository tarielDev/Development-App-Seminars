
int target = 199;
int[] arr = Enumerable.Range(1, 100).ToArray();
int cnt = 0;
//Можно изменять количество слагаемых
int count = 3;
var sum = new int[count];

void SumOfThree(int[] arr, int target, int count = 2)
{
    HashSet<int> ints = new HashSet<int>();

    foreach (int i in arr)
    {
        int x = target - i;
        if (ints.Contains(x))
        {
            sum[cnt] = i;
            cnt++;

            if (cnt == count - 1)
            {
                sum[cnt] = x;
                for (var j = 0; j < count; j++)
                {
                    if (j == count - 1)
                    {
                        Console.WriteLine($"{sum[j]}");
                    }
                    else
                    {
                        Console.Write($"{sum[j]} + ");
                    }

                }
                break;
            }
            else
            {
                SumOfThree(arr, x, count);
                break;
            }

        }
        else
        {
            ints.Add(i);
        }
    }

}

SumOfThree(arr, target, count);
