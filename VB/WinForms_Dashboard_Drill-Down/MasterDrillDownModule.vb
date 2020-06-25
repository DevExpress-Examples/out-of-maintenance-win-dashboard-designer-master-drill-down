Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWin
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports System.Collections.Generic
Imports System.Linq

Namespace WinForms_Dashboard_Drill_Down
	Public Class MasterDrillDownModule
		Private Shared MFDPropertyName As String = "MasterDrillDown"
		Private Shared IgnoreMFDPropertyName As String = "IgnoreMasterDrillDown"

		Private ReadOnly designer As DashboardDesigner
		Private ReadOnly ribbon As RibbonControl
		Private mfdBarItem As BarCheckItem
		Private ignoreMfdBarItem As BarCheckItem

		Public Sub New(ByVal designer As DashboardDesigner)
			Me.designer = designer
			AddHandler designer.DrillDownPerformed, AddressOf OnDrillDownPerformed
			AddHandler designer.DrillUpPerformed, AddressOf OnDrillUpPerformed
			AddHandler designer.DashboardItemSelected, AddressOf OnDashboardItemSelected
			AddHandler designer.DashboardCustomPropertyChanged, AddressOf OnCustomPropertyChanged

			ribbon = designer.Ribbon
			CreateBarItems(DashboardBarItemCategory.ChartTools, DashboardBarItemCategory.GridTools)
		End Sub
		Private Sub CreateBarItems(ParamArray ByVal categories() As DashboardBarItemCategory)
			mfdBarItem = New BarCheckItem(ribbon.Manager, False)
			mfdBarItem.Caption = MFDPropertyName
			AddHandler mfdBarItem.ItemClick, AddressOf OnMFDButtonClick
			ignoreMfdBarItem = New BarCheckItem(ribbon.Manager, False)
			ignoreMfdBarItem.Caption = IgnoreMFDPropertyName
			AddHandler ignoreMfdBarItem.ItemClick, AddressOf OnIgnoreMFDBarItemClick

			For Each category As DashboardBarItemCategory In categories
				Dim page As RibbonPage = ribbon.GetDashboardRibbonPage(category, DashboardRibbonPage.Data)
				Dim group As RibbonPageGroup = page.GetGroupByName("Drill-Down")
				If group Is Nothing Then
					group = New RibbonPageGroup("Drill-Down") With {.Name = "Drill-Down"}
					page.Groups.Add(group)
				End If
				group.ItemLinks.Add(mfdBarItem)
				group.ItemLinks.Add(ignoreMfdBarItem)
			Next category
		End Sub
		Private Sub OnDrillDownPerformed(ByVal sender As Object, ByVal e As DrillActionEventArgs)
			SyncronizeData(e)
		End Sub
		Private Sub OnDrillUpPerformed(ByVal sender As Object, ByVal e As DrillActionEventArgs)
			SyncronizeData(e)
		End Sub
		Private Sub SyncronizeData(ByVal e As DrillActionEventArgs)
			If Not GetCustomPropertyValue(e.DashboardItemName, MFDPropertyName) Then
				Return
			End If

			Dim drillDownValues As New List(Of Object)()
			If e.DrillDownLevel > 0 Then
				For i As Integer = 0 To e.DrillDownLevel - 1
					drillDownValues.Add(e.Values(0).DataSet.GetValue(0, i))
				Next i
			End If

			Dim state As New DashboardState()
			For Each item In designer.Dashboard.Items.OfType(Of DataDashboardItem)().Where(Function(i) Not GetCustomPropertyValue(i.ComponentName, IgnoreMFDPropertyName))
				state.Items.Add(New DashboardItemState(item.ComponentName) With {.DrillDownValues = drillDownValues})
			Next item
			designer.SetDashboardState(state)
		End Sub
		Private Sub OnCustomPropertyChanged(ByVal sender As Object, ByVal e As CustomPropertyChangedEventArgs)
			If e.Name = MFDPropertyName Then
				UpdateMasterDrillDownBarItem()
			End If
			If e.Name = IgnoreMFDPropertyName Then
				UpdateIgnoreMasterDrillDownBarItem()
			End If
		End Sub
		Private Sub OnDashboardItemSelected(ByVal sender As Object, ByVal e As DashboardItemSelectedEventArgs)
			UpdateMasterDrillDownBarItem()
			UpdateIgnoreMasterDrillDownBarItem()
		End Sub
		Private Sub OnMFDButtonClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			Dim newValue = Not GetCustomPropertyValue(designer.SelectedDashboardItem?.ComponentName, MFDPropertyName)
			designer.AddToHistory(New CustomPropertyHistoryItem(designer.SelectedDashboardItem, MFDPropertyName, newValue.ToString(), String.Format("{0} Changed", MFDPropertyName)))
		End Sub

		Private Sub OnIgnoreMFDBarItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			Dim newValue = Not GetCustomPropertyValue(designer.SelectedDashboardItem?.ComponentName, IgnoreMFDPropertyName)
			designer.AddToHistory(New CustomPropertyHistoryItem(designer.SelectedDashboardItem, IgnoreMFDPropertyName, newValue.ToString(), String.Format("{0} Changed", IgnoreMFDPropertyName)))
		End Sub

		Private Sub UpdateMasterDrillDownBarItem()
			mfdBarItem.Checked = GetCustomPropertyValue(designer.SelectedDashboardItem?.ComponentName, MFDPropertyName)
			ignoreMfdBarItem.Enabled = Not mfdBarItem.Checked
		End Sub
		Private Sub UpdateIgnoreMasterDrillDownBarItem()
			ignoreMfdBarItem.Checked = GetCustomPropertyValue(designer.SelectedDashboardItem?.ComponentName, IgnoreMFDPropertyName)
			mfdBarItem.Enabled = Not ignoreMfdBarItem.Checked
		End Sub
		Private Function GetCustomPropertyValue(ByVal itemName As String, ByVal propertyName As String) As Boolean
			Dim dashboardItem As DashboardItem = designer.Dashboard.Items(itemName)
			Return If(dashboardItem IsNot Nothing, dashboardItem.CustomProperties.GetValue(Of Boolean)(propertyName), False)
		End Function
	End Class
End Namespace
