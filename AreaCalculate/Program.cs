﻿using System;
using System.Collections;
using System.Text;

namespace AreaCalculate
{

    #region DataMaker 实验一

    /// <summary>
    /// 数据生成器
    /// </summary>
    public class DataMaker
    {
        /// <summary>
        /// Y坐标的最大值
        /// </summary>
        public static int MAX = 100;

        /// <summary>
        /// 生成数据
        /// </summary>
        /// <param name="seed">一位种子</param>
        /// <param name="count">生成数据的数量</param>
        /// <returns>随机出来的数组</returns>
        public static ArrayList Making(int seed, int count)
        {
            Random random = new Random(seed);
            ArrayList list = new ArrayList();
            while (--count >= 0)
            {
                list.Add((float)(random.Next(MAX - 1) + random.NextDouble()));
            }
            return list;
        }

        /// <summary>
        /// 根据X坐标生成Y坐标
        /// </summary>
        /// <param name="seed">X坐标</param>
        /// <returns>随机生成的Y坐标</returns>
        public static ArrayList Making(int[] seed)
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < seed.Length; ++i)
            {
                Random random = new Random(seed[i]);
                //这里随机不出0。。。
                list.Add(random.Next(MAX));
            }
            return list;
        }

        /// <summary>
        /// 根据X坐标生成Y坐标
        /// </summary>
        /// <param name="seed">X坐标</param>
        /// <returns>随机生成的Y坐标</returns>
        public static ArrayList Making(float[] seed)
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < seed.Length; ++i)
            {
                Random random = new Random((int)seed[i]);
                list.Add((float)(random.NextDouble() + random.Next(MAX - 1)));
            }
            return list;
        }
    }


    #endregion DataMaker


    #region 实验二 图形面积计算

    /// <summary>
    /// 计算面积的接口
    /// </summary>
    interface IShape
    {
        Double Arae();
    }

    public class Coordinate : IComparable<Coordinate>
    {
        public float x;
        public float y;

        public Coordinate(float v1, float v2)
        {
            this.x = v1;
            this.y = v2;
        }


        /// <summary>
        /// 按x从小到大，如果相同则按y从大到小
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Coordinate other)
        {
            if (null == other)
            {
                return 0;//空值比较大，返回1
            }
            int result = other.x.CompareTo(this.x);
            //return this.Id.CompareTo(other.Id);//升序
            if (result == 0)
            {
                result = this.y.CompareTo(other.y);
            }
            return result;//降序
        }
    }

    /// <summary>
    /// 三角形
    /// </summary>
    class Triangle : IShape
    {
        readonly float height;
        readonly float width;

        public Triangle(float height, float width)
        {
            this.height = height;
            this.width = width;
        }

        public Double Arae()
        {
            return height * width / 2;
        }
    }

    /// <summary>
    /// 圆
    /// </summary>
    class Circle : IShape
    {
        static readonly Double PI = 3.14;
        Double r;

        public Circle(Double r)
        {
            this.r = r;
        }

        public Double Arae()
        {
            return 2*PI*r*r;
        }
    }

    /// <summary>
    /// 矩形
    /// </summary>
    class Rectangle : IShape
    {
        Double height;
        Double width;

        public Rectangle(Double height,Double width)
        {
            this.height = height;
            this.width = width;
        }

        public Double Arae()
        {
            return height * width;
        }
    }

    /// <summary>
    /// 不规则图形
    /// </summary>
    public class UShape : IShape
    {
        //X坐标
        int[] XList;
        //Y坐标
        int[] YList;

        public Coordinate[] coordinates;

        public UShape(float[] list)
        {
            ArrayList x = new ArrayList(list);
            ArrayList y = DataMaker.Making(list);
            ArrayList t = new ArrayList();

            for (int i = 0;i< x.Count; ++i)
            {
                t.Add(new Coordinate((float)x[i], (float)y[i]));
            }

            for (int i = 0; i < t.Count; ++i)
            {
                x[i] = (int)(float)x[i];
            }
            for (int i = 0; i < t.Count; ++i)
            {
                y[i] = (int)(float)y[i];
            }

            for (int i = 0;i < t.Count; ++i)
            {
                for (int j = i + 1; j < t.Count; ++j)
                {
                    if (((Coordinate)t[i]).x == ((Coordinate)t[j]).x && ((Coordinate)t[i]).y == ((Coordinate)t[j]).y)
                    {
                        t.Remove(i);
                        continue;
                    }
                }
            }

            //判断数组中是否有重复的数据，并过滤
            for (int i = 0; i < x.Count; ++i)
            {
                for (int j = i+1; j < x.Count; ++j)
                {
                    if ((int)x[i] == (int)x[j] && (int)y[i] == (int)y[j])
                    {
                        x.RemoveAt(j);
                        y.RemoveAt(j);
                        --j;
                        continue;
                    }
                }
            }
            //这里的排序是为了打印图形而准备的，打印的时候插入字符*需要从小到大插
            x.Sort();
            y.Sort();

            XList = ((int[])x.ToArray(typeof(int)));
            YList = ((int[])y.ToArray(typeof(int)));

            coordinates = (Coordinate[])t.ToArray(typeof(Coordinate));
            Array.Sort(coordinates);
        }

        public Double Arae()
        {
            Double result = 0;
            for (int i = 0; i < XList.Length - 1; ++i)
            {
                float h = XList[i + 1] - XList[i];
                result += (YList[i] + YList[i + 1]) / 2.0 * h;
            }
            return result;
        }

        /// <summary>
        /// 将不规则的图形打印的函数
        /// </summary>
        public void Print()
        {
            //动态改变的字符串，用于打印*号
            StringBuilder s = new StringBuilder();
            //每一个坐标的空格
            String tab = "   ";
            //初始化打印对应Y轴的X轴的字符串,以X轴的最大值为界
            for (int i = 0; i < GetMax(XList); ++i)
            {
                s.Append(tab);
            }
            ////储存原始的空白字符串
            String temp = s.ToString();
            //打印Y轴,从1打印到最大值
            for (int i = DataMaker.MAX; i >= 1; i -= 1)
            {
                char c = '|';
                for (int j = 0; j < YList.Length; ++j)
                {
                    //如果相等就赋值
                    if (YList[j] == i)
                    {
                        //X为0的情况
                        if (XList[j] != 0)
                        {
                            s.Insert(tab.Length * ((int)XList[j] - 1), "  *");
                        }
                        else
                        {
                            c = '*';
                        }
                    }
                }
                Console.Write("{0,3}{1}", i, c);
                Console.Write(s);
                s = new StringBuilder(temp);
                Console.WriteLine();
            }

            ArrayList zeroList = new ArrayList();

            //找出Y坐标为0的点,并将X值储存起来
            for (int i = 0; i < YList.Length; ++i)
            {
                if (YList[i] == 0)
                {
                    zeroList.Add(XList[i]);
                }
            }
            if (zeroList.Count > 0 && (int)zeroList[0] == 0)
            {
                Console.Write(tab + "*");
            }
            else
            {
                Console.Write(tab + "|");
            }

            //打印X轴
            for (int i = 1; i <= GetMax(XList); i += 1)
            {
                char c = '|';
                foreach (int j in zeroList)
                {
                    if (j == i)
                    {
                        c = '*';
                    }
                }
                Console.Write("--{0}", c);
            }

            Console.WriteLine();
            Console.Write(" ");
            //打印X轴数字
            for (int i = 0; i <= GetMax(XList); i += 1)
            {
                Console.Write("{0,3}", i);
            }
            Console.WriteLine();
        }

        private int GetMax(int[] list)
        {
            float max = -1;
            foreach (float i in list)
            {
                if (i > max) { max = i; }
            }
            return (int)max;
        }

        private int GetMin(int[] list)
        {
            float min = list[0];
            foreach (float i in list)
            {
                if (i < min) { min = i; }
            }
            return (int)min;
        }

        public void PrintC()
        {
            foreach (Coordinate c in coordinates)
            {
                Console.Write("({0},{1}) ",c.x,c.y);
            }
        }

    }

    #endregion

    class MainClass
    {
        public static void Main(string[] args)
        {
            ArrayList array = DataMaker.Making((int)DateTime.Now.Ticks, 30);
            foreach (var i in array)
            {
                Console.Write("{0,2}",i);
            }

            while (true)
            {
                float[] x = ((float[])DataMaker.Making((int)DateTime.Now.Ticks, 30).ToArray(typeof(float))) ;
                UShape u = new UShape(x);
                u.Print();
                Console.WriteLine();
                Console.WriteLine("图形的面积是：{0}", u.Arae());
                u.PrintC();
                Console.ReadKey();
            }

            Console.ReadKey();
        }
    }


}
