using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace Devilguard
{
    class Node
    {
        public Point Pos;
        public int G, H;
        public Node Parent;


        public Node(Point pos)
        {
            Pos = pos;
        }

        public Node(Point pos, Node parent, int g, int h)
        {
            Pos = pos;
            Parent = parent;
            G = g;
            H = h;
        }

        public int F()
        {
            return G + H;
        }
    }


    class Priority_List_Class
    {
        public Dictionary<int, List<Point>> List = new Dictionary<int, List<Point>>();

        public void Add(int amount,Point node)
        {
            if (!List.ContainsKey(amount))
                List.Add(amount, new List<Point>());
            if (!List[amount].Contains(node))
                List[amount].Add(node);
        }

        public void Remove(int amount, Point node)
        {
            List[amount].Remove(node);
            if (List[amount].Count == 0) List.Remove(amount);
        }


        public int Count()
        {
            return List.Count();
        }

        public int MinList()
        {
            return List.Keys.Min();
        }

        public Point FirstNode(int MinList)
        {
            return List[MinList].First();
        }

    }






    class PathFind
    {
        Tile_Type[,] Tile_Map;
        Catalog Data;

        Rectangle MapRect;
        //public LinkedList<Point> Path = new LinkedList<Point>();
        public List<Point> Path = new List<Point>();
        public PathFind(Tile_Type[,] Map, Rectangle Rect, Catalog catalog)
        {
            Tile_Map = Map;
            MapRect = Rect;
            Data = catalog;
        }

        public Priority_List_Class Priority_List = new Priority_List_Class();
        public Dictionary<Point, Node> List = new Dictionary<Point, Node>();
        public HashSet<Point> CloseList = new HashSet<Point>();

        public void AddTo_OpenList(Node Node)
        {
            List.Add(Node.Pos, Node);
            Priority_List.Add(Node.F(), Node.Pos);
        }


        public void AddTo_ClosedList(Node Node)
        {
            CloseList.Add(Node.Pos);
        }


        public Point Start_Point;
        public Point End_Point;        
        public PFState Found_State;


        public PFState Pathfind_Step()
        {
            if (Priority_List.Count() == 0)
                Found_State = PFState.Imposable;
            if (Found_State != PFState.NotFinished)
                return Found_State;

            if (Found_State == PFState.NotFinished)
            {
                int MinF = Priority_List.MinList();
                Node Node = List[Priority_List.FirstNode(MinF)];
                if (Node.Pos == End_Point)
                    Found_State = PFState.Found;
                Priority_List.Remove(MinF, Node.Pos);
                Calculate_Node(Node);
                CloseList.Add(Node.Pos);

                //Add items to found Node
                if (Found_State == PFState.Found)
                {
                    Node ListNode = List[End_Point];
                    int index;
                    while (ListNode != null)
                    {
                        Path.Add(ListNode.Pos);
                        ListNode = ListNode.Parent;
                    }
                    Path.Reverse();
                }
                return PFState.NotFinished;
            }
            return PFState.NotFinished;
        }


        public void Standard_Pathfind(Point start_Point, Point end_Point, int MaxDistance)
        {
            if (Math.Abs(start_Point.X - end_Point.X) > MaxDistance || Math.Abs(start_Point.Y - end_Point.Y) > MaxDistance)
            {
                Found_State = PFState.Imposable; return;
            }

            Clear_All();
            MapRect = new Rectangle(start_Point, new Point(0, 0));
            MapRect.Inflate(MaxDistance, MaxDistance);
            if (MapRect.X < 0) MapRect.X = 0;
            if (MapRect.X > 999) MapRect.X = 999;

            if (MapRect.Y < 0) MapRect.Y = 0;
            if (MapRect.Y > 999) MapRect.Y = 999;

            AddTo_OpenList(new Node(start_Point, null, 10, 0));
            Start_Point = start_Point;
            End_Point = end_Point;

            if (Tile_Map[End_Point.X, End_Point.Y].Structure != null)
                if (!(Data.structure.Data[Tile_Map[End_Point.X, End_Point.Y].Structure.Type].Walkalble))
                {
                    Found_State = PFState.Imposable; return;
                }
            do
                Pathfind_Step();
            while (Found_State == PFState.NotFinished);
        }

        public void Calculate_Node(Node Node)
        {
            Point Pos = Node.Pos;

            for (int y = Pos.Y - 1; y <= Pos.Y + 1; y++)
            {
                for (int x = Pos.X - 1; x <= Pos.X + 1; x++)
                {
                    if (!(x == Pos.X && y == Pos.Y) && 
                        x >= MapRect.X && x <= MapRect.Right && y >= MapRect.Top && y <= MapRect.Bottom)
                    {
                        //Walkable
                        if (Tile_Map[x, y].Walkable(Data))
                        {
                            if (!CloseList.Contains(new Point(x, y)))
                            {
                                //Standard cost of a tile
                                int G_Add = 10;
                                int H;
                                bool SetNode = true;
                                int dx, dy;

                                if (x != Pos.X && y != Pos.Y)
                                {
                                    //Cost of Diagnals
                                    G_Add = 14;                                    
                                    //Not Walkable
                                    if (!Tile_Map[Pos.X, y].Walkable(Data))
                                        SetNode = false;
                                    if (!Tile_Map[x, Pos.Y].Walkable(Data))
                                        SetNode = false;
                                }


                                if (SetNode == true)
                                {
                                    if (List.ContainsKey(new Point(x, y)))
                                    {
                                        int Old_G = List[new Point(x, y)].G;
                                        int New_G = Node.G + G_Add;
                                        if (New_G < Old_G)
                                        {
                                            Priority_List.Remove(List[new Point(x, y)].F(), new Point(x, y));
                                            List[new Point(x, y)].G = Node.G + G_Add;
                                            List[new Point(x, y)].Parent = Node;
                                            Priority_List.Add(List[new Point(x, y)].F(), new Point(x, y));
                                        }
                                    }
                                    else
                                    {
                                        // H = Math.Abs(x - End_Point.x) * 10 + Math.Abs(y - End_Point.y) * 10
                                        dx = Math.Abs(x - End_Point.X);
                                        dy = Math.Abs(y - End_Point.Y);
                                        H = 10 * (dx + dy);
                                        AddTo_OpenList(new Node(new Point(x, y), Node, Node.G + G_Add, H));
                                    }
                                }
                            }
                        }
                    }
                    else
                        AddTo_ClosedList(new Node(new Point(x, y)));
                }
            }
            AddTo_ClosedList(Node);
        }




        public void Clear_All()
        {
            Found_State = PFState.NotFinished;
            Path.Clear();
            Priority_List.List.Clear();
            List.Clear();
            CloseList.Clear();
        }

    }





    enum PFState : byte
    {
        NotFinished,
        Found,
        Imposable
    }

}
