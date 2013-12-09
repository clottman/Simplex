using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaikesSimplexService.DataModel
{
    public class BasicVar
    {
       public int row;
       public int column;

       public BasicVar(int ro, int col)
       {
           row = ro;
           column = col;
       }
    }
}
