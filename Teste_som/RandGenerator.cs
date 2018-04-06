using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Teste_som
{
    public class RandGenerator
    {
        private double[] rand;
        private int[] intrand;
        private int irand;
        private int nrand;

        public RandGenerator()
        {
            //ReadList();
            irand = 0;
            Random random = new Random();
            irand = random.Next(0, 100);
            irand = irand % nrand;
            intrand[irand] = random.Next(0, 100);
            random = new Random(intrand[irand]);
            rand[irand] = random.Next(0, 10000) / 10000;
            
        }       


    }
}
