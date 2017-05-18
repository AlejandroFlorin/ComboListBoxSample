Imports System.Globalization

Public Class CommonUtilities

    Public Shared Function ConvertStringToDouble(Value As String) As Double

        'Convert string to double, taking into account the current culture's number separator
        Dim nfi As New NumberFormatInfo

        If Value.Contains(".") Then
            nfi.NumberDecimalSeparator = "."
        ElseIf Value.Contains(",") Then
            nfi.NumberDecimalSeparator = ","
        End If

        Return Double.Parse(Value, NumberStyles.Float, nfi)

    End Function

    Public Shared Sub RequenceTable(dt As DataTable, seqColName As String, startIndex As Integer)

        For Each dr As DataRow In dt.Select("", seqColName)
            dr(seqColName) = startIndex
            startIndex += 1
        Next

    End Sub

    Public Shared Function InsertRow(dt As DataTable, seqColName As String, drNewRow As DataRow) As Integer
        'This inserts the new row AFTER the sequence value specified in the new row

        If IsValidInteger(drNewRow(seqColName).ToString) Then
            For Each dr As DataRow In dt.Rows
                If dr.RowState <> DataRowState.Deleted Then
                    If Val(dr(seqColName)) > Val(drNewRow(seqColName)) Then
                        dr(seqColName) = CInt(dr(seqColName)) + 1
                    End If
                End If
            Next
            drNewRow(seqColName) = CInt(drNewRow(seqColName)) + 1
            dt.Rows.Add(drNewRow)
            Return CInt(drNewRow(seqColName))
        Else
            Throw New System.Exception("The seqColName field in drNewRow must contain an integer")
        End If

    End Function

    Public Shared Sub DeleteRow(dt As DataTable, seqColName As String, drRowToDelete As DataRow)

        If IsValidInteger(drRowToDelete.Item(seqColName).ToString) Then

            Dim deletePosition As Integer = CInt(drRowToDelete.Item(seqColName))
            Dim oldPosition As Integer
            Dim dr As DataRow

            For Each dr In dt.Rows
                If dr.RowState <> DataRowState.Deleted Then
                    oldPosition = CInt(dr.Item(seqColName))
                    If oldPosition > deletePosition Then
                        dr.Item(seqColName) = oldPosition - 1
                    End If
                End If
            Next
            drRowToDelete.Delete()

        Else
            Throw New System.Exception("The seqColName field in drRowToDelete must contain an integer")
        End If

    End Sub

    Public Shared Sub MoveRow(dt As DataTable, seqColName As String, drRowToMove As DataRow, direction As CommonEnums.MoveDirections)

        If IsValidInteger(drRowToMove.Item(seqColName).ToString) Then

            Dim seqChange As Integer
            If direction = CommonEnums.MoveDirections.Up Then
                seqChange = -1
            Else
                seqChange = 1
            End If

            Dim MovePosition As Integer = CInt(drRowToMove.Item(seqColName))
            Dim dr As DataRow
            For Each dr In dt.Rows
                If dr.RowState <> DataRowState.Deleted Then
                    If CInt(dr.Item(seqColName)) = MovePosition + seqChange Then
                        dr.Item(seqColName) = MovePosition
                        drRowToMove.Item(seqColName) = MovePosition + seqChange
                        Exit For
                    End If
                End If
            Next
        Else
            Throw New System.Exception("The seqColName field in drRowToMove must contain an integer")
        End If

    End Sub

    Public Shared Sub AddTableColumn(dataTable As DataTable, columnName As String, type As Type)

        If dataTable.Columns.Contains(columnName) Then Return

        dataTable.Columns.Add(New DataColumn(columnName, type))

    End Sub

    Public Shared Function IsValidInteger(
        value As String,
        Optional minValue As Integer = Integer.MinValue,
        Optional maxValue As Integer = Integer.MaxValue,
        Optional allowPrecisionLoss As Boolean = False) As Boolean

        'Utility function used to check whether a string value is a valid integer. It includes parameters for checking a valid range as well as allowing for precision loss

        If Not allowPrecisionLoss Then

            Dim integerValue As Integer

            If Not Integer.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, integerValue) Then
                Return False
            End If

            If integerValue = Integer.MinValue OrElse integerValue = Integer.MaxValue OrElse integerValue < minValue OrElse integerValue > maxValue Then
                Return False
            End If

        Else

            'If we are allowing fractional digits via AllowPrecisionLoss, use Decimal.TryParse to accept any fractional digits and exponential notation
            Dim decimalValue As Decimal

            Dim nfi As New NumberFormatInfo

            If value.Contains(".") Then
                nfi.NumberDecimalSeparator = "."
            ElseIf value.Contains(",") Then
                nfi.NumberDecimalSeparator = ","
            End If

            If Not Decimal.TryParse(value, NumberStyles.Float, nfi, decimalValue) Then
                Return False
            End If

            If decimalValue = Integer.MinValue OrElse decimalValue = Integer.MaxValue OrElse decimalValue < minValue OrElse decimalValue > maxValue Then
                Return False
            End If

        End If

        Return True

    End Function

End Class
