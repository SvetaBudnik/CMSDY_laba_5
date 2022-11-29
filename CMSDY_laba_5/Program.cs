double y = 1;
double t = 0;

const double h = 0.00001;

static void Main(string[] args)
{
    double a = 0.0, b = 20.0;
    int n = Math.Round((b - a) / h) + 1;

    double [] grid_ = new double[n];
    double[] res_p = new double[n];
    double[] res_q = new double[n];


    //    void creat_grid(double[] grid_, int n, double a, double b);
    //    size_t part = 0;

    //    std::cout << "\nMENU\n";
    //    std::cout << "direct (1)\n";
    //    std::cout << "sequential (2)\n";
    //    std::cout << "exit(0)?\n\n";
    //    std::cin >> part;

    //    while (part != 0)
    //    {
    //        switch (part)
    //        {
    //            case (1):
    //                direct(grid_, res_p, res_q, n);
    //                break;
    //            case (2):
    //                sequential(grid_, res_p, res_q, n);
    //                break;
    //            default:
    //                break;
    //        }
    //        FILE* file;
    //        fopen_s(&file, "gспрямой_e5.txt", "w");
    //        int k = 0;
    //        std::cout << std::fixed << std::setprecision(16);
    //        for (int i = 0; i < n; i += 100)
    //        {
    //            k++;
    //            fprintf_s(file, "%20.15lf %20.15lf %20.15lf\n", grid_[i], res_p[i],
    //           res_q[i]);
    //            std::cout << std::setw(7) << grid_[i] << std::setw(25) << res_p[i] << std::setw(25) << res_q[i] << std::endl;
    //        }
    //        std::cout << "k=" << k << std::endl;
    //        std::cout << std::setw(7) << grid_[(n) / 2] << std::setw(25) << res_p[(n) / 2]
    //        << std::setw(25) << res_q[(n) / 2] << std::endl;
    //        fclose(file);
    //        std::cout << "\nMENU\n";
    //        std::cout << "direct (1)\n";
    //        std::cout << "sequential (2)\n";
    //        std::cout << "exit(0)?\n\n";
    //        std::cin >> part;
    //    }
 }


    //void creat_grid(double[] grid, int n, double[] t_start, double[] t_end)
    //{
    //    grid[0] = t_start;
    //    for (int i = 1; i < n - 1; i++)
    //    {
    //        grid[i] = i * h;
    //    }
    //    grid[n - 1] = t_end;
    //}

    void direct(double[] grid, double[] p, double[] q, int n)
{
     p[0] = 1e5;
     q[0] = 0.0;
    for (int i = 1; i < n; i++)
    {
        double k1_p = func_p(grid[i - 1], q[i - 1]);
        double k1_q = func_q(grid[i - 1], p[i - 1], q[i - 1]);
        double k2_p = func_p(grid[i - 1] + h / 2, q[i - 1] + h / 2 * k1_q);
        double k2_q = func_q(grid[i - 1] + h / 2, p[i - 1] + h / 2 * k1_p, q[i - 1] + h / 2 * k1_q);
        double k3_p = func_p(grid[i - 1] + h / 2, q[i - 1] + h / 2 * k2_q);
        double k3_q = func_q(grid[i - 1] + h / 2, p[i - 1] + h / 2 * k2_p, q[i - 1] + h / 2 * k2_q);
        double k4_p = func_p(grid[i - 1] + h, q[i - 1] + h * k3_q);
        double k4_q = func_q(grid[i - 1] + h, p[i - 1] + h * k3_p, q[i - 1] + h *
       k3_q);
        p[i] = p[i - 1] + h / 6 * (k1_p + 2 * k2_p + 2 * k3_p + k4_p);
        q[i] = q[i - 1] + h / 6 * (k1_q + 2 * k2_q + 2 * k3_q + k4_q);
    }
}

//static double kn(double t, double y, double h)
//{
//    double k1, k2, k3, k4;
//    k1 = func(t, y);
//    k2 = func(t + h / 2, y + h / 2 * k1);
//    k3 = func(t + h / 2, y + h / 2 * k2);
//    k4 = func(t + h, y + h * k3);
//    return 1.0 / 6 * (k1 + 2.0 * k2 + 2.0 * k3 + k4);

//}

void sequential(double[] grid, double[] p, double[] q, int n)
{
    p[0] = 1e5;
    q[0] = 0;
    for (int i = 1; i < n; i++)
    {
        double k1_p = func_p(grid[i - 1], q[i - 1]);
        double k2_p = func_p(grid[i - 1] + h / 2, q[i - 1] + h / 2 * q[i - 1]);
        double k3_p = func_p(grid[i - 1] + h / 2, q[i - 1] + h / 2 * q[i - 1]);
        double k4_p = func_p(grid[i - 1] + h, q[i - 1] + h * q[i - 1]);

        p[i] = p[i - 1] + h / 6 * (k1_p + 2 * k2_p + 2 * k3_p + k4_p);

        double k1_q = func_q(grid[i - 1], p[i], q[i - 1]);
        double k2_q = func_q(grid[i - 1] + h / 2, p[i], q[i - 1] + h / 2 * k1_q);
        double k3_q = func_q(grid[i - 1] + h / 2, p[i], q[i - 1] + h / 2 * k2_q);
        double k4_q = func_q(grid[i - 1] + h, p[i], q[i - 1] + h * k3_q);

        q[i] = q[i - 1] + h / 6 * (k1_q + 2 * k2_q + 2 * k3_q + k4_q);
    }
}

double func_p(double t, double q)
{
    double l = 1;
    double d = 0.01;
    double r = 1000;
    double Csnd = 1260;
    double s = Math.PI * d * d / 4;
    double C = l * s / r / Csnd / Csnd;

    return (q_n(t) - q) / C;
}

double func_q(double t, double p, double q)
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

double q_n(double t)
    {
        double q0 = 0.001;
        if (t < 1)
            return q0 * t;
        else
            return q0;
    }
