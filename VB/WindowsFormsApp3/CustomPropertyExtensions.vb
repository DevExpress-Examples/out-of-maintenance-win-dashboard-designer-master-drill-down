Imports Microsoft.VisualBasic
Imports DevExpress.DashboardCommon
Imports System

Namespace WindowsFormsApp3
	Public Module CustomPropertyExtensions
		<System.Runtime.CompilerServices.Extension>
		Public Function GetValue(Of T As Structure)(ByVal [property] As CustomProperties, ByVal name As String) As T
			Dim value = [property].GetValue(name)
			If value Is Nothing Then
				Return Nothing
			End If
			Return CType(Convert.ChangeType(value, GetType(T)), T)
		End Function
	End Module
End Namespace

