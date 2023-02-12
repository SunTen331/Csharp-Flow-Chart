using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppFlowChart
{
    public partial class ScpyAddForm : Form
    {
        List<Sequence> flowChartContent = FlowChart.GetFlowChart;
        List<string> categoryList = new List<string>();
        List<string> syntaxList = new List<string>();

        public ScpyAddForm(List<Sequence> content = null)
        {
            InitializeComponent();

            // Initialize ComboBox Category
            if (content != null) flowChartContent = content;

            categoryList.Clear();
            comboBoxCategory.Items.Clear();
            foreach (var s in flowChartContent)
            {
                foreach (var p in s.sequence)
                {
                    if (p.process[0] == FlowChart.SCPY)
                    {
                        if (categoryList.Contains(p.process[1]) == false)
                        {
                            categoryList.Add(p.process[1]);
                            break;
                        }
                    }
                }
            }
            foreach (string c in categoryList) comboBoxCategory.Items.Add(c);
            comboBoxCategory.SelectedIndex = 0;
        }

        private void ComboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Initialize ComboBox Syntax
            syntaxList.Clear();
            comboBoxSyntax.Items.Clear();
            foreach (var s in flowChartContent)
            {
                foreach (var p in s.sequence)
                {
                    if (p.process[1] == comboBoxCategory.SelectedItem.ToString())
                    {
                        if (p.process[2] == FlowChart.SYNTAX)
                        {
                            if (syntaxList.Contains(p.process[3]) == false)
                            {
                                syntaxList.Add(p.process[3]);
                                break;
                            }
                        }
                    }
                }
            }
            foreach (string s in syntaxList) comboBoxSyntax.Items.Add(s);
            comboBoxSyntax.SelectedIndex = 0;
        }

        public Sequence GetSelectedScpy()
        {
            foreach (var s in flowChartContent)
            {
                if(s.sequence[0].process[3] == comboBoxSyntax.SelectedItem.ToString())
                    return s;
            }
            return flowChartContent[0];
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
