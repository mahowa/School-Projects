using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_MAIN
{
    public class Cache
    {
        private int BLOCKSIZE, ROWS, CACHETYPE, NWAY;
        public int misses = 0, hits = 0, tag, row, counter = 0;
        private int[] entries; 
        private object[,] SA;
        private bool CacheHit;
        private int address;
        
            
        public Cache(int bs, int rows, int ct, int nway = 0)
        {
            BLOCKSIZE = bs;
            ROWS = rows;
            CACHETYPE = ct;
            NWAY = nway;
            
            entries = new int[ROWS];
            SA = new object[ROWS, 2];
            //For SET ASSOCIATIVE
            if (CACHETYPE == 2)
            {
                for (int i = 0; i < ROWS; i++)
                {
                    //WE NEED AN lru array with the same size as "WAYS"
                    int[] tmpLRU = new int[NWAY];
                    int[] tmpEntries = new int[NWAY];
                    counter = 0;
                    foreach (int e in tmpEntries)
                        tmpEntries[counter++] = -1;
                    counter = 0;
                    foreach (int w in tmpLRU)
                        tmpLRU[counter] = NWAY - counter++;

                    SA[i, 0] = tmpEntries;
                    SA[i, 1] = tmpLRU;
                }
            }
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
        public bool Simulator(int address)
        {
            this.address = address;
            //Calculate row and tag from memorry address
            row = (address / BLOCKSIZE) % ROWS;
            if (CACHETYPE == 1)
                tag = address / (BLOCKSIZE * 1);
            else
                tag = address / (BLOCKSIZE * ROWS);

            //Direct Mapped
            if (CACHETYPE == 0)
            {
                if (entries[row] == tag)
                {
                    hits++;
                    CacheHit = true;
                }
                else
                {
                    entries[row] = tag;
                    misses++;
                    CacheHit = false;
                }
            }
            //SET ASSOCIATIVE
            else  
                SetAssociative();
            return CacheHit;
        }


        private void SetAssociative()
        {
            int[] lruA = new int[0];
            if(CACHETYPE == 2)
            {
                entries = (int[])SA[row,0];
                lruA = (int[])SA[row, 1];
            }
            int Entry = 0;
            bool TAGROWMISS = true;
            for (int i = 0; i < NWAY; i++)
            {
                //check each row if tag matches
                if (entries[i] == tag)
                {
                    Entry = i;
                    TAGROWMISS = false;
                    hits++;

                    //RESET LRU BIT
                    int temp = lruA[i];
                    lruA[i] = 1;

                    //Increment ever other row lru bit less than the one we reset
                    for (int j = 0; j < lruA.Length; j++)
                        if (j != i)
                            if (lruA[j] < temp)
                                lruA[j]++;
                    CacheHit = true;
                    break;
                }

                //Get the row that was Least Recently Used
                if (lruA[i] == lruA.Length)
                    Entry = i;
            }
            if (TAGROWMISS)
            {
                misses++;
                CacheHit = false;
                for (int i = 0; i < NWAY; i++)
                {
                    //RESET entry that was Least Resently Used and cache the tag
                    if (i == Entry)
                    {
                        entries[Entry] = tag;
                        lruA[i] = 1;
                    }
                    //Increment every other rows lru bit
                    else
                        lruA[i] += 1;
                }
            }
        }

    }
}
