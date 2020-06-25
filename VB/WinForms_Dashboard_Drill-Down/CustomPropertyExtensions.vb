Imports DevExpress.DashboardCommon
Imports System

Namespace WinForms_Dashboard_Drill_Down
	Public Module CustomPropertyExtensions
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetValue(Of T As Structure)(ByVal [property] As CustomProperties, ByVal name As String) As T
			Dim value = [property].GetValue(name)
			If value Is Nothing Then
				Return CType(Nothing, T)
			End If
			Return DirectCast(Convert.ChangeType(value, GetType(T)), T)
		End Function
	End Module
End Namespace

