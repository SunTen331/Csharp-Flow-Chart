using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppFlowChart
{
    public partial class MainForm : Form
    {
        private static List<Sequence> scpyFunction = FlowChart.GetFlowChart;
        List<Sequence> selectedList = new List<Sequence>();
        Sequence selectedScpy = scpyFunction[0];
        private static List<Block> generalBlockList = new List<Block>();

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            ScpyAddForm scpyAddForm = new ScpyAddForm();

            if (scpyAddForm.ShowDialog() == DialogResult.OK)
            {
                selectedScpy = scpyAddForm.GetSelectedScpy();

                if (selectedScpy != scpyFunction[0])
                {
                    if (selectedList.Contains(selectedScpy))
                    {
                        MessageBox.Show(
                        "Already add before.",
                        "Add Message",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }
                    else
                    {
                        int idx = selectedList.Count;
                        for (int i=0; i<selectedList.Count; ++i)
                        {
                            if (selectedList[i].sequence[0].process[1] 
                                == selectedScpy.sequence[0].process[1])
                            {
                                idx = (i+1);
                            }
                        }
                        selectedList.Insert(idx, selectedScpy);

                        //selectedList.Add(selectedScpy);
                        RepaintFlowChart();
                    }
                }
            }
        }

        private void ButtonDel_Click(object sender, EventArgs e)
        {
            if (selectedList.Count == 0)
            {
                MessageBox.Show(
                    "Nothing to delete.", 
                    "Delete Message",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
            else
            {
                ScpyAddForm scpyAddForm = new ScpyAddForm(selectedList);

                if (scpyAddForm.ShowDialog() == DialogResult.OK)
                {
                    selectedScpy = scpyAddForm.GetSelectedScpy();
                    selectedList.Remove(selectedScpy);
                    RepaintFlowChart();
                }
            }
        }

        /// <summary>
        /// Refresh the flowchart.
        /// When add/delete flowchart, the blocks position might be different
        /// </summary>
        private void RepaintFlowChart()
        {
            panelFlowChart.Controls.Clear();
            generalBlockList.Clear();

            int xLoc = 10, yLoc = 10;
            foreach (var s in selectedList)
            {
                foreach (var p in s.sequence)
                {
                    if (p.process[0] == FlowChart.SCPY)
                    {
                        Process titleFormat = new Process(new List<string> { FlowChart.SCPY, p.process[3] });
                        panelFlowChart.Controls.Add(new Block(xLoc, yLoc, titleFormat).blockPanel);
                    }
                    else
                    {
                        xLoc = panelFlowChart.Width + 10;
                        panelFlowChart.Controls.Add(new Block(xLoc, yLoc, p).blockPanel);
                    }
                }
                
                // Readjust blocks position - X-axis
                int xUpdate = 10;
                foreach (var block in generalBlockList)
                {
                    string[] blockNames = block.blockFullName.Split(Path.DirectorySeparatorChar);
                    if (blockNames.Length == 1)
                    {
                        block.blockPanel.Location = new Point(xUpdate, 10);
                        xUpdate += (block.blockPanel.Width + 20);
                    }
                    else
                    {
                        // Readjust blocks position - Y-axis
                        string parentBlockName = null;
                        for (int i=0; i<blockNames.Length; ++i)
                        {
                            if (parentBlockName == null) parentBlockName = blockNames[i];
                            else parentBlockName += (Path.DirectorySeparatorChar + blockNames[i]);

                            foreach (var parentBlock in generalBlockList)
                            {
                                if (parentBlock.blockFullName == parentBlockName)
                                {
                                    int yUpdate = 20;
                                    foreach (Control ctrl in parentBlock.blockPanel.Controls)
                                    {
                                        if (ctrl.GetType() == typeof(Panel))
                                        {
                                            ctrl.Location = new Point(ctrl.Location.X, yUpdate);
                                            yUpdate += ctrl.Height + 10;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // After readjust blocks position, 
            // get each of their start point & end point for draw line purpose
            List<List<Point>> flowChartLines = new List<List<Point>>();
            foreach (var s in selectedList)
            {
                List<string> blocks;
                List<Point> lines = new List<Point>();
                foreach (var p in s.sequence)
                {
                    if (p.process[0] == FlowChart.SCPY) blocks = new List<string> { FlowChart.SCPY, p.process[3] };
                    else blocks = p.process;

                    string[] blockNames = blocks[0].Split(Path.DirectorySeparatorChar);
                    List<string> fullNames = new List<string>();
                    string name = null;
                    foreach (var blockName in blockNames)
                    {
                        if (name == null) name = blockName;
                        else name += (Path.DirectorySeparatorChar + blockName);
                        fullNames.Add(name);
                    }

                    int startPointX = 0, startPointY = 0;
                    int endPointX = 0, endPointY = 0;

                    for (int i = 0; i < fullNames.Count; ++i)
                    {
                        foreach (var b in generalBlockList)
                        {
                            if (b.blockFullName == fullNames[i])
                            {
                                startPointX += (b.blockPanel.Location.X);
                                startPointY += (b.blockPanel.Location.Y);
                                endPointX += (b.blockPanel.Location.X);
                                endPointY += (b.blockPanel.Location.Y);

                                if (fullNames.Last() == fullNames[i])
                                {
                                    foreach (Control ctrl in b.blockPanel.Controls)
                                    {
                                        if (ctrl.GetType() == typeof(Label))
                                        {
                                            if (ctrl.Text == blocks[1])
                                            {
                                                startPointX += ctrl.Location.X;
                                                startPointY += (ctrl.Location.Y + (ctrl.Height/2));
                                            }
                                            if (ctrl.Text == blocks.Last())
                                            {
                                                endPointX += (ctrl.Location.X + ctrl.Width);
                                                endPointY += (ctrl.Location.Y + (ctrl.Height / 2));
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    lines.Add(new Point(startPointX, startPointY));
                    lines.Add(new Point(endPointX, endPointY));
                }
                
                // Draw lines
                for (int i=1; i<lines.Count - 1; i+=2) DrawLine(panelFlowChart, lines[i], lines[i+1]);
            }
        }

        /// <summary>
        /// This will draw a line on the top of given panel
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="pixel"></param>
        /// <param name="color"></param>
        public void DrawLine(Panel panel, Point startPoint, Point endPoint, int pixel = 3, Color? color = null)
        {
            Point[] points = {
                startPoint,
                endPoint,
                new Point(endPoint.X, endPoint.Y + pixel),
                new Point(startPoint.X, startPoint.Y + pixel)
            };

            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(points);

            Panel panelDrawLine = new Panel()
            {
                Size = panel.Size,
                BackColor = color == null? Color.Red : (Color) color,
                Region = new Region(gp)
            };

            panel.Controls.Add(panelDrawLine);
            panelDrawLine.BringToFront();
        }

        /// <summary>
        /// 
        /// Block class element:
        ///     - 1 panel:  to store label or panel
        ///     - 1 label:  to show the block name (Note: not blockFullName, just the blockName)
        ///     - zero / more label (optional): to show the process 
        ///     
        /// Example:
        ///     User path:      "A/ab/ccc"
        ///     User process:   p1, p2, p3
        ///     
        ///     Process:        @"A/ab/ccc", "p1", "p2", "p3"
        ///     After receive parameter (Process), this class will generate Block object with:
        ///         - blockFullName = @"A"
        ///         - blockName = "A"
        ///     Then first Block object will auto generate a children Block object with:
        ///         - blockFullName = @"A/ab"
        ///         - blockName = "ab" 
        ///     The children Block object will auto generate another children Block object with:
        ///         - blockFullName = @"A/ab/ccc"
        ///         - blockName = "ccc" 
        ///         - label_1.Text = "p1"
        ///         - label_2.Text = "p2"
        ///         - label_3.Text = "p3"
        ///         
        /// </summary>
        public class Block
        {
            public Panel blockPanel;
            public string blockFullName, parentBlockName;

            public Block(int x, int y, Process p, int dirSeparate = 0)
            {
                // Create Block
                blockPanel = new Panel()
                {
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    AutoSize = true,
                    Location = new Point(x, y),
                    BackColor = Color.SkyBlue
                };

                // Create Block Name
                string[] blockNames = p.process[0].Split(Path.DirectorySeparatorChar);
                string blockName = blockNames[dirSeparate];
                int xLoc = 10, yLoc = 10;
                blockPanel.Controls.Add(new Label()
                {
                    AutoSize = true,
                    Location = new Point(xLoc, yLoc),
                    BackColor = Color.DeepSkyBlue,
                    Text = blockName
                });

                // Check first time add
                for (int i = 0; i <= dirSeparate; ++i)
                {
                    if (i == 0) blockFullName = blockNames[0];
                    else blockFullName += (Path.DirectorySeparatorChar + blockNames[i]);
                }

                bool firstAdd = true;
                foreach (var block in generalBlockList)
                {
                    if (blockFullName == block.blockFullName)
                    {
                        // If not first time, use the exist panel
                        blockPanel = generalBlockList[generalBlockList.IndexOf(block)].blockPanel;
                        yLoc = blockPanel.Height;
                        firstAdd = false;
                        break;
                    }
                }
                if (firstAdd)   // If first time, add it to generalBlockList
                {
                    generalBlockList.Add(this);
                    yLoc = 30;
                }

                // Create Children Block - if needed
                if (++dirSeparate < blockNames.Length)
                {
                    blockPanel.Controls.Add(new Block(xLoc, yLoc, p, dirSeparate).blockPanel);
                }

                // Create Process Block - if there is no more children block needed
                else
                {
                    foreach (var process in p.process)
                    {
                        if (process == p.process[0]) continue;

                        Label l = new Label()
                        {
                            AutoSize = true,
                            Location = new Point(xLoc, yLoc),
                            BackColor = Color.White,
                            Text = process
                        };
                        blockPanel.Controls.Add(l);

                        yLoc += 15;
                    }
                }
            }
        }
    }
}
