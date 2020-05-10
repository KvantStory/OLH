using System;
using System.Collections.Generic;

namespace Server
{
    static class Algo
    {
        static public List<Data.InfoPlane> InfosPlane { get; set; } = new List<Data.InfoPlane>();

        public struct sam
        {
            public int id; // id в базе
            public string name;// модель самолета
            public int nom_dogovora;//номер договора
            public int s;//прибыль
            public int time; // время ремонта
            private DateTime srok; // крайний срок

            public int w;//ширина
            public int l;//длинна
            public int h;//высота
            public int hd;//высота до дна

            public int aw;//положение по ширине в ангаре
            public int al;//положение по длине в ангаре

            public bool ready;
            public bool g90; // поворот на 90

        };

        public struct dom
        {
            public int w;//ширина
            public int l;//длинна
            public int h;//высота
            public int arenda;//аренда ангара в сутки
        };

        private const int col = 15; //количество самолетов

        static public sam[] masSam = new sam[col]; //массив самолетов
        static public sam[] masSamReady = new sam[col];// массив выгодных самолетов
        static private dom an;// ангар
        static private Random ran = new Random();
        static private int combo = 0; // кол-во возможных комбинаций
        static public int sum = 0;// сумма выгоды
        static public int newSum = 0;// промежуточная сумма
                                      //новые

        static private long iter = 0;
        static private long stopIter = 0;


        //структура точек возможного размещения
        public struct point
        {
            public int w;
            public int l;
            public int dno;
            public int idSam;
        };

        static private point[] stp = new point[1];


        //обнуление и заполнение переменных

        //алгоритм поиска
        static public void sort(int look, point[] t, sam[] masGood, long kof, int newSum1)
        {
            iter++;
            //if (iter > stopIter) return;
            if (combo > 10000000)
            {
                return;
            }

            if (look < 1)
            {
                combo++;

                for (int i = 0; i < masGood.Length; i++)
                {
                    newSum = newSum + masGood[i].s;
                }

                if (newSum > sum)
                {
                    sum = newSum;
                    //if (combo > kof) return;
                    masSamReady = new sam[masGood.Length];
                    for (int k = 0; k < masGood.Length; k++)
                    {
                        masSamReady[k] = masGood[k];
                    }
                    newSum = 0;

                }
                newSum = 0;
                return;
            }

            for (int i = masSam.Length - look; i < masSam.Length; i++)
            {
                for (int v = 0; v < t.Length; v++)
                {


                    if ((masSam[i].h > an.h) || (masSam[i].w + t[v].w > an.w) || (masSam[i].l + t[v].l > an.l))
                    {
                        look--;
                        sort(look, t, masGood, kof, newSum);
                        look++;

                    }
                    else
                    {
                        sam buf = new sam();
                        buf = masSam[i];
                        buf.aw = t[v].w;
                        buf.al = t[v].l;

                        if (!Buter(masGood, buf))
                        {
                            point[] tnew = new point[t.Length + 2];

                            byte pl = 0;
                            for (int y = 0; y < t.Length; y++)
                            {
                                if (y == v) { pl = 1; continue; }
                                tnew[y - pl] = t[y];
                            }
                            tnew[tnew.Length - 3].w = buf.w + 1 + buf.aw;
                            tnew[tnew.Length - 3].l = buf.l + buf.al + 1;
                            tnew[tnew.Length - 2].w = buf.w + 1 + buf.aw;
                            tnew[tnew.Length - 2].l = buf.al;
                            tnew[tnew.Length - 1].l = buf.l + buf.al + 1;
                            tnew[tnew.Length - 1].w = buf.aw;

                            sam[] newMasGood = new sam[masGood.Length + 1];

                            // if(masGood.Length > 0)
                            for (int k = 0; k < masGood.Length; k++)
                            {
                                newMasGood[k] = masGood[k];
                            }

                            newMasGood[newMasGood.Length - 1] = buf;
                            //newSum = newSum + buf.s;


                            look--;
                            sort(look, tnew, newMasGood, kof, newSum);
                            look++;
                            //newSum = newSum - buf.s;
                            // break;

                        }
                        else
                        {
                            look--;
                            sort(look, t, masGood, kof, newSum);
                            look++;
                        }
                    }

                }
            }
        }
        //проверка наложения друг на друга
        static public bool Buter(sam[] masProv, sam prov)
        {
            bool peresechenir = false;
            int ax1 = prov.aw;
            int ay1 = prov.al;
            int ax2 = prov.aw + prov.w;
            int ay2 = prov.al + prov.l;


            for (int i = 0; i < masProv.Length; i++)
            {
                if (true)
                {
                    int bx1 = masProv[i].aw;
                    int by1 = masProv[i].al;
                    int bx2 = masProv[i].aw + masProv[i].w;
                    int by2 = masProv[i].al + masProv[i].l;

                    bool s1 = ((ax1 > bx1) && (ax1 < bx2)) || ((ax2 > bx1 && ax2 < bx2));
                    bool s2 = ((ay1 > by1) && (ay1 < by2)) || ((ay2 > by1 && ay2 < by2));
                    bool s3 = ((bx1 > ax1) && (bx1 < ax2)) || ((bx2 > ax1 && bx2 < ax2));
                    bool s4 = ((by1 > ay1) && (by1 < ay2)) || ((by2 > ay1 && by2 < ay2));

                    //peresechenir = !((ay1 > by2) || (ay2 < by1) || (ax2 < bx1) || (ax1 > bx2));

                    peresechenir = (s1 && s2) || (s3 && s4) || (s2 && s3) || (s1 || s4);
                    peresechenir = (s1 && s2) || (s3 && s4) || (s2 && s3) || (s1 || s4);
                    if (peresechenir)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }

}