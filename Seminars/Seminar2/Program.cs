public interface IBits
{
    bool GetBit(int i);
    void SetBit(bool bit, int index);
}

public class Bits : IBits
{
    public Bits(byte b)
    {
        this.Value = b;
        MaxBitsCount = sizeof(byte) * 8;
    }

    public Bits(short b)
    {
        this.Value = b;
        MaxBitsCount = sizeof(short) * 8;
    }

    public Bits(int b)
    {
        this.Value = b;
        MaxBitsCount = sizeof(int) * 8;
    }

    public Bits(long b)
    {
        this.Value = b;
        MaxBitsCount = sizeof(long) * 8;
    }

    public long Value { get; set; } = 0;
    private int MaxBitsCount { get; set; }

    public bool GetBit(int index)
    {
        if (index > MaxBitsCount || index < 0)
        {
            Console.WriteLine($"Выход за пределы от 0 до {MaxBitsCount}");
            return false;
        }

        return ((Value >> index) & 1) == 1;
    }

    public void SetBit(bool bit, int index)
    {
        if (index > MaxBitsCount || index < 0)
        {
            Console.WriteLine($"Выход за пределы от 0 до {MaxBitsCount}");
            return;
        }
        if (bit == true)
            Value = (byte)(Value | (1 << index));
        else
        {
            var mask = (byte)(1 << index);
            mask = (byte)(0xff ^ mask);
            Value &= (byte)(Value & mask);
        }
    }

    public static implicit operator Bits(long value) => new Bits(value);

    public static implicit operator Bits(int value) => new Bits(value);

    public static implicit operator Bits(byte value) => new Bits(value);

}

class Program
{
    static void Main()
    {
        Bits bitsFromLong = 123456789L; // Неявное преобразование из long
        Bits bitsFromInt = 42;          // Неявное преобразование из int
        Bits bitsFromByte = (byte)7;    // Неявное преобразование из byte

        Console.WriteLine("Bits from long: " + bitsFromLong.Value);
        Console.WriteLine("Bits from int: " + bitsFromInt.Value);
        Console.WriteLine("Bits from byte: " + bitsFromByte.Value);


    }
}
