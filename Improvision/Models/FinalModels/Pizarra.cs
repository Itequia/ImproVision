using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Improvision.Models.FinalModels
{
    public class Pizarra
    {
        public string Texto { get; set; }
        public BoundingBox BoundingBox { get; set; }

        public List<Frase> Frases { get; set; }
    }
}
