using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Проект// название проекта
{
    public partial class Form1 : Form // создание класса Форм
    {
        class Point// класс точек
        {
            public double X;// сделали доступной точку Х
            public double Y;// сделали доступной точку Y
            public double Z;// сделали доступной точку Z
            public double H;// сделали доступной точку H

            static public Point Parse(string str) // определяем функцию инициализации точки
            {
                Point point = new Point();// выделяем память

                string[] st = str.Split(' ');// массив из точек

                point.X = Double.Parse(st[0]);// присваивание точкам координаты
                point.Y = Double.Parse(st[1]);// присваивание точкам координаты
                point.Z = Double.Parse(st[2]);// присваивание точкам координаты
                point.H = Double.Parse(st[3]);// присваивание точкам координаты

                return point;
            }
        }

        class Line// класс линий
        {
            public int begin;// начальная точка отрезка
            public int end;// конечная

            static public Line Parse(string str)// определяем функцию инициализации линии
            {
                Line line = new Line();// выделяем память

                string[] st = str.Split(' ');// массив из линий

                line.begin = int.Parse(st[0]);// координаты 
                line.end = int.Parse(st[1]);//

                return line;
            }
        }
        class ploskost //объявляем класс плоскость
        {
            public int A;//доступ к точкам
            public int B;
            public int C;
            static public ploskost Parse(string str)// определяем функцию инициализации плоскости
            {
                ploskost line = new ploskost();//выделяем память

                string[] st = str.Split(' ');// массив из линий

                line.A = int.Parse(st[0]);// координаты 
                line.B = int.Parse(st[1]);//
                line.C = int.Parse(st[2]);
                return line;

            }
        }

        List<Point> tochki = new List<Point>();//лист точек
        List<Line> otrzki = new List<Line>();//лист линий
        List<ploskost> plosko = new List<ploskost>();//лист линий

        Graphics g;// нарисовать

        void ReadFile()//читаем из файла
        {
            StreamReader sr = new StreamReader("C:\\Users\\Admin\\Desktop\\123.txt");//путь к файлу
            
            var str = sr.ReadLine();//объявление переменной
            while (true)//цикл,пока верно
            {

                if (sr.EndOfStream) break;// возвращает значение
                if (str == "*") break;//читать точки до звездочки
                tochki.Add(Point.Parse(str));//читает следующую строчку???
                str = sr.ReadLine();//
            }
            str = sr.ReadLine();
            while (true)//цикл, пока верно
            {

                if (sr.EndOfStream) break;//возвращает значение
                if (str == "*") break;//читать точки до звездочки
                otrzki.Add(Line.Parse(str));
                str = sr.ReadLine();//читать до конца файла
            }
            str = sr.ReadLine();
            while (true)//цикл, пока верно
            {

                plosko.Add(ploskost.Parse(str));
                if (sr.EndOfStream) break;//возвращает значение
                str = sr.ReadLine();//читать до конца файла
            }
        }

        void Vpisat() //функция вписать
        {
            double maxX = tochki[0].X;//присваивание первой точке макс значения
            double minX = tochki[0].X;//присваивание первой точке мин значения
            double maxY = tochki[0].Y;
            double minY = tochki[0].Y;

            for (int i = 0; i < tochki.Count; i++) //цикл по определению максимальной и минимальной точек
            {
                if (tochki[i].X > maxX) maxX = tochki[i].X;//если точка больше, то она становится максимальной
                if (tochki[i].Y > maxY) maxY = tochki[i].Y;

                if (tochki[i].X < minX) minX = tochki[i].X;//если точка меньше, то она становится минимальной
                if (tochki[i].Y < minY) minY = tochki[i].Y;
            }

            double k;//учитываем масштаб и сдвиг

            if (pictureBox1.Height / (maxY - minY) > pictureBox1.Width / (maxX - minX))
                // если отношение высоты пикчербокса к разнице макс и минимальных значений точек больше, чем
               // отношение ширины пикчербокса к разнице его крайних значений, то
                k = pictureBox1.Width / (maxX - minX); // коэффициент масштаба приобретаеть значение 
            // отношения ширины к разнице крайних значений
            else k = pictureBox1.Height / (maxY - minY);// иначе берем результат отношения высоты


            for (int i = 0; i < tochki.Count; i++)//цикл по сдвигу и масштабированию
            {
                tochki[i].X -= minX;// вычитание из точек минимальные значения, чтобы сдвинуть в пикчербокс
                tochki[i].Y -= minY;

                tochki[i].X *= k;// вписываем точки с помощью коэффициента масштаба
                tochki[i].Y *= k;
                tochki[i].Z *= k;
            }
        }

        void Narisovat()//отрисовка линий
        {
            for(int i = 0; i< tochki.Count; i++)// цикл ?????
            {
                tochki[i].X /= tochki[i].H;
                tochki[i].Y /= tochki[i].H;
                tochki[i].Z /= tochki[i].H;
                tochki[i].H = 1;
            }

            g.Clear(Color.White);//цвет фона белый
            if(radioButtonk.Checked) //если выбран элемент, то
            {
                for (int i = 0; i < otrzki.Count; i++)// цикл по отрисовке отрезков
                {
                    g.DrawLine(new Pen(Color.Black, 2),//цвет линий черный
                        (float)tochki[otrzki[i].begin - 1].X, (float)tochki[otrzki[i].begin - 1].Y,//-1, ибо отсчет идет от нуля
                        (float)tochki[otrzki[i].end - 1].X, (float)tochki[otrzki[i].end - 1].Y);
                }
            }
            if (radioButtonR.Checked)//если выбран элемент, то
            {
                for (int i = 0; i < plosko.Count; i++)//цикл по отрисовке плоскостей
                {
                    double ABx = tochki[plosko[i].B - 1].X - tochki[plosko[i].A - 1].X;//отрисовки плоскости АВ
                    double ABy = tochki[plosko[i].B - 1].Y - tochki[plosko[i].A - 1].Y;
                    double ABz = tochki[plosko[i].B - 1].Z - tochki[plosko[i].A - 1].Z;

                    double ACx = tochki[plosko[i].C - 1].X - tochki[plosko[i].A - 1].X;//АС
                    double ACy = tochki[plosko[i].C - 1].Y - tochki[plosko[i].A - 1].Y;
                    double ACz = tochki[plosko[i].C - 1].Z - tochki[plosko[i].A - 1].Z;

                    double Nx = ABy * ACz - ACy * ABz;//вычисляем нормали плоскостей
                    double Ny = -( ABx * ACz - ACx * ABz);
                    double Nz = ABx * ACy - ACx * ABy;

                    double Sp = 0; //скалярное произведение равно 0

                    for (int j = 0; j < tochki.Count; j++)//цикл по скалярному произведению
                    {
                        double tx = tochki[j].X - tochki[plosko[i].A - 1].X;// поиск точки??? 
                        double ty = tochki[j].Y - tochki[plosko[i].A - 1].Y;
                        double tz = tochki[j].Z - tochki[plosko[i].A - 1].Z;

                        Sp = Nx * tx + Ny * ty + Nz * tz; //поиск скалярного произведения 

                        if (Sp > 1 || Sp < -1) break; //если скалярное произведение больше 1 или меньше -1,то
                    }

                    if (Sp>0)//если скалярное произведение больше 1,то 
                    {
                        Nx *= -1; // нормаль меняем в направлении противоположном
                        Ny = -Ny;
                        Nz = -Nz;
                    }

                    if (Nz > 0)//если нормальный вектор больше 0,то
                    {
                        for (int k = 0; k < otrzki.Count; k++)//цикл по отрисовке всех и нужных плоскостей
                        {
                            if ((plosko[i].A == otrzki[k].begin & plosko[i].B == otrzki[k].end) ||
                                (plosko[i].A == otrzki[k].end & plosko[i].B == otrzki[k].begin))
                                //если плоскоть А это начало отрезка и В его конец, или наоборот
                            {
                                g.DrawLine(new Pen(Color.Black, 2),//цвет линий черный
                                      (float)tochki[otrzki[k].begin - 1].X, (float)tochki[otrzki[k].begin - 1].Y,
                                      (float)tochki[otrzki[k].end - 1].X, (float)tochki[otrzki[k].end - 1].Y);
                                //что рисуем
                            }

                            if ((plosko[i].A == otrzki[k].begin & plosko[i].C == otrzki[k].end) ||
                                (plosko[i].A == otrzki[k].end & plosko[i].C == otrzki[k].begin))
                            //если плоскоть А это начало отрезка и С его конец, или наоборот
                            {
                                g.DrawLine(new Pen(Color.Black, 2),//цвет линий черный
                                      (float)tochki[otrzki[k].begin - 1].X, (float)tochki[otrzki[k].begin - 1].Y,
                                      (float)tochki[otrzki[k].end - 1].X, (float)tochki[otrzki[k].end - 1].Y);
                            }

                            if ((plosko[i].C == otrzki[k].begin & plosko[i].B == otrzki[k].end) ||
                                (plosko[i].C == otrzki[k].end & plosko[i].B == otrzki[k].begin))
                            //если плоскоть С это начало отрезка и В его конец, или наоборот
                            {
                                g.DrawLine(new Pen(Color.Black, 2),//цвет линий черный
                                      (float)tochki[otrzki[k].begin - 1].X, (float)tochki[otrzki[k].begin - 1].Y,
                                      (float)tochki[otrzki[k].end - 1].X, (float)tochki[otrzki[k].end - 1].Y);
                            }
                        }
                    }


                }
            }

            pictureBox1.Invalidate(); //перерисовываем
        }

        void Smestit(double x, double y, double z)// смещение
        {
            for (int i = 0; i < tochki.Count; i++)//цикл на смещение точек
            {
                tochki[i].X = tochki[i].X + tochki[i].H * x;//новое положение (координата) точки
                tochki[i].Y = tochki[i].Y + tochki[i].H * y;
                tochki[i].Z = tochki[i].Z + tochki[i].H * z;
            }
        }

        void Mashtab(double x, double y, double z)// масштаб
        {
            for (int i = 0; i < tochki.Count; i++)//цикл на масштабирование
            {
                tochki[i].X = tochki[i].X * x;// задать масштаб в координатах
                tochki[i].Y = tochki[i].Y * y;
                tochki[i].Z = tochki[i].Z * z;
            }
        }
        void Povorotik(double x, double y, double z)//поворот на угол
        {
            double newX, newY, newZ;//создание новых переменных
            for (int i = 0; i < tochki.Count; i++)//цикл для поворота по х  
            {
                newY = tochki[i].Y * Math.Cos(x * Math.PI / 180) - tochki[i].Z * Math.Sin(x * Math.PI / 180);
                // координаты новой точки
                newZ = tochki[i].Y * Math.Sin(x * Math.PI / 180) + tochki[i].Z * Math.Cos(x * Math.PI / 180);
                tochki[i].Y = newY;//присваивание координат
                tochki[i].Z = newZ;
            }
            for(int i = 0; i < tochki.Count; i++)//цикл для поворота по у
            {
                newX = tochki[i].X * Math.Cos(y * Math.PI / 180) + tochki[i].Z * Math.Sin(y * Math.PI / 180);
                newZ = -tochki[i].X * Math.Sin(y * Math.PI / 180) + tochki[i].Z * Math.Cos(y * Math.PI / 180);
                tochki[i].X = newX;
                tochki[i].Z = newZ;
            }
            for(int i = 0; i < tochki.Count; i++)//цикл для поворота по Z
            {
                newY = tochki[i].X * Math.Sin(z * Math.PI / 180) + tochki[i].Y * Math.Cos(z * Math.PI / 180);
                newX = tochki[i].X * Math.Cos(z * Math.PI / 180) - tochki[i].Y * Math.Sin(z * Math.PI / 180);
                tochki[i].Y = newY;
                tochki[i].X = newX;
            }
        }

        void Sdvig(double xy, double xz, double yz, double yx,double zx,double zy)// сдвиг
        {
            for (int i = 0; i < tochki.Count; i++) 
            {
                tochki[i].X = tochki[i].X + tochki[i].Y * xy;// координаты точки при сдвиге на ось                         
            }
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].X = tochki[i].X + tochki[i].Z * xz;
            }
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].Y = tochki[i].Y + tochki[i].X * yx;
            }
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].Y = tochki[i].Y + tochki[i].Z * yz;
            }
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].Z = tochki[i].Z + tochki[i].X * zx;
            }
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].X = tochki[i].Z + tochki[i].Y * zy;
            }
        }
        void OPP(double x, double y, double z)//фокусное расстояние
        {
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].H += tochki[i].X / x;//значение точки с фокусом
                tochki[i].H += tochki[i].Y / y;
                tochki[i].H += tochki[i].Z / z;
            }
        }
        public Form1()// рисование в окне
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); //выделение памяти
            g = Graphics.FromImage(pictureBox1.Image);
            
            ReadFile();// прочитать файл

            Vpisat();//вписать

            Narisovat();//нарисовать
        }

        private void buttonSmestit_Click(object sender, EventArgs e)// кнопка смещения
        {
            Smestit(Double.Parse(textBoxx.Text), 
                Double.Parse(textBoxy.Text), 
                Double.Parse(textBoxz.Text));
            Narisovat();// перерисовать
        }

        private void button1_Click(object sender, EventArgs e)// кнопка масштаба
        {
           Mashtab (Double.Parse(textBoxmashx.Text),
                Double.Parse(textBoxmashy.Text),
                Double.Parse(textBoxmashz.Text));
            Narisovat();// перерисовать
        }

        private void buttonpovorotik_Click(object sender, EventArgs e)// кнопка поворота на угол
        {
            Povorotik(Double.Parse(textBoxyx.Text),
                   Double.Parse(textBoxyy.Text),
                   Double.Parse(textBoxyz.Text));
            Narisovat(); //перерисовать      

        }

        private void buttonvp_Click(object sender, EventArgs e)// кнопка вписывающая изображение в окно
        {
            Vpisat();//

            Narisovat();//

        }

        // при нажатии на кнопку ...
        private void buttonsdvig_Click(object sender, EventArgs e)
        {
            // произвести сдвиг
            Sdvig(Double.Parse(textBoxsxy.Text),
                   Double.Parse(textBoxsxz.Text),
                   Double.Parse(textBoxsyx.Text),
                   Double.Parse(textBoxsyz.Text),
                   Double.Parse(textBoxszx.Text),
                   Double.Parse(textBoxszy.Text));
            Narisovat(); // перерисовать сцену        

        }

        private void buttonopp_Click(object sender, EventArgs e)// кнопка фокуса
        {
            OPP(Double.Parse(textBoxoppx.Text),
                   Double.Parse(textBoxoppy.Text),
                   Double.Parse(textBoxoppz.Text));
            Narisovat();//перерисовать
        }
    }
}
 