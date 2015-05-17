using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPU_MAIN;

namespace CPU_Simulator
{
    public class CPU_SIMULATOR2
    {
        public CPU_SIMULATOR2()
        {
            inputController = new Queue<int>();
            outputController = new Queue<int>();
            memory = new int[255];
            Cycles = 0;
        }
        public TwoBit TB = new TwoBit();
        private int PC { get; set; }
        private int P { get; set; }
        private int CP { get; set; }
        private int BD { get; set; }
        private int BA { get; set; }
        private int PD { get; set; }
        private int[] memory { get; set; }
        public Queue<int> inputController { get; set; }
        public Queue<int> outputController { get; set; }
        public int Cycles { get; set; }
        public int TotalCycles { get { return Cycles / 2 + TB.stalls / 2; } }
        public int TimeSpan { get { return TotalCycles * Clock; } }
        private int Clock = 9;
        private int instructionCount { get; set; }
        public double CPI { get { return (double)TotalCycles / (double)instructionCount; } }
        private void point(int imm)
        {
            P = imm;
            CP = P;
            PC++;

            Cycles++;
        }
        private void read()
        {

            BD = inputController.Dequeue();
            PC++;
            Cycles++;
        }

        private void sift()
        {
            if (memory[CP-1] > BD)
            {
                BD = memory[CP-1];
                BA = CP-1;
            }
            Cycles++;
            PC++;
        }
        private void citer(int imm)
        {
            CP--;
            if (CP > 0)
            {
                PC = imm;
                TB.predict(true, PC);
            }
            else
            {
                TB.predict(false, PC);
                PC++;
            }
            Cycles++;
        }
        private void piter(int imm)
        {
            P--;
            if (P > 0)
            {
                TB.predict(true, PC);
                PC = imm;
            }
            else
            {
                TB.predict(false, PC);
                PC++;
            }
            CP = P;
            Cycles++;
        }
        private void regload()
        {
            BA = CP-1;
            BD = memory[CP-1];
            PD = BD;
            PC++;
            Cycles++;
        }
        private void store(int imm)
        {
            if (imm != 255)
                memory[imm] = BD;
            else
                outputController.Enqueue(BD);
            PC++;
            Cycles++;
        }
        private void swapa()
        {
            memory[P-1] = BD;
            PC++;
            Cycles++;
        }
        private void swapb()
        {
            memory[BA] = PD;
            PC++;
            Cycles++;
        }
        private void write()
        {
            BD = memory[CP-1];
            PC++;
            Cycles++;
        }
        public void SIMULATOR()
        {
            
            string[] instructions = {
                                        "point","read", "store", "citer",                               //Read input
                                        "point","regload","sift", "citer", "swapa", "swapb", "piter",   //Sort it
                                        "point","write", "store", "citer"                               //Output
                                    };
            const int Input = 1, Load = 5, Find = 6, Output = 12;
            
            while (PC < instructions.Count())
            {
                Cycles++;
                instructionCount++;
                switch (instructions[PC])
                {
                    case "point":
                        point(100);
                        break;
                    case "read":
                        read();
                        break;
                    case "store":
                        if(PC == 2)
                            store(CP-1);
                        else
                            store(255);
                        break;
                    case "citer":
                        if (PC == 3)
                            citer(Input);
                        else if (PC == 7)
                            citer(Find);
                        else
                            citer(Output);
                        break;
                    case "regload":
                        regload();
                        break;
                    case "sift":
                        sift();
                        break;
                    case "swapa":
                        swapa();
                        break;
                    case "swapb":
                        swapb();
                        break;
                    case "piter":
                        piter(Load);
                        break;
                    case "write":
                        write();
                        break;
                    
                }
            }
        }
        public class TwoBit
        {
            private int position = 0;    ///Set to take
            public int stalls = 0;
            public string stallstring = "";
            private int times = 0;
            public bool predict(bool took, int line)
            {
                if (took && position<2)
                {
                    times++;
                    if(position > 0)
                        position--;
                    return true;
                }
                else if(!took && position>2)
                {
                    times++;
                    if(position < 4)
                        position++;
                    return true;
                }
                else{
                    stallstring += Environment.NewLine + "Stall on line " + line + "\tafter " + times + "\tcorrect predictions";
                    times = 0;
                    stalls++;
                    return false;

                }
            }
        }
    }
}
