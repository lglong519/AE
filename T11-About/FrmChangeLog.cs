using System.Drawing;
using System.Windows.Forms;

namespace T11_About
{
    public partial class FrmChangeLog : Form
    {
        public FrmChangeLog()
        {
            InitializeComponent();
            const string logs = @"1.4.1 [19-05-31]
	批量赋值条件修改;角度检测结果列表可以按ID选择要素;数字保留两位小数
1.4.0 [19-05-29]
	[增加]属性批量赋值:统一修改种植属性代码、名称、耕地类型、清空’0‘和‘无’
1.3.0 [19-05-26]
	[增加]权属性质计算：可定义属性值；
	去除可能导致属性丢失的代码,允许开启编辑的状态下修改属性；
	优化内存可以连续计算10万+的数据，添加分段计算功能；
	[增加]角度检测：可检测尖锐角、狭长面，角度可自定义；
	修复其他已知Bug
1.2.2 [19-05-10]
	修复因图层名相同导致字段选择项闪烁的Bug；
	快速修改模式下防止开启编辑导致属性丢失
1.2.1 [19-05-08]
	基本农田计算阈值由0.5改为0.45，开启快速修改模式；
	修复各种可能导致ArcGis崩溃的Bug；
	添加本地日志和配置
1.2.0 [19-05-07]
	[增加]基本农田标识码计算
1.0 [19-04-30]
	[增加]图层名添加至图层要素属性；
	耕地等别
	.NETFramework4.0 转 3.5；";
            string[] sections = logs.Split('\n');
            for (int i = 0; i < sections.Length; i++)
            {
                string section=sections[i];
                if (string.IsNullOrEmpty(section.Trim()))
                {
                    continue;
                }
                if (section.StartsWith("	") || section.StartsWith(" "))
                {
                    richTextBox1.SelectionIndent = 24;
                    richTextBox1.SelectionBullet = true;
                }
                else
                {
                    richTextBox1.SelectionColor = Color.Blue;
                    richTextBox1.SelectionIndent = 0;
                    richTextBox1.SelectionBullet = false;
                }
                if (i == sections.Length - 1)
                {
                    richTextBox1.AppendText(sections[i].Trim());
                }
                else
                {
                    richTextBox1.AppendText(sections[i].Trim() + "\n");
                }
            }
        }
    }
}
