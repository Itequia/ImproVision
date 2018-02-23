using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Improvision.Models.InitialModels
{
    public class MicrosoftVisionAPIResult
    {
        public string status { get; set; }
        public bool succeeded { get; set; }
        public bool failed { get; set; }
        public bool finished { get; set; }
        public recognitionResult recognitionResult { get; set; }
    }

    public class recognitionResult
    {
        public List<line> lines { get; set; }
    }

    public class line : Boxeable
    {
        public string text { get; set; }
        public List<word> words { get; set; }
    }

    public class word : Boxeable
    {
        public string text { get; set; }     
    }

    public abstract class Boxeable
    {
        public int[] boundingBox { get; set; }

        public BoundingBox Box
        {
            get
            {
                return new BoundingBox
                {
                    coordenadas = new Coordenada[] {
                          new Coordenada{x = boundingBox[0], y = boundingBox[1]}, // 0
                          new Coordenada{x = boundingBox[2], y = boundingBox[3]}, // 1
                          new Coordenada{x = boundingBox[4], y = boundingBox[5]}, // 2
                          new Coordenada{x = boundingBox[6], y = boundingBox[7]}, // 3
                    }
                };
            }
        }

        public string Height
        {
            get
            {
                return average(boundingBox[5], boundingBox[7]) -
                       average(boundingBox[1], boundingBox[3]) + "px";
            }
        }

        public string Width
        {
            get
            {
                return average(boundingBox[2], boundingBox[4]) -
                       average(boundingBox[0], boundingBox[6]) + "px";
            }
        }

        public string Left
        {
            get
            {
                return average(boundingBox[0], boundingBox[6]) + "px";
            }
        }

        public string Top
        {
            get
            {
                return average(boundingBox[1], boundingBox[3]) + "px";
            }
        }

         public string FontSize
        {
            get
            {
                double height = average(boundingBox[5], boundingBox[7]) - average(boundingBox[1], boundingBox[3]);
                return Math.Ceiling(height * 0.7) + "px";
            }
        }

        private int average(int a, int b)
        {
            return (a + b) / 2;
        }
    }

    public class BoundingBox
    {
        public Coordenada[] coordenadas { get; set; }
    }
    public class Coordenada
    {
        public int x { get; set; }
        public int y { get; set; }

    }

     
}
