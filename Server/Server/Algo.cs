using System;
using System.Collections.Generic;

namespace Server
{
    internal class Algo
    {
        public List<Data.InfoPlane> InfosPlane { get; set; } = new List<Data.InfoPlane>();
        public List<Data.InfoPlane> Work()
        {
            return InfosPlane;
        }

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

        public sam[] masSam = new sam[col]; //массив самолетов
        public sam[] masSamReady = new sam[col];// массив выгодных самолетов
        private dom an;// ангар
        private Random ran = new Random();
        private int combo = 0; // кол-во возможных комбинаций
        private int sum = 0;// сумма выгоды
        private int newSum = 0;// промежуточная сумма

        //структура точек возможного размещения
        public struct point
        {
            public int w;
            public int l;
            public int dno;
            public int idSam;
        };
        point[] stp = new point[1];


        //обнуление и заполнение переменных
        public Algo(List<Data.InfoPlane> infosPlane)
        {
            InfosPlane.Add(infosPlane[1]);
            InfosPlane.Add(infosPlane[3]);

            /*
            stp[0].w = 0;
            stp[0].l = 0;
            stp[0].dno = an.h;
            int r = 0;
            //размеры аэропорта
            an.h = 400;//ширина
            an.l = 450;//длинна
            an.w = 30;//высота

            //заполнение массива случайными самолетами
            for (int i = 0; i < masSam.Length; i++)
            {
                masSam[i].id = i;
                r = ran.Next(0, 70);
                masSam[i].w = 20 + r;
                r = ran.Next(0, 70);
                masSam[i].l = 35 + r;
                r = ran.Next(0, 5);
                masSam[i].hd = 1 + r;
                r = ran.Next(0, 10);
                masSam[i].h = masSam[i].hd + r;

                r = ran.Next(500, 700);
                masSam[i].s = r;
            }
            */
        }

        //алгоритм поиска
        public void sort(sam[] revolver, dom Ang, point[] t, int schet)
        {
            if (revolver.Length < 1)
            {
                combo++;
                if (newSum > sum)
                {
                    sum = newSum;
                    newSum = 0;
                }
                return;
            }

            foreach (point tn in t)
            {
                bool ok = true;
                sam buf = revolver[0];

                buf.aw = tn.w;
                buf.al = tn.l;

                if (buf.h > Ang.h)
                {
                    ok = false;
                }

                if (buf.w + tn.w > Ang.w)
                {
                    ok = false;
                }

                if (buf.l + tn.l > Ang.l)
                {
                    ok = false;
                }

                if (Buter(masSamReady, buf, schet))
                {
                    ok = false;
                }

                if (ok)
                {

                    point[] tnew = new point[t.Length + 2];
                    tnew[t.Length].w = buf.w + tn.w;
                    tnew[t.Length].l = tn.l;
                    tnew[t.Length - 1].l = buf.l + tn.l;
                    tnew[t.Length - 1].w = tn.w;
                    newSum += buf.s;
                    masSamReady[schet] = buf;
                    schet++;

                    sam[] newRevolver = new sam[revolver.Length - 1];
                    for (int i = 1; i < revolver.Length; i++)
                    {
                        newRevolver[i - 1] = revolver[i];
                    }

                    sort(newRevolver, Ang, tnew, schet);
                }
                else
                {
                    sam[] newRevolver = new sam[revolver.Length - 1];
                    for (int i = 1; i < revolver.Length; i++)
                    {
                        newRevolver[i - 1] = revolver[i];
                    }

                    sort(newRevolver, Ang, t, schet);
                }
            }

        }
        //проверка наложения друг на друга
        public bool Buter(sam[] masProv, sam prov, int schet)
        {
            bool peresechenir = false;
            int ax1 = prov.aw;
            int ay1 = prov.al;
            int ax2 = prov.w;
            int ay2 = prov.l;

            ax2 = ax1 + ax2;
            ay2 = ay1 + ay2;

            for (int i = 0; i < schet; i++)
            {
                if (schet > 0)
                {
                    int bx1 = masProv[i].aw;
                    int by1 = masProv[i].al;
                    int bx2 = masProv[i].w;
                    int by2 = masProv[i].l;

                    bx2 = bx1 + bx2;
                    by2 = by1 + by2;

                    bool s1 = ((ax1 >= bx1) && (ax1 <= bx2)) || ((ax2 >= bx1 && ax2 <= bx2));
                    bool s2 = ((ay1 >= by1) && (ay1 <= by2)) || ((ay2 >= by1 && ay2 <= by2));
                    bool s3 = ((bx1 >= ax1) && (bx1 <= ax2)) || ((bx2 >= ax1 && bx2 <= ax2));
                    bool s4 = ((by1 >= ay1) && (by1 <= ay2)) || ((by2 >= ay1 && by2 <= ay2));

                    peresechenir = (s1 && s2) || (s3 && s4) || (s2 && s3) || (s1 && s4);
                }
            }
            return peresechenir;
        }


    }
}