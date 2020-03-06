using DevExpress.DashboardCommon;
using System.Windows.Forms;

namespace WindowsFormsApp3 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            dashboardDesigner1.AsyncDataLoading += (sender, e) => {
                e.Data = DataRow.GetData();
            };

            Dashboard dashboard = new Dashboard();
            DashboardObjectDataSource ds = new DashboardObjectDataSource(new object());
            dashboard.DataSources.Add(ds);
            GridDashboardItem grid = new GridDashboardItem() { DataSource = ds };
            grid.Columns.Add(new GridDimensionColumn(new Dimension("Dimension1")));
            grid.Columns.Add(new GridDimensionColumn(new Dimension("Dimension2")));
            grid.Columns.Add(new GridDimensionColumn(new Dimension("Dimension3")));
            grid.Columns.Add(new GridMeasureColumn(new Measure("Measure")));
            grid.InteractivityOptions.IsDrillDownEnabled = true;
            dashboard.Items.Add(grid);
            ChartDashboardItem chart = new ChartDashboardItem() { DataSource = ds };
            chart.Arguments.Add(new Dimension("Dimension1"));
            chart.Arguments.Add(new Dimension("Dimension2"));
            chart.Arguments.Add(new Dimension("Dimension3"));
            ChartPane pane = new ChartPane();
            pane.Series.Add(new SimpleSeries(new Measure("Measure")));
            chart.Panes.Add(pane);
            chart.InteractivityOptions.IsDrillDownEnabled = true;
            dashboard.Items.Add(chart);

            dashboardDesigner1.Dashboard = dashboard;

            new MasterDrillDownModule(dashboardDesigner1);
        }
    }
}
