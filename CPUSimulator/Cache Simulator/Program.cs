using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Cache_Simulator
{
    class Program
    {
        static StreamWriter writer;
        static TextWriter oldOut = Console.Out;

        /// <summary>
        /// Sets up the cache using User input
        /// Calculates cache use and bit distribution throughout cache
        /// Calls the cache simulator method using input as parameters
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            while (true)
            {
                //Variables used for Cache Simulation
                const int addr_bits = 16, cyclesHit = 1, cachLimit = 900, valid_bit = 1;
                int lru_bits = 0, data_bits, offset_bits, tag_bits, row_bits, total_bits,
                    remaining_bits, cyclesMiss = 20, NWAY = 0, ROWS, CACHETYPE, BLOCKSIZE, LOOP;
                int[] addresses;
                string cache = "";
                //ASK for cache Type
                Console.Write("Enter TYPE OF CACHE\n0 = DIRECT MAPPED \n1 = FULLY ASSOCIATIVE \n2 = SET ASSOCIATIVE\n\n\tTYPE=:\t\t");
                CACHETYPE = Convert.ToInt16(Console.ReadLine());        //0 = DIRECT MAPPED; 1 = FULLY ASSOCIATIVE; 2 = SET ASSOCIATIVE
                
                //GET the number of ways for SET ASSOCIATIVE
                if (CACHETYPE == 2)
                {
                    Console.Write("\nEnter #-Ways:\t");
                    NWAY = Convert.ToInt16(Console.ReadLine());
                    //CALCULATE LRU BITS
                    lru_bits = Convert.ToInt32(Math.Ceiling(Math.Log(NWAY, 2)));
                }

                //GET THE NUMBER OF ROWS IN CACHE
                Console.Write("\nEnter # of Rows in Cache:\t");
                ROWS = Convert.ToInt16(Console.ReadLine());
                
                //CALCULATE ROW BITS
                row_bits = Convert.ToInt32(Math.Ceiling(Math.Log(ROWS, 2)));

                //SET THE ADDRESS BITS FOR FULLY ASSOCIATIVE
                if (CACHETYPE == 1)
                {
                    lru_bits = Convert.ToInt32(Math.Ceiling(Math.Log(ROWS, 2)));
                    row_bits = 0;
                }
                
                //GET DATA BLOCK SIZE
                Console.Write("\nEnter Data Blocksize in bytes:\t");
                BLOCKSIZE = Convert.ToInt16(Console.ReadLine());       

                //CALCULATE THE MISS CYCLES (20 + 1 per byte read)
                cyclesMiss += BLOCKSIZE;

                //CALCULATE THE Bit layout of cache
                data_bits = BLOCKSIZE * 8;
                offset_bits = Convert.ToInt32(Math.Ceiling(Math.Log(BLOCKSIZE, 2)));
                tag_bits = addr_bits - row_bits - offset_bits;
                int entryNum = ROWS;
                if (CACHETYPE == 2)
                    entryNum = ROWS * NWAY;
                total_bits = (valid_bit + data_bits + tag_bits + row_bits + lru_bits)* entryNum;
                remaining_bits = cachLimit - total_bits;

                //ASK FOR LOOPS TO SIMULATE //DEFAULT 2
                //Console.Write("\nEnter # of loops to simulate:\t");
                LOOP = 2;//Convert.ToInt16(Console.ReadLine());             //Number of times we want to repeat simulation and memory addresses

                //MEMORY ADDRESSES FOR CACHE SIMULATION
                addresses =             
                            new int[]{ 
                                        16, 20, 24, 28, 32, 36, 60, 
                                        64, 56, 60, 64, 68, 56, 60, 
                                        64, 72, 76, 92, 96, 100, 104,
                                        108, 112, 120, 124, 128, 144, 148 
                                    };
                
                //CREATE THE OUTPUT STRING
                switch(CACHETYPE)
                {
                    case 0:
                        cache = "Direct Mapped  cache  with\n" + ROWS + " Rows and " + BLOCKSIZE + " bytes per row:\n"
                            +"\nBits in the data block:\t"+ data_bits
                            + "\nOffset address bits:\t" + offset_bits
                            + "\nBits in the valid bit:\t" + valid_bit
                            + "\nBits in the tag:\t" + tag_bits
                            + "\nBits in the row\t" + row_bits + "\n";
                        break;
                    case 1:
                        cache = "Fully Associative cache with\n" + ROWS + " Rows and " + BLOCKSIZE + " bytes per row:\n"
                            + "\nBits in the data block:\t" + data_bits
                            + "\nOffset address bits:\t" + offset_bits
                            + "\nBits in the valid bit:\t" + valid_bit
                            + "\nBits in the tag:\t" + tag_bits
                            + "\nBits in the LRU\t\t" + lru_bits + "\n";
                        break;
                    case 2:
                        cache = NWAY + "- Way Set Associative cache with\n" + ROWS + " Rows and " + BLOCKSIZE + " bytes per row:\n"
                            + "\nBits in the data block:\t" + data_bits
                            + "\nOffset address bits:\t" + offset_bits
                            + "\nBits in the valid bit:\t" + valid_bit
                            + "\nBits in the tag:\t" + tag_bits
                            + "\nBits in the row\t" + row_bits
                            + "\nBits in the LRU\t\t" + lru_bits + "\n";
                        break;
                }
                //Write to Txt Document
                writeToFile();

                //OUTPUT BIT TOTALS
                Console.WriteLine("\n"+cache);
                Console.WriteLine("Total Used Bits:\t" + total_bits + "\nRemaining Bits:\t" + remaining_bits + "\n");
                Console.WriteLine("Hit Time:\t" + cyclesHit + "\tMiss Time:\t" + cyclesMiss);

                //CALL THE CACHE SIMULATOR
                CacheSimulator(BLOCKSIZE, ROWS, addresses, LOOP, CACHETYPE, NWAY, cyclesMiss, cyclesHit);

                //Stop writing to file
                closeFile();

                //ASK TO RESTART
                Console.WriteLine("Another simulation? (y/n):\t");
                if (Console.ReadLine().ToLower() == "n")
                    break;
            }
        }

        /// <summary>
        /// Method to start writing console output to text file
        /// </summary>
        private static void writeToFile()
        {
            try
            {
                //Write output of simulation to CACHE_SIMULATION.txt is source folder
                writer = File.AppendText("./CACHE_SIMULATION.txt");//new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);
        }

        /// <summary>
        /// Method to stop writing to text file
        /// </summary>
        private static void closeFile()
        {
            Console.SetOut(oldOut);
            writer.Close();
        }

        /// <summary>
        /// Cach simulator program
        /// </summary>
        /// <param name="BLOCKSIZE">Data Block Size</param>
        /// <param name="ROWS"># of Rows in Cache</param>
        /// <param name="addresses">Array of int memmory addresses</param>
        /// <param name="LOOP"># of times to simulate accesing those memory addresses</param>
        /// <param name="CACHETYPE">Direct Mapped: 0 , Fully Associative: 1, Set associative: 2</param>
        /// <param name="NWAY">If Set Associative # of entries per row</param>
        /// <param name="mCycles">Miss cycles cost</param>
        /// <param name="hCycles">Hit cycles cost</param>
        private static void CacheSimulator(int BLOCKSIZE, int ROWS, int[] addresses, int LOOP, int CACHETYPE, int NWAY, int mCycles, int hCycles)
        {      
            int misses = 0, hits = 0, tag, row, counter = 0;
            object[] entries= new object[ROWS];
            int[] lruA, nWays = new int[NWAY];
            object[,] SA = new object[ROWS, 2];

            //For SET ASSOCIATIVE
            if (CACHETYPE == 2)
            {
                //WE NEED AN lru array with the same saze as "WAYS"
                lruA = new int[NWAY];

                //Invalidate eache entry
                foreach (int i in nWays)        
                    nWays[counter++] = -1;

                //Set up lru
                if (CACHETYPE == 2)
                {
                    counter = 0;
                    foreach (int l in lruA)         //Invalidate "WAY" (Entry) as
                        lruA[counter] = (NWAY - 1) - counter++;
                }

                //Set up multidimentional array. 
                //Each row contains an array of n-entries and 
                //An another same sized array for keeeping track of lru entry
                counter = ROWS;
                while (counter-- > 0)
                {
                    SA[counter, 0] = nWays;
                    SA[counter, 1] = lruA;
                }
            }
            //For FULLY ASSOCIATIVE
            else
                lruA = new int[ROWS];           //WE NEED AN lru array with the size of the # of rows

            //Invalidate each row to simulate cache startup
            counter = 0;
            foreach (var r in entries)
            {
                entries[counter] = -1;
                if (CACHETYPE != 2)
                {
                    lruA[counter] = ROWS - counter++;
                }
            }
            
            //Start Cache Simulation
            counter = 1;
            while (LOOP-- > 0)
            {
                //Reset loop variables
                hits = 0;
                misses = 0;
                int cost = 0;
                double cpi = 0;

                //Iterate through memory addresses
                foreach (int a in addresses)
                {
                    //Calculate row and tag from memorry address
                    row = (a / BLOCKSIZE) % ROWS;
                    if (CACHETYPE == 1)
                        tag = a / (BLOCKSIZE * 1);
                    else
                        tag = a / (BLOCKSIZE * ROWS);

                    //Direct Mapped
                    //Check row for tag. If tag exists mark hit. else mark miss
                    if (CACHETYPE == 0)
                    {
                        if ((int)entries[row] == tag)
                        {
                            hits++;
                            Console.WriteLine("Address\t{0}\tHit on row\t{1}\twith tag\t{2}", a, row, tag);
                        }
                        else
                        {
                            entries[row] = tag;
                            Console.WriteLine("Address\t{0}\tMiss on row\t{1} \twith tag\t{2}", a, row, tag);
                            misses++;
                        }
                    }

                    //FULLY ASSOCIATIVE
                    else if (CACHETYPE == 1)
                    {
                        int[] FA = new int[ROWS];
                        counter = 0;
                        foreach (var v in entries)
                            FA[counter++] = (int)v;
                        FullyAssociative(ref FA, ref lruA, tag, ROWS, a, ref hits, ref misses, row, 0);
                        for (int i = 0; i < ROWS; i++)
                            entries[i] = FA[i];
                    }

                    //SET ASSOCIATIVE
                    else
                    {
                        int[] FA = (int[])SA[row, 0];
                        int[] rowLRU = (int[])SA[row, 1];
                        FullyAssociative(ref FA, ref rowLRU, tag, NWAY, a, ref hits, ref misses, row, 1);
                        SA[row, 0] = FA;
                        SA[row, 1] = rowLRU;

                    }
                }

                //Calculate and write out totals from the cache simulation
                Console.WriteLine("\n\t\tMisses:\t{0}\tHits:\t{1}\n\t\tLoop:\t{2}\n", misses, hits, counter++);
                cost = (hCycles * hits) + (mCycles * misses);
                cpi = (double)cost /(double) addresses.Length;
                Console.WriteLine("Total clock cycles:\t" + cost + "\nAverage CPI:\t" + cpi+ "\n");
            }
        }

        private static void FullyAssociative(ref int[] entries, ref int[] lruA, int tag, int ROWS, int a, ref int hits, ref int misses, int row, int cache)
        {
            int Entry = 0;
            bool TAGROWMISS = true;
            for (int i = 0; i < ROWS; i++)
            {
                //check each row if tag matches
                if (entries[i] == tag)
                {
                    Entry = i;
                    TAGROWMISS = false;
                    hits++;
                    //SET ASSOCIATIVE
                    //if (cache == 2)
                        Console.WriteLine("Address\t" + a + "\tHit on Entry\t" + Entry + "\t in row\t" + row + "\twith tag\t" + tag);
                    //Fully Associative
                   // else
                    //    Console.WriteLine("Address\t" + a + "\tHit on Entry\t" + Entry + "\twith tag\t" + tag);

                    //RESET LRU BIT
                    int temp = lruA[i];
                    lruA[i] = 1;

                    //Increment ever other row lru bit less than the one we reset
                    for (int j = 0; j < lruA.Length; j++)
                        if (j != i)
                            if (lruA[j] < temp)
                                lruA[j]++;
                    break;
                }

                //Get the row that was Least Recently Used
                if (lruA[i] == lruA.Length)
                    Entry = i;
            }
            if (TAGROWMISS)
            {
                misses++;
                for (int i = 0; i < ROWS; i++)
                {
                    //RESET entry that was Least Resently Used and cache the tag
                    if (i == Entry)
                    {
                        entries[Entry] = tag;
                        lruA[i] = 1;
                        //if (cache == 2)
                            Console.WriteLine("Address\t" + a + "\tMiss on Entry\t" + Entry + "\t in row\t" + row + "\twith tag\t" + tag);
                        //else
                          //  Console.WriteLine("Address\t" + a + "\tMiss on Entry\t" + Entry + "\twith tag\t" + tag);
                    }
                    //Increment every other rows lru bit
                    else
                        lruA[i] += 1;
                }
            }
        }

    }
}
