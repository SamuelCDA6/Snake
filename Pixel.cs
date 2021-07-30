using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{

    public class Pixel
    {
        // Shape : 0 Head, 1 Tail, 2 Fruit, 3 Blank
        private int _Xpos, _Ypos, _Shape;

        public int Xpos { get => _Xpos; set => _Xpos = value; }
        public int Ypos { get => _Ypos; set => _Ypos = value; }
        public int Shape { get => _Shape; private set => _Shape = value; }



        public Pixel(int xpos, int ypos, int shape)
        {
            Xpos = xpos;
            Ypos = ypos;
            Shape = shape;
        }

        public Pixel(Pixel point, int shape)
        {
            Xpos = point.Xpos;
            Ypos = point.Ypos;
            Shape = shape;
        }

        public override bool Equals(object obj)
        {
            return obj is Pixel p && Xpos ==  p.Xpos && Ypos == p.Ypos;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
