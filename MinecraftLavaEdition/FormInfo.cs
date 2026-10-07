using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MinecraftLavaEdition
{
    public partial class FormInfo : Form
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref Point lParam);

        private const int EM_GETSCROLLPOS = 0x04DD;
        private const int EM_SETSCROLLPOS = 0x04DE;
        public FormInfo()
        {
            InitializeComponent();
            SetInfo(null);
        }

        public void SetInfo(LavaRenderInfo? info)
        {
            if (info == null)
            {
                lblCoord.Text = "坐标：—";
                txtInfo.Text = "双击输出结果的任意一行以查看信息。";
                txtInfo.SelectionStart = 0; 
                txtInfo.SelectionLength = 0;
                txtInfo.ScrollToCaret();
                return;
            }
            txtInfo.SelectionStart = 0;
            txtInfo.SelectionLength = 0;
            txtInfo.ScrollToCaret();
            lblCoord.Text = $"坐标：{info.Position}";

            var sb = new StringBuilder();

            sb.AppendLine("流向：");
            sb.AppendLine($"  向量 = ({info.FlowX:F4}, {info.FlowY:F4}, {info.FlowZ:F4})");
            if (info.FlowAngle < -999f)
                sb.AppendLine("  角度 = 无");
            else
                sb.AppendLine($"  角度 = {info.FlowAngle * 180 / Math.PI:F1}°");
            sb.AppendLine();

            sb.AppendLine("模型四角高度：");
            sb.AppendLine($"  NW = {info.Corners.NW:F3}");
            sb.AppendLine($"  NE = {info.Corners.NE:F3}");
            sb.AppendLine($"  SE = {info.Corners.SE:F3}");
            sb.AppendLine($"  SW = {info.Corners.SW:F3}");
            sb.AppendLine($"  平均（中心） = {info.SurfaceHeight:F3}");
            sb.AppendLine();

            sb.AppendLine("顶面顶点：");
            for (int i = 0; i < 4; i++)
            {
                var c = info.TopCorners[i];
                if (c == null) { sb.AppendLine($"  顶点{i} = null"); continue; }
                sb.AppendLine($"  顶点{i} = ({c[0]:F4}, {c[1]:F4}, {c[2]:F4})");
            }
            sb.AppendLine();

            sb.AppendLine("顶面 UV：");
            sb.AppendLine("  " + info.TopFaceUV);
            sb.AppendLine();

            sb.AppendLine("侧面 UV：");
            string[] sideNames = { "北", "南", "西", "东" };
            for (int i = 0; i < 4; i++)
            {
                var uv = info.SideFaceUVValues[i];
                if (uv == null)
                {
                    sb.AppendLine($"  {sideNames[i]}面：{info.SideFaceUV[i]}");
                    continue;
                }
                sb.AppendLine($"  {sideNames[i]}面：");
                sb.AppendLine($"    顶点0 (u,v) = ({uv[0]:F4}, {uv[1]:F4})");
                sb.AppendLine($"    顶点1 (u,v) = ({uv[2]:F4}, {uv[3]:F4})");
                sb.AppendLine($"    顶点2 (u,v) = ({uv[4]:F4}, {uv[5]:F4})");
                sb.AppendLine($"    顶点3 (u,v) = ({uv[6]:F4}, {uv[7]:F4})");
            }

            txtInfo.Text = sb.ToString();
        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            txtInfo.Multiline = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.WordWrap = false;
            this.ShowInTaskbar = false;
        }
    }
}