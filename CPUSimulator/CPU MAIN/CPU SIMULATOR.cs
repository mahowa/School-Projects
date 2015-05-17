using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_MAIN;

namespace CPU_Simulator
{
    public class CPU_SIMULATOR
    {
        public CPU_SIMULATOR()
        {
            memory = new int[255];
            PC = 0;
        }
        /// <summary>
        /// Register For Math Operation
        /// </summary>
        private int A {get; set;}
        /// <summary>
        /// Temporary Register
        /// </summary>
        private int B { get; set; }
        /// <summary>
        /// Program Counter
        /// </summary>
        private int PC { get; set; }
        /// <summary>
        /// Status Bits for ALU
        /// </summary>
        private int S { get; set; }
        /// <summary>
        /// Counter Register
        /// </summary>
        private int I { get; set; }
        /// <summary>
        /// Represents RAM
        /// </summary>
        public int[] memory { get; set; }
        /// <summary>
        /// Input Device
        /// </summary>
        public Queue<int> inputController = new Queue<int>();
        /// <summary>
        /// Output Device
        /// </summary>
        public Queue<int> OutputController = new Queue<int>();
        /// <summary>
        /// Memory address of current position variable
        /// </summary>
        private int pos { get; set; }
        /// <summary>
        /// Memory address of current data variable
        /// </summary>
        private int current { get { return pos + 1; } }
        /// <summary>
        /// Memory address of best data variable
        /// </summary>
        private int best { get { return pos + 2; } }        
        /// <summary>
        /// Instruction Fetch Cache
        /// </summary>
        public Cache IFCache = new Cache(4, 4, 2, 2);
        /// <summary>
        /// Data Cache 
        /// </summary>
        public Cache DataCache = new Cache(4, 4, 0);
        /// <summary>
        /// CPU Clock in Nano Seconds
        /// </summary>
        private int clock = 3;

        public int input { get; set; }          //Input Cycles
        public int output { get; set; }         //Output Cycles
        public int IFcycles { get; set; }       //Instruction fetch cycles
        public int PCcycles { get; set; }       //PC counter cycles
        public int OPcycles { get; set; }       //ALU OP Cycles
        public int CompletionTime { get { return Cycles * clock; } }        //Time in nano seconds to completion
        public int BranchCycles { get; set; }                               //Branch Cycles

        private int instructionsCount { get; set; }
        public double CPI { get { return (double)Cycles / (double)instructionsCount; } }
        private int _Cycles;
        /// <summary>
        /// Cycles to complete computation
        /// </summary>
        public int Cycles {
            get { return _Cycles; }
            set {  _Cycles = value;}
        }


        /// <summary>
        /// Sets the A register to contain a sign-extended immediate value
        /// </summary>
        /// <param name="immediate"></param>
        public void seta(int immediate)
        {
            Cycles += 2;
            A = immediate;
            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Decrements the value in the I register by 1,
        /// sets the equal and less than status flags
        /// appropriately for the implicit comparison
        /// between I and 1
        /// </summary>
        public void deci()
        {
            Cycles += 2;
            I--;
            PC++;
            PCcycles++;
            if (I == 1)
                S = 1;
            else if (I > 1)
                S = 2;
            else
                S = 0;
            OPcycles++;
        }

        /// <summary>
        /// Loads a word from memory from the specified address
        /// and stores it in A
        /// </summary>
        /// <param name="adrr"></param>
        public void loada(int adrr)
        {
            if (DataCache.Simulator(adrr))
                Cycles += 2;
            else
                Cycles += (2 + 3);
            int cur = memory[current];
            int bs = memory[best];
            int poss = memory[pos];
            //Read input from i/o
            if (adrr == 254)
            {
                A = inputController.Dequeue();
                input += 2;
                Cycles += 2;
            }
            //Read From memory
            else
                if (adrr == current)
                    A = cur;
                else if (adrr == best)
                    A = bs;
                else A = poss;
            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Loads a word from memory using the I
        /// register for the address and stores it in A
        /// </summary>
        public void loadai()
        {
            if (DataCache.Simulator(I-1))
                Cycles += 2;
            else
                Cycles += (2 + 3);

            A = memory[I-1];
            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Stores the word in the A register to the memory location at 
        /// the specified address
        /// </summary>
        public void storea()
        {
            if (PC == 8)
            {
                memory[pos] = A;
                DataCache.Simulator(pos);
                DataCache.misses--;
                Cycles += 5;
            }
            else if (PC == 11)
            {
                memory[current] = A;
                DataCache.Simulator(current);
                DataCache.misses--;
                Cycles += 5;
            }

            else if (PC == 23)
            {
                memory[best] = A;
                DataCache.Simulator(best);
                DataCache.misses--;
                Cycles += 5;
            }
            else if (PC == 33)
            {
                memory[pos] = A;
                DataCache.Simulator(pos);
                DataCache.misses--;
                Cycles += 5;
            }

            else
            {
                DataCache.Simulator(I - 1);

                Cycles += 2;
                output += 2;

                OutputController.Enqueue(memory[I - 1]);
            }
            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Stores the word in the A register to the memory location at 
        /// the address stored in the I register
        /// </summary>
        /// <param name="adrr"></param>
        public void storeai()
        {
            DataCache.Simulator(I - 1);
            Cycles += 5;
            memory[I-1] = A;
            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Ensures the data at the specified memory address is not in the cache
        /// </summary>
        /// <param name="adrr"></param>
        public void flush(int adrr)
        {
            Cycles += 2;
            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Loads a word from memory using the I register for the address and compares it against the
        /// A register, the status flags are set appropriately for the comparison between A and Mem[I]
        /// </summary>
        public void cmpai()
        {
            if (DataCache.Simulator(I - 1))
                Cycles += 2;
            else
                Cycles += (2+3);

            if (A < memory[I-1])
                S = 0;
            else if (A == memory[I-1])
                S = 1; 
            else
                S = 2;

            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Branch to the specified address if the last math operation indicated
        /// the primary operand was greater than or equal to the secondary operand
        /// </summary>
        /// <param name="adrr"></param>
        public void bge(int adrr)
        {
            Cycles ++;
            if (S > 0)
                PC = adrr;
            else
                PC++;
            PCcycles++;
            BranchCycles ++;
        }

        /// <summary>
        /// Branch to the specified address if the last mathc operation
        /// indicated the primary operand was less than the secondary operand
        /// </summary>
        /// <param name="adrr"></param>
        public void blt(int adrr)
        {
            Cycles ++;
            if (adrr == 0)
            {
                PC++;
                return;
            }
            if (S == 0)
                PC = adrr;
            else
                PC++;
            PCcycles++;
            BranchCycles += 2;
        }

        /// <summary>
        /// Copies the contents of the A register
        /// into the I register
        /// </summary>
        public void tai()
        {
            Cycles += 2;
            I = A + 1;
            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Copies contents of the B register
        /// into the I register
        /// </summary>
        public void tbi()
        {
            Cycles += 2;
            I = B + 1;
            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Copies contents of the I register
        /// into the A register
        /// </summary>
        public void tia()
        {
            Cycles += 2;
            A = I - 1;
            PC++;
            PCcycles++;
            OPcycles++;
        }

        /// <summary>
        /// Copies contents of the I register
        /// into the B register
        /// </summary>
        public void tib()
        {
            Cycles += 2;
            B = I - 1;
            PC++;
            PCcycles++;
            OPcycles++;
        }

        public void SIMULATOR()
        {
            pos = inputController.Count;
            string[] instructions = {
                                        "seta", "tai", "flush", "loada", "storeai", "deci",
                                        "bge", "seta", "storea", "tai", "loadai", "storea",
                                        "tib", "loadai", "deci", "blt","cmpai", "blt", "tib",
                                        "bge", "cmpai", "bge", "blt", "storea", "loada","tbi","storeai",
                                        "loada", "tai", "loada", "storeai", "deci", "tia", "storea", "bge",
                                        "seta", "tai", "loadai", "storea", "deci", "bge", "blt"
                                    };
            const int Start = 0, InLoop = 2, Outer = 10, Keep = 13, Inner = 14, NoBetter = 20, Swap = 23, OutLoop = 37;
            
            while (PC < instructions.Count())
            {
                instructionsCount++;
                if (IFCache.Simulator(PC))
                {
                    Cycles++;
                    if (instructions[PC].ToCharArray()[0] == 'b')
                        BranchCycles++;
                }
                else
                {
                    Cycles += 3;
                    if (instructions[PC].ToCharArray()[0] == 'b')
                        BranchCycles += 3;
                }

                IFcycles++;

                switch (instructions[PC])
                {
                    case "seta":
                        seta(99);
                        break;
                    case "deci":
                        deci();
                        break;
                    case "loada":
                        if (PC == 3)
                            loada(254);
                        else if (PC == 24)
                            loada(current);
                        else if (PC == 27)
                            loada(pos);
                        else
                            loada(best);
                        break;
                    case "loadai":
                        loadai();
                        break;
                    case "storea":
                        storea();
                            break;
                    case "storeai":
                        storeai();
                        break;
                    case "flush":
                        flush(254);
                        break;
                    case "cmpai":
                        cmpai();
                        break;
                    case "bge":
                        if (PC == 6)
                            bge(InLoop);
                        else if (PC == 19)
                            bge(Keep);
                        else if (PC == 21)
                            bge(Inner);
                        else if (PC == 34)
                            bge(Outer);
                        else
                            bge(OutLoop);
                        break;
                    case "blt":
                        if (PC == 15)
                            blt(Swap);
                        else if (PC == 17)
                            blt(NoBetter);
                        else if (PC == 22)
                            blt(Inner);
                        else
                            blt(Start);
                        break;
                    case"tai":
                        tai();
                        break;
                    case"tbi":
                        tbi();
                        break;
                    case "tia":
                        tia();
                        break;
                    case "tib":
                        tib();
                        break;
                }
            }
        }

    }
}
