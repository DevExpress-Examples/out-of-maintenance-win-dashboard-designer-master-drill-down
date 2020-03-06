Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq

Namespace WindowsFormsApp3
	Public Class DataRow
		Public Shared Function GetData() As List(Of DataRow)
			Dim data As New List(Of DataRow)()
			For i As Integer = 1 To 9
				data.Add(New DataRow() With {.Dimension1 = "A" & i, .Dimension2 = "AA" & i, .Dimension3 = "AAA" & i, .Measure = i})
			Next i
			Return data
		End Function

		Private privateDimension1 As String
		Public Property Dimension1() As String
			Get
				Return privateDimension1
			End Get
			Set(ByVal value As String)
				privateDimension1 = value
			End Set
		End Property
		Private privateDimension2 As String
		Public Property Dimension2() As String
			Get
				Return privateDimension2
			End Get
			Set(ByVal value As String)
				privateDimension2 = value
			End Set
		End Property
		Private privateDimension3 As String
		Public Property Dimension3() As String
			Get
				Return privateDimension3
			End Get
			Set(ByVal value As String)
				privateDimension3 = value
			End Set
		End Property
		Private privateMeasure As Integer
		Public Property Measure() As Integer
			Get
				Return privateMeasure
			End Get
			Set(ByVal value As Integer)
				privateMeasure = value
			End Set
		End Property
	End Class
End Namespace
