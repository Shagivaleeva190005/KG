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
    public partial class Form1 : Form
    {
        // класс точек
        class Point
        {
            //объявили абциссу, ординату и аппликату точки
            public double X;
            public double Y;
            public double Z;
            public double H;

            static public Point Parse(string str) //создание точки из строки с координатами
            {
                Point point = new Point();// создаем точку

                string[] st = str.Split(' ');// массив строк из координат

                //элементы массива преобразуем в число и присваиваем к координатам
                point.X = Double.Parse(st[0]);
                point.Y = Double.Parse(st[1]);
                point.Z = Double.Parse(st[2]);
                point.H = Double.Parse(st[3]);

                return point;//возвращаем созданную точку
            }
        }

        // класс линий
        class Line
        {            
            public int begin;//номер точки начала
            public int end;//номер точки конца

            static public Line Parse(string str)//создание линии из строки с номерами точек
            {
                Line line = new Line();//создаем объект класса линий

                string[] st = str.Split(' ');// массив строк из точек начала и конца

                //элементы строки массива преобразуем в число и присваиваем к номерам точек начала и конца
                line.begin = int.Parse(st[0]);
                line.end = int.Parse(st[1]);

                return line;//возвращаем созданную линию
            }
        }

        //объявляем класс плоскость
        class ploskost
        { 
            //номера точек
            public int A;
            public int B;
            public int C;

            static public ploskost Parse(string str)//создание плоскости из строки с номерами граничных точек
            {
                ploskost line = new ploskost();//создаем объект класса плоскость

                string[] st = str.Split(' ');// массив строк из точек

                //элементы строки преобразуем в число и присваиваем номерам точек
                line.A = int.Parse(st[0]);
                line.B = int.Parse(st[1]);
                line.C = int.Parse(st[2]);

                return line;
            }
        }

        List<Point> tochki = new List<Point>();//лист точек
        List<Line> otrzki = new List<Line>();//лист линий
        List<ploskost> plosko = new List<ploskost>();//лист плоскостей

        Graphics g;// нарисовать

        //читаем из файла
        void ReadFile()
        {
            StreamReader sr = new StreamReader(@"..\..\123.txt");//путь к файлу
            
            var str = sr.ReadLine();//читаем строку
            //чтение точек
            while (true)//цикл,пока верно
            {
                if (sr.EndOfStream) break;// если конец файла, прервать цикл
                if (str == "*") break;//если *, прервать цикл
                tochki.Add(Point.Parse(str));//переменную str в точку и добавить в лист
                str = sr.ReadLine();// прочитать следующую строчку из файла
            }
            str = sr.ReadLine();// прочитать следующую строчку из файла
           
            //чтение отрезков
            while (true)//цикл, пока верно
            {
                if (sr.EndOfStream) break;// если конец файла, прервать цикл
                if (str == "*") break;//если *, прервать цикл
                otrzki.Add(Line.Parse(str));//переменную str в точки начала и конца и добавить в лист
                str = sr.ReadLine();// прочитать следующую строчку из файла
            }
            str = sr.ReadLine();// прочитать следующую строчку из файла
            
            //чтение плоскостей
            while (true)//цикл, пока верно
            {
                plosko.Add(ploskost.Parse(str));//переменную str в номера точек и добавить в лист
                if (sr.EndOfStream) break;// если конец файла, прервать цикл
                str = sr.ReadLine();// прочитать следующую строчку из файла
            }
        }

        //функция вписать
        void Vpisat()
        {
            double maxX = tochki[0].X;//присваивание макс значения координат первой точки
            double minX = tochki[0].X;//присваивание мин значения координат первой точки
            double maxY = tochki[0].Y;
            double minY = tochki[0].Y;

            for (int i = 0; i < tochki.Count; i++) //перебираем все точки
            {
                if (tochki[i].X > maxX) maxX = tochki[i].X;//если абцисса точки больше макс, то макс значение меняется на абциссу точки
                if (tochki[i].Y > maxY) maxY = tochki[i].Y;//если ордината точки больше макс, то макс значение меняется на ординату точки

                if (tochki[i].X < minX) minX = tochki[i].X;//если абцисса точки меньше мин, то мин значение меняется на абциссу точки
                if (tochki[i].Y < minY) minY = tochki[i].Y;//если ордината точки меньше мин, то мин значение меняется на ординату точки
            }

            double k;//коэффициент масштабирования
            //находим минимальное значенние отношения размеров сцены и пикчербокса, присваиваем это значение коэфф масшт
            if (pictureBox1.Height / (maxY - minY) > pictureBox1.Width / (maxX - minX))
                k = pictureBox1.Width / (maxX - minX); 
            else k = pictureBox1.Height / (maxY - minY);// иначе берем результат отношения высоты

            for (int i = 0; i < tochki.Count; i++)//перебираем точки
            {
                // смещаем точку
                tochki[i].X -= minX;
                tochki[i].Y -= minY;
                
                // масштабируем
                tochki[i].X *= k;
                tochki[i].Y *= k;
                tochki[i].Z *= k;
            }
        }

        //отрисовка линий
        void Narisovat()
        {   
            //перевод из однородных в декартовые
            for(int i = 0; i< tochki.Count; i++)//перебираем точек  
            {
                tochki[i].X /= tochki[i].H;
                tochki[i].Y /= tochki[i].H;
                tochki[i].Z /= tochki[i].H;
                tochki[i].H = 1;
            }

            g.Clear(Color.White);//очистка изображения и заливка белым цветом

            if(radioButtonk.Checked) //если выбран каркас
            {
                for (int i = 0; i < otrzki.Count; i++)//для каждого отрезка
                {   
                    //нарисовать соответствующую отрезку черную линию
                    g.DrawLine(new Pen(Color.Black, 2),
                        (float)tochki[otrzki[i].begin - 1].X, (float)tochki[otrzki[i].begin - 1].Y,
                        (float)tochki[otrzki[i].end - 1].X, (float)tochki[otrzki[i].end - 1].Y);
                }
            }
            if (radioButtonR.Checked)//если выбран алгоритм Робертса
            {
                for (int i = 0; i < plosko.Count; i++)//перебор плоскостей
                {   
                    //проекция границ плоскости на оси
                    double ABx = tochki[plosko[i].B - 1].X - tochki[plosko[i].A - 1].X;
                    double ABy = tochki[plosko[i].B - 1].Y - tochki[plosko[i].A - 1].Y;
                    double ABz = tochki[plosko[i].B - 1].Z - tochki[plosko[i].A - 1].Z;

                    double ACx = tochki[plosko[i].C - 1].X - tochki[plosko[i].A - 1].X;
                    double ACy = tochki[plosko[i].C - 1].Y - tochki[plosko[i].A - 1].Y;
                    double ACz = tochki[plosko[i].C - 1].Z - tochki[plosko[i].A - 1].Z;
                    
                    //координаты нормали
                    double Nx = ABy * ACz - ACy * ABz;
                    double Ny = -( ABx * ACz - ACx * ABz);
                    double Nz = ABx * ACy - ACx * ABy;

                    double Sp = 0; //скалярное произведение равно 0

                    for (int j = 0; j < tochki.Count; j++)//цперебираем все точки
                    {  
                        //проекция отрезка от точки плоскости до текущей точки
                        double tx = tochki[j].X - tochki[plosko[i].A - 1].X; 
                        double ty = tochki[j].Y - tochki[plosko[i].A - 1].Y;
                        double tz = tochki[j].Z - tochki[plosko[i].A - 1].Z;
                       
                        //скалярное произведение нормали к плоскости отрезка
                        Sp = Nx * tx + Ny * ty + Nz * tz; 
                       
                        //если текущая точка не лежит в плоскости, выйти из цикла
                        if (Sp > 1 || Sp < -1) break; 
                    }

                    //если нашлась точка, которая лежит со стороны нормали
                    if (Sp>0)
                    {   
                        //сменить направление нормали на противоположное
                        Nx *= -1; 
                        Ny = -Ny;
                        Nz = -Nz;
                    }

                    //если нормаль направлена в сторону наблюдателя
                    if (Nz > 0)
                    {
                        for (int k = 0; k < otrzki.Count; k++)//перебираем все отрезки
                        {
                            if ((plosko[i].A == otrzki[k].begin & plosko[i].B == otrzki[k].end) ||
                                (plosko[i].A == otrzki[k].end & plosko[i].B == otrzki[k].begin))
                                //если точка А это начало отрезка и В его конец, или наоборот
                            {
                                g.DrawLine(new Pen(Color.Black, 2),//цвет линий черный
                                      (float)tochki[otrzki[k].begin - 1].X, (float)tochki[otrzki[k].begin - 1].Y,
                                      (float)tochki[otrzki[k].end - 1].X, (float)tochki[otrzki[k].end - 1].Y);
                                //что рисуем
                            }

                            if ((plosko[i].A == otrzki[k].begin & plosko[i].C == otrzki[k].end) ||
                                (plosko[i].A == otrzki[k].end & plosko[i].C == otrzki[k].begin))
                                //если точка А это начало отрезка и С его конец, или наоборот
                            {
                                g.DrawLine(new Pen(Color.Black, 2),//цвет линий черный
                                      (float)tochki[otrzki[k].begin - 1].X, (float)tochki[otrzki[k].begin - 1].Y,
                                      (float)tochki[otrzki[k].end - 1].X, (float)tochki[otrzki[k].end - 1].Y);
                            }

                            if ((plosko[i].C == otrzki[k].begin & plosko[i].B == otrzki[k].end) ||
                                (plosko[i].C == otrzki[k].end & plosko[i].B == otrzki[k].begin))
                                //если точка С это начало отрезка и В его конец, или наоборот
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

        // функция на смещение
        void Smestit(double x, double y, double z)
        {
            for (int i = 0; i < tochki.Count; i++)//перебираем точек
            {
                tochki[i].X = tochki[i].X + tochki[i].H * x;//новые координаты смещенной точки x по x
                tochki[i].Y = tochki[i].Y + tochki[i].H * y;//новые координаты смещенной точки у по у
                tochki[i].Z = tochki[i].Z + tochki[i].H * z;//новые координаты смещенной точки z по z
            }
        }

        // функция на масштаб
        void Mashtab(double x, double y, double z)
        {
            for (int i = 0; i < tochki.Count; i++)//перебираем точки
            {
                tochki[i].X = tochki[i].X * x;// масштабируем х на значение по х
                tochki[i].Y = tochki[i].Y * y;// масштабируем y на значение по y
                tochki[i].Z = tochki[i].Z * z;// масштабируем z на значение по z
            }
        }

        //функция поворота на угол
        void Povorotik(double x, double y, double z)
        {
            double newX, newY, newZ;//объявляем новые переменные

            //поворот по ох
            for (int i = 0; i < tochki.Count; i++)//перебираем точки 
            {   
                // координаты новой точки у
                newY = tochki[i].Y * Math.Cos(x * Math.PI / 180) - tochki[i].Z * Math.Sin(x * Math.PI / 180);
                // координаты новой точки z
                newZ = tochki[i].Y * Math.Sin(x * Math.PI / 180) + tochki[i].Z * Math.Cos(x * Math.PI / 180);
                //новые координаты у и z
                tochki[i].Y = newY;
                tochki[i].Z = newZ;
            }

            //поворот по оу
            for(int i = 0; i < tochki.Count; i++)//перебираем точки
            {   
                // координаты новой точки x
                newX = tochki[i].X * Math.Cos(y * Math.PI / 180) + tochki[i].Z * Math.Sin(y * Math.PI / 180);
                // координаты новой точки z
                newZ = -tochki[i].X * Math.Sin(y * Math.PI / 180) + tochki[i].Z * Math.Cos(y * Math.PI / 180);
                //новые координаты х и z
                tochki[i].X = newX;
                tochki[i].Z = newZ;
            }

            //поворот по оZ
            for(int i = 0; i < tochki.Count; i++)//перебираем точки                
            {   
                // координаты новой точки у
                newY = tochki[i].X * Math.Sin(z * Math.PI / 180) + tochki[i].Y * Math.Cos(z * Math.PI / 180);
                // координаты новой точки x
                newX = tochki[i].X * Math.Cos(z * Math.PI / 180) - tochki[i].Y * Math.Sin(z * Math.PI / 180);
                //новые координаты у и х
                tochki[i].Y = newY;
                tochki[i].X = newX;
            }
        }

        // параллельный перенос
        void Sdvig(double xy, double xz, double yz, double yx,double zx,double zy)
        {  
            //х по у
            for (int i = 0; i < tochki.Count; i++) //перебор точек
            {
                tochki[i].X = tochki[i].X + tochki[i].Y * xy;                         
            }

            //х по z
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].X = tochki[i].X + tochki[i].Z * xz;
            }

            //у по х
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].Y = tochki[i].Y + tochki[i].X * yx;
            }

            //у по z
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].Y = tochki[i].Y + tochki[i].Z * yz;
            }

            //z по х
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].Z = tochki[i].Z + tochki[i].X * zx;
            }

            //z по у
            for (int i = 0; i < tochki.Count; i++)
            {
                tochki[i].X = tochki[i].Z + tochki[i].Y * zy;
            }
        }

        //одноточечное проективное преобразование
        void OPP(double x, double y, double z)
        {
            for (int i = 0; i < tochki.Count; i++)//перебор точек
            {   //три опп по трем осям
                tochki[i].H += tochki[i].X / x;
                tochki[i].H += tochki[i].Y / y;
                tochki[i].H += tochki[i].Z / z;
            }
        }

        //создание изображения в пикчербокс
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); // создание изобржения и помещение его в пикчербокс

            g = Graphics.FromImage(pictureBox1.Image);//создание графики из изображения
            
            ReadFile();// прочитать файл

            Vpisat();//вписать

            Narisovat();//нарисовать сцену
        }

        // при нажатии на кнопку смещения
        private void buttonSmestit_Click(object sender, EventArgs e)
        {  
            //преобразовываем строку в число и смещаем на это значение
            Smestit(Double.Parse(textBoxx.Text), 
                Double.Parse(textBoxy.Text), 
                Double.Parse(textBoxz.Text));
            Narisovat();// перерисовать сцену
        }

        //при нажатии на кнопку масштаба
        private void button1_Click(object sender, EventArgs e)
        { 
            // преобразовываем строку в число и масштабируем на это число         
            Mashtab(Double.Parse(textBoxmashx.Text), 
                Double.Parse(textBoxmashy.Text),
                Double.Parse(textBoxmashz.Text));
            Narisovat();// перерисовать сцену
        }

        //рпи нажатии на кнопку воворота на угол
        private void buttonpovorotik_Click(object sender, EventArgs e)
        {
            //преобразовываем строку в число и поворачиваем на это значение угла
            Povorotik(Double.Parse(textBoxyx.Text),
                   Double.Parse(textBoxyy.Text),
                   Double.Parse(textBoxyz.Text));
            Narisovat(); //перерисовать сцену 
        }

        // при нажатии на кнопку вписать 
        private void buttonvp_Click(object sender, EventArgs e)
        {
            Vpisat(); //вписать изображение в пикчербокс
            Narisovat();// перерисовать сцену
        }

        // при нажатии на кнопку сдвига
        private void buttonsdvig_Click(object sender, EventArgs e)
        {      
            //произвести сдвиг      
            Sdvig(Double.Parse(textBoxsxy.Text),
                   Double.Parse(textBoxsxz.Text),
                   Double.Parse(textBoxsyx.Text),
                   Double.Parse(textBoxsyz.Text),
                   Double.Parse(textBoxszx.Text),
                   Double.Parse(textBoxszy.Text));
            Narisovat(); // перерисовать сцену        
        }

        // при нажатии на кнопку ОПП
        private void buttonopp_Click(object sender, EventArgs e)
        {
            //производится одноточечное проективное преобразование    
            OPP(Double.Parse(textBoxoppx.Text), //значение по х берется из текстбокса
                   Double.Parse(textBoxoppy.Text),//значение по у берется из текстбокса
                   Double.Parse(textBoxoppz.Text));//значение по z берется из текстбокса
            Narisovat();//перерисовать сцену
        }
    }
}
 