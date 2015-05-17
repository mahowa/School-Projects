using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_Simulator;

namespace CPU_MAIN
{
    class SelectionSortCPU
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int count = 0;
                int[] unsorted = new int[100];//{ 
                //                     81, 93, 33, 15, 53, 71, 76, 86, 3, 37, 
                //                     94, 87, 22, 86, 82, 32, 1,	31, 56, 18, 
                //                     70, 76, 68, 68, 96, 12, 29, 72, 28, 76,
                //                     27, 11, 20, 87, 83, 68, 3, 34, 61, 10, 
                //                     14, 56, 38, 2, 47,	5, 30, 70, 36, 4,	
                //                     96, 42, 4, 76, 46, 53, 14,	85, 16, 34, 
                //                     90, 27, 82, 89, 35, 52, 88, 9, 49, 51,
                //                     56, 12, 69, 2,	31,	74,	34,	51,	30, 91, 
                //                     34, 49, 34, 1,	71,	10,	2,	26,	32,	91,	
                //                     74, 90, 66, 30, 95, 18, 18, 31, 44, 100
                //                 };
                Random numGen = new Random();
                for (int i = 0; i < 100; i++)
                    unsorted[i] = numGen.Next(0,200);

                CPU_SIMULATOR sim = new CPU_SIMULATOR();
                foreach (int i in unsorted)
                    sim.inputController.Enqueue(i);
                sim.SIMULATOR();

                count = 100;
                while (count-- > 0)
                    Console.Write(sim.OutputController.Dequeue() + "\t");

                Console.WriteLine("It took the cpu " + sim.Cycles + " cycles to complete\n");
                Console.WriteLine("NEW Simulation\t(y/n)");
                if (Console.ReadLine() == "n")
                    break;
            }
        }
    }
}
