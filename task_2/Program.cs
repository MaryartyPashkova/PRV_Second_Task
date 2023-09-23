using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Threading;
static int[,] NewMas() {
    Console.Write("Введите количество строк: ");
    int a1 = Convert.ToInt32(Console.ReadLine());
    Console.Write("Введите количество столбцов: ");
    int b1 = Convert.ToInt32(Console.ReadLine());
    int[,] mas =new int[a1,b1];
    //Console.WriteLine();
    Random rand = new Random();
    Console.WriteLine("Сгенерированная матрица {0} на {1}:", a1, b1);
    for (int i = 0; i < mas.GetLength(0); i++)
    {
        for (int j = 0; j < mas.GetLength(1); j++)
        {
            mas[i, j] =  rand.Next(10);
        }
    }
        return mas;
}



//delegate void RenderArray(ListBox listbox, int[] arr, ListBox listBox_2, string time);
Console.WriteLine("Заполните данные для матрицы А");
int[,] a = NewMas();
//console_log(a);
Console.WriteLine();
Console.WriteLine("Заполните данные для матрицы В");
int[,] b = NewMas();
//console_log(b);

Stopwatch stopWatch = new Stopwatch();

int[,] c = Matrix_Task(a, b, stopWatch);

Console.WriteLine("Result:");
//console_log(c);
Console.WriteLine();


//int[,] d = Matrix_Thread(a, b, stopWatch1);
//Console.Write("Result:");
//console_log(d);



Stopwatch stopWatch1 = new Stopwatch();
int[,] d = Matrix_Thread(a, b, stopWatch1);
//console_log(d);
Console.WriteLine();
Console.ReadLine();

static void console_log(int[,] mas)
{

    for (int i = 0; i < mas.GetLength(0); i++)
    {
        for (int j = 0; j < mas.GetLength(1); j++)
        {
            Console.Write("{0} ", mas[i, j]);
        }
        Console.WriteLine();
    }
}

static int[,] Matrix_Thread(int[,] a, int[,] b, Stopwatch stopWatch)
{
    Console.WriteLine();
    stopWatch.Start();
    int i1;
    int j1;
    int[,] new_matrix2 = new int[a.GetLength(0), b.GetLength(1)];
    static void prod(int i, int j, int[,] a, int[,] b, int[,] new_matrix2)
    {
        for (int k = 0; k < b.GetLength(0); k++)
        {
            new_matrix2[i, j] += a[i, k] * b[k, j];
        }
       // Console.WriteLine("Потоки:" + new_matrix2[i, j]);
    }

    for (int i = 0; i < a.GetLength(0); i++)
    {
        for (int j = 0; j < b.GetLength(1); j++)
        {
            i1 = i;
            j1 = j;
            Thread t = new Thread(() => prod(i1, j1, a, b, new_matrix2));
            t.Start();
            t.Join();
        }
    }
    string s = Convert.ToString(stopWatch.ElapsedMilliseconds);
    Console.WriteLine();
    Console.Write("Time Thread: ");
    Console.WriteLine(s);
    stopWatch.Stop();
    return new_matrix2;
}

static int[,]  Matrix_Task(int[,] a, int[,] b, Stopwatch stopWatch) {
    Console.WriteLine();
    int i1;
    int j1;
    List<Task> t = new List<Task>();
    stopWatch.Start();
    if (a.GetLength(1) != b.GetLength(0)) throw new Exception("ERROR!");  
    int[,] new_matrix = new int[a.GetLength(0), b.GetLength(1)];
    for (int i = 0; i < a.GetLength(0); i++) {
        for (int j = 0; j < b.GetLength(1); j++) {
            i1 = i;
            j1=j;
            Task task = Task.Factory.StartNew(() => {
                for (int k = 0; k < b.GetLength(0); k++) {
                    new_matrix[i1, j1] += a[i1, k] * b[k, j1];
                }
                //Console.WriteLine("Task = {0}", Task.CurrentId);
            });
            task.Wait();
            t.Add(task);
        }
    }
    string s = Convert.ToString(stopWatch.ElapsedMilliseconds);
    Console.WriteLine();
    Console.WriteLine();
    Console.Write("Time task: ");
    Console.WriteLine(s);
    stopWatch.Stop();
    Console.WriteLine();
    return new_matrix;
}