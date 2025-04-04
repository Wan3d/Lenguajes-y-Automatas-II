using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

static void Main(string[] args)
{
    float x, i, j;
    float res;
    i = 0;
    // x = Console.ReadLine();
    x = Console.Read();
    int cont;
    int altura = 10;
    i = 0;

    for (i = 0; i <= x; i++)
    {
        Console.WriteLine("Nooo");
    }
    i = 1;
    while (i <= x)
    {
        j = 1;
        while (j <= i)
        {
            if (j % 2 == 0)
                Console.Write("*");
            else
                Console.Write("-");
            j++;
        }
        i++;
        Console.WriteLine();
    }
    /*Console.WriteLine("Altura = " + altura);
    i = 0;
    while (i < x)
    {
        Console.Write(i + " ");
        i++;
    }
    Console.WriteLine();*/
}

/*char n;
int p;


float altura, i, j;
float x = 0, y = 10, z = 2;
float c;

// Error, no se puede asignar un Int a un Char
// c = (100+200)
c = (char)(100 + 200); // 44

Console.Write("Valor de altura = ");
altura = Console.ReadLine();

x += sqrt(abs(0 - altura)); // 100
Console.WriteLine("x = " + x);

// Console.WriteLine("Valor de altura = " + altura);

// Error, no se puede asignar un Int a un Char
// x = ((3 + altura) * 8 - (10 - 4) / 2); // = 61

x = (char)(((3 + altura) * 8 - (10 - 4) / 2)); // Para altura = 5, x = 61
x--;                                           // 60
x += sqrt(abs(0 - altura));                    // 100
// Console.WriteLine("x = " + x);
x *= 2;       // 200
x /= (y - 6); // 50
x = x + 5;    // 55

x = 0;

do
{
    y = 0;
    do
    {
        if (y % 2 == 0)
        {
            Console.Write("*");
        }
        else
        {
            Console.Write("-");
        }
        y++;
    } while (y <= x);
    Console.WriteLine();
    x++;
} while (x < altura);
*/
/*do {
    y = 0;
    do {
        //Console.Write("*");
        y++;
    }while(y <= x);
    Console.WriteLine();
    x++;
} while(x < altura);*/

/*for (i = 1; i<=altura; i++)
{
    for (j = 1; j<=i; j++)
    {
        if (j%2==0)
            Console.Write("*");
        else
            Console.Write("-");
    }
    Console.WriteLine("");
}

i = 0;

do
{
    Console.Write("-");
    i++;
}
while (i<altura*2);
Console.WriteLine("");
for (i = 1; i<=altura; i++)
{
    j = 1;
    while (j<=i)
    {
        Console.Write(""+j);
        j++;
    }
    Console.WriteLine("");
}
i = 0;
do
{
    Console.Write("-");
    i++;
}
while (i<altura*2);
Console.WriteLine("");*/
