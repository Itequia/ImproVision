﻿using System;
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
                          new Coordenada{x = boundingBox[0], y = boundingBox[1]},
                          new Coordenada{x = boundingBox[2], y = boundingBox[3]},
                          new Coordenada{x = boundingBox[4], y = boundingBox[5]},
                          new Coordenada{x = boundingBox[6], y = boundingBox[7]},
                    }
                };
            }
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
