class Program
{
    static double y = 1;
    static double t = 0;

    const double h = 0.00001;

    public static void Main(string[] args)
    {
        double a = 0.0, b = 20.0;
        int n = (int)Math.Round((b - a) / h) + 1;

        double[] grid_ = new double[n];
        double[] res_p = new double[n];
        double[] res_q = new double[n];


        Create_grid(grid_, a, b);
        
        int part = 1;
        while (part != 0)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("Direct (1)");
            Console.WriteLine("Sequential (2)");
            Console.WriteLine("Exit (0)");

            part = int.Parse(Console.ReadLine());

            StreamWriter? file = null;
            switch (part)
            {
                case (1):
                    {
                        Direct(grid_, res_p, res_q, n);
                        file = new StreamWriter("Прямой.txt");
                        break;
                    }
                case (2):
                    {
                        Sequential(grid_, res_p, res_q, n);
                        file = new StreamWriter("Последовательный.txt");
                        break;
                    }
                default:
                    break;
            }
            if (file == null)
            {
                continue;
            }

            int k = 0;
            for (int i = 0; i < n; i += 200)
            {
                k++;
                var str = $"{grid_[i], 20:e15} {res_p[i], 20:e15} {res_q[i],20:e15}";
                file.WriteLine(str);
                //Console.WriteLine(str);

            }
            Console.WriteLine($"Количество выведенных элементов {k}");
            Console.WriteLine($"{grid_[(n) / 2],20:e15} {res_p[(n) / 2],20:e15} {res_q[(n) / 2],20:e15}");

            file.Close();
            file = null;
        }
    }


    static void Create_grid(double[] grid, double t_start, double t_end)
    {
        int n = grid.Length;
        grid[0] = t_start;
        for (int i = 1; i < n - 1; i++)
        {
            grid[i] = i * h;
        }
        grid[n - 1] = t_end;
    }

    static void Direct(double[] grid, double[] p, double[] q, int n)
    {
        p[0] = 1e5;
        q[0] = 0.0;
        for (int i = 1; i < n; i++)
        {
            double k1_p = Func_p(grid[i - 1], q[i - 1]);
            double k1_q = Func_q(grid[i - 1], p[i - 1], q[i - 1]);

            double k2_p = Func_p(grid[i - 1] + h / 2, q[i - 1] + h / 2 * k1_q);
            double k2_q = Func_q(grid[i - 1] + h / 2, p[i - 1] + h / 2 * k1_p, q[i - 1] + h / 2 * k1_q);

            double k3_p = Func_p(grid[i - 1] + h / 2, q[i - 1] + h / 2 * k2_q);
            double k3_q = Func_q(grid[i - 1] + h / 2, p[i - 1] + h / 2 * k2_p, q[i - 1] + h / 2 * k2_q);

            double k4_p = Func_p(grid[i - 1] + h, q[i - 1] + h * k3_q);
            double k4_q = Func_q(grid[i - 1] + h, p[i - 1] + h * k3_p, q[i - 1] + h * k3_q);

            p[i] = p[i - 1] + h / 6 * (k1_p + 2 * k2_p + 2 * k3_p + k4_p);
            q[i] = q[i - 1] + h / 6 * (k1_q + 2 * k2_q + 2 * k3_q + k4_q);


        }
    }


    static void Sequential(double[] grid, double[] p, double[] q, int n)
    {
        p[0] = 1e5;
        q[0] = 0;
        for (int i = 1; i < n; i++)
        {
            double k1_p = Func_p(grid[i - 1], q[i - 1]);
            double k2_p = Func_p(grid[i - 1] + h / 2, q[i - 1] + h / 2 * q[i - 1]);
            double k3_p = Func_p(grid[i - 1] + h / 2, q[i - 1] + h / 2 * q[i - 1]);
            double k4_p = Func_p(grid[i - 1] + h, q[i - 1] + h * q[i - 1]);

            p[i] = p[i - 1] + h / 6 * (k1_p + 2 * k2_p + 2 * k3_p + k4_p);

            double k1_q = Func_q(grid[i - 1], p[i], q[i - 1]);
            double k2_q = Func_q(grid[i - 1] + h / 2, p[i], q[i - 1] + h / 2 * k1_q);
            double k3_q = Func_q(grid[i - 1] + h / 2, p[i], q[i - 1] + h / 2 * k2_q);
            double k4_q = Func_q(grid[i - 1] + h, p[i], q[i - 1] + h * k3_q);

            q[i] = q[i - 1] + h / 6 * (k1_q + 2 * k2_q + 2 * k3_q + k4_q);
        }
    }

    static double Func_p(double t, double q)
    {
        double l = 1;
        double d = 0.01;
        double r = 1000;
        double Csnd = 1260;
        double s = Math.PI * d * d / 4;
        double C = l * s / r / Csnd / Csnd;

        return (q_n(t) - q) / C;
    }

    static double Func_q(double t, double p, double q)
    {
        double Patm = 1e5;
        double q0 = 0.001;
        double ksi = 1 - Math.Sqrt(2) / 2;
        double l = 1;
        double d = 0.01;
        double r = 1000;
        double Csnd = 1260;
        double s = Math.PI * d * d / 4;
        double C = l * s / r / Csnd / Csnd;

        return Math.Sqrt(ksi / 2 / r * Math.Abs(p - Patm)) * (s * Math.Sqrt(2 / ksi / r * Math.Abs(p - Patm)) * (p - Patm > 0 ? 1 : -1) - q);
    }

    static double q_n(double t)
    {
        double q0 = 0.001;
        if (t < 1)
            return q0 * t;
        else
            return q0;
    }
}