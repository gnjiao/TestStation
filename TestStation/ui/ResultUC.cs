using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestStation.ui
{
    public partial class ResultUC : UserControl
    {
        public ResultUC()
        {
            InitializeComponent();
        }
        public void SetType(object type)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            switch (type as string)
            {
                case "Type A":
                    data["Dead Emitter Count"] = "";
                    data["Dead Cluster Count"] = "";
                    data["Emitter Divergence Angle"] = "";
                    data["Beam Waist Diameter"] = "";
                    data["Emission Uniformity"] = "";
                    break;
                case "Type B":
                    data["Divergence Angle"] = "";
                    break;
            }
            DGV_Result.DataSource = data.ToArray();
            Format();
        }
        public void Update(object data)
        {
            DGV_Result.DataSource = (data as Dictionary<string, string>).ToArray();
            Format();
        }
        private void Format()
        {
            DGV_Result.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DGV_Result.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DGV_Result.Columns[0].DefaultCellStyle.Font = new Font("宋体", 9, FontStyle.Bold);
        }
    }
}
