using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class Snowflake
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Speed { get; set; }
    public int Size { get; set; }

    public Snowflake(float x, float y, float speed, int size)
        ///<summary>
        /// Переменные
        ///</summеry>
    {
        X = x;
        Y = y;
        Speed = speed;
        Size = size;
    }
}

public class SnowfallScreensaver : Form
///<summary>
        /// Создание эффекта снегопада
        ///</summеry>
{
    private List<Snowflake> snowflakes;
    private Random random;
    private Timer timer;
    private Image background;

    public SnowfallScreensaver()
    ///<summary>
        /// Устанавливаем свойства окна
        ///</summеry>
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.BackColor = Color.Black;
        this.DoubleBuffered = true;

        background = Image.FromFile("Shegpad.jpg");
        ///<summary>
        /// Загрузка фонового изображения
        ///</summеry>

        /// Инициализация списка снежинок
        snowflakes = new List<Snowflake>();
        random = new Random();

        for (int i = 0; i < 1000; i++)
        ///<summary>
        /// Создаем начальные снежинки
        ///</summеry>
        {
            AddSnowflake();
        }

        /// Настройка таймера
        timer = new Timer();
        timer.Interval = 16; /// ~60 FPS
        timer.Tick += Timer_Tick;
        timer.Start();

        this.KeyDown += (s, e) =>
        ///<summary>
        /// Обработка клавиш
        ///</summеry>
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        };
    }

    private void AddSnowflake()
    ///<summary>
        /// Начальный спавн снежинок
        ///</summеry>
    {
        float x = random.Next(0, 1600);
        float y = random.Next(-this.ClientSize.Height, 0);
        float speed = (float)random.NextDouble() * 2 + 1;
        int size = random.Next(5, 10);
        snowflakes.Add(new Snowflake(x, y, speed, size));
    }

    private void Timer_Tick(object sender, EventArgs e)
    ///<summary>
        /// Перемещает снежинки и перерисовывает окно.
        ///</summеry>
    {
        MoveSnowflakes();
        this.Invalidate();
    }

    private void MoveSnowflakes()
    ///<summary>
        /// Спавн снежинок когда они зашли за границу снизу
        ///</summеry>
    {
        foreach (var snowflake in snowflakes)
        {
            snowflake.Y += snowflake.Speed;

            if (snowflake.Y > this.ClientSize.Height)
            ///<summary>
        /// Сбрасываем снежинку наверх
        ///</summеry>
            {
                snowflake.X = random.Next(0, this.ClientSize.Width);
                snowflake.Y = random.Next(-this.ClientSize.Height, 0);
                snowflake.Speed = (float)random.NextDouble() * 2 + 1;
                snowflake.Size = random.Next(5, 10);
            }
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    ///<summary>
        /// Ресуем фон и снежинки (Отрисовку окна)
        ///</summеry>
    {
        Graphics g = e.Graphics;

        g.DrawImage(background, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
        ///<summary>
        /// Ресуем фон
        ///</summеry>

        foreach (var snowflake in snowflakes)
        ///<summary>
        /// Ресуем снежинки
        ///</summеry>
        {
            using (Brush brush = new SolidBrush(Color.White))
            {
                g.FillEllipse(brush, snowflake.X, snowflake.Y, snowflake.Size, snowflake.Size);
            }
        }
    }

    [STAThread]
    public static void Main()
    ///<summary>
        /// Точка входа в программу
        ///</summеry>
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new SnowfallScreensaver());
    }
}
