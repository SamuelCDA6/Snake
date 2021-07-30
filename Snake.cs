using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class Snake
    {
        public enum Direction { Up, Down, Left, Right };

        private bool _IsAlive = true;
        private List<Pixel> _Tail = new();
        private Pixel _Head;
        private Direction _Dir = Direction.Left;

        public bool IsAlive { get => _IsAlive; set => _IsAlive = value; }
        public List<Pixel> Tail { get => _Tail; set => _Tail = value; }
        public Pixel Head { get => _Head; set => _Head = value; }
        public Direction Dir { get => _Dir; set => _Dir = value; }

        public Snake(Pixel head)
        {
            Head = head;
        }
        
        /// <summary>
        /// It's for adding a new part of the tail when the snake eat a fruit
        /// </summary>
        /// <param name="p">The pixel position to add the tail</param>
        public void AddTail(Pixel p)
        {
                Tail.Add(new(p.Xpos, p.Ypos, 1));
        }
        
    }
}
