Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace WinForms_Dashboard_Drill_Down
	Public Class DataRow
		Public Shared Function GetData() As List(Of DataRow)
			Dim data As New List(Of DataRow)()
			For i As Integer = 1 To 9
				data.Add(New DataRow() With {
					.Dimension1 = "A" & i,
					.Dimension2 = "AA" & i,
					.Dimension3 = "AAA" & i,
					.Measure = i
				})
			Next i
			Return data
		End Function

		Public Property Dimension1() As String
		Public Property Dimension2() As String
		Public Property Dimension3() As String
		Public Property Measure() As Integer
	End Class
End Namespace
