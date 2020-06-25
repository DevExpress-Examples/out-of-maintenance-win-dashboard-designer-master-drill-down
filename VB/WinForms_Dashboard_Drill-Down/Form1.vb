Imports System
Imports DevExpress.DashboardCommon
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace WinForms_Dashboard_Drill_Down
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
			dashboardDesigner1.CreateRibbon()
			dashboardDesigner1.AsyncMode = True
			AddHandler dashboardDesigner1.AsyncDataLoading, Sub(sender, e)
				e.Data = DataRow.GetData()
			End Sub
			Dim dashboard As New Dashboard()
			Dim ds As New DashboardObjectDataSource(New Object())
			dashboard.DataSources.Add(ds)
			Dim grid As New GridDashboardItem() With {.DataSource = ds}
			grid.Columns.Add(New GridDimensionColumn(New Dimension("Dimension1")))
			grid.Columns.Add(New GridDimensionColumn(New Dimension("Dimension2")))
			grid.Columns.Add(New GridDimensionColumn(New Dimension("Dimension3")))
			grid.Columns.Add(New GridMeasureColumn(New Measure("Measure")))
			grid.InteractivityOptions.IsDrillDownEnabled = True
			dashboard.Items.Add(grid)
			Dim chart As New ChartDashboardItem() With {.DataSource = ds}
			chart.Arguments.Add(New Dimension("Dimension1"))
			chart.Arguments.Add(New Dimension("Dimension2"))
			chart.Arguments.Add(New Dimension("Dimension3"))
			Dim pane As New ChartPane()
			pane.Series.Add(New SimpleSeries(New Measure("Measure")))
			chart.Panes.Add(pane)
			chart.InteractivityOptions.IsDrillDownEnabled = True
			dashboard.Items.Add(chart)

			dashboardDesigner1.Dashboard = dashboard

			Dim tempVar As New MasterDrillDownModule(dashboardDesigner1)
		End Sub
	End Class
End Namespace
