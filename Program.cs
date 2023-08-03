using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

public class MainForm : Form
{
    private Timer timer;
    private PictureBox pictureBox;
    private Label label;

    public MainForm()
    {
        // 设置窗体属性
        this.Text = "原神检测";
        this.Size = new Size(1280, 720);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        // 创建并设置PictureBox控件
        pictureBox = new PictureBox();
        pictureBox.Image = GetEmbeddedImage("2.jpg");
        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox.Size = new Size(855, 473);
        pictureBox.Location = new Point(this.ClientSize.Width / 2 - pictureBox.Width / 2, 50);
        this.Controls.Add(pictureBox);

        // 创建并设置Label控件
        label = new Label();
        label.Text = "原神检测";
        label.Font = new Font("Arial", 48, FontStyle.Bold);
        label.AutoSize = true;
        label.Location = new Point(this.ClientSize.Width / 2 - label.Width / 2, pictureBox.Bottom + 30);
        this.Controls.Add(label);

        // 创建并设置Timer控件
        timer = new Timer();
        timer.Interval = 1500; // 延迟时间，单位为毫秒
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        timer.Stop(); // 停止定时器

        // 定义原神安装路径的注册表项路径
        string regPath = @"SOFTWARE\miHoYo\原神";

        // 使用Registry类获取注册表项
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(regPath))
        {
            // 检查注册表项是否存在
            bool isInstalled = key != null;

            // 输出结果
            if (isInstalled)
            {
                MessageBox.Show("原神,启动！");

                // 获取原神安装路径
                string installPath = key.GetValue("InstallPath") as string;

                // 启动原神程序
                Process.Start(installPath);
            }
            else
            {
                MessageBox.Show("你还没装原神？");

                // 打开下载链接
                Process.Start("https://ys-api.mihoyo.com/event/download_porter/link/ys_cn/official/pc_default");
            }
        }
    }

    private Image GetEmbeddedImage(string resourceName)
    {
        // 获取当前程序集
        Assembly assembly = Assembly.GetExecutingAssembly();

        // 构建资源的完全限定名称（包括命名空间）
        string fullResourceName = $"{assembly.GetName().Name}.{resourceName}";

        // 通过资源名称加载资源流
        using (Stream stream = assembly.GetManifestResourceStream(fullResourceName))
        {
            if (stream != null)
            {
                // 从流中创建并返回图片对象
                return Image.FromStream(stream);
            }
        }

        return null;
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}