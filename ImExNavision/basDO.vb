Imports System.Data.SqlClient
Imports System.IO

Module basDO
    Public MyConnection As SqlConnection
    Public MyAnotherConnection As SqlConnection
    Public bConn As Boolean = False
    Public bAConn As Boolean = False
    Public Sub Createfile(ByVal message As String)
        Dim lstrADate As String = DateTime.Now.ToString("yyyyMMdd")
        Dim str As String = ""
        Dim sw As StreamWriter
        Dim sfolder As String = System.Windows.Forms.Application.StartupPath & "\DAWOODIMPLog"
        Dim sFileName As String = System.Windows.Forms.Application.StartupPath & "\DAWOODIMPLog\" & lstrADate & "Log.txt"

        If Not System.IO.Directory.Exists(sfolder) Then
            System.IO.Directory.CreateDirectory(sfolder)
        Else
            Dim yourRootDir As DirectoryInfo = New DirectoryInfo(sfolder)

            For Each file As FileInfo In yourRootDir.GetFiles()
                If file.LastWriteTime < DateTime.Now.AddDays(-30) Then file.Delete()
            Next
        End If

        str = message.ToString()
        message = (System.DateTime.Now & " " & str.ToString())

        If Not System.IO.File.Exists(sFileName) Then
            Dim fs As FileStream = New FileStream(sFileName, FileMode.Create, FileAccess.Write)
            Dim s As StreamWriter = New StreamWriter(fs)
            s.BaseStream.Seek(0, SeekOrigin.[End])
            s.WriteLine((lstrADate) & " " & message)
            s.Close()
        Else
            sw = File.AppendText(sFileName)
            sw.WriteLine((lstrADate) & " " & message)
            sw.Close()
        End If
    End Sub

    Public Sub ConnectDB()
        Dim MyConString As String = My.Settings.ConnectionString '"Data Source=IBIZ-JAGADISH\SQLEXPRESS;Initial Catalog=Sales;Integrated Security=True" 'My.Settings.ConnectionString   '"Dsn=IbizMobile;dbq=C:\My Projects\Chee Seng\Database\Sales.mdb;driverid=25;fil=MS Access;maxbuffersize=2048;pagetimeout=5"
        MyConnection = New SqlConnection(MyConString)
        MyConnection.Open()
        bConn = True
    End Sub

    Public Sub DisconnectDB()
        If bConn = True Then MyConnection.Close()
        bConn = False
    End Sub

    Public Sub ExecuteSQL(ByVal mySQL As String)
        Createfile(mySQL)
        Try
            Dim MyCommand As New SqlCommand
            If bConn = False Then ConnectDB()
            MyCommand.Connection = MyConnection
            MyCommand.CommandText = mySQL
            MyCommand.CommandTimeout = 0
            MyCommand.ExecuteNonQuery()
            MyCommand.Dispose()
        Catch ex As Exception
            'MsgBox(mySQL & vbCrLf & ex.Message)
            Createfile(ex.Message.ToString())
            'System.IO.File.AppendAllText("C:\ErrorLog.txt", Date.Now & " From ExecuteSQL: " & mySQL & vbCrLf & ex.Message & vbCrLf)
        End Try
    End Sub

    Public Function ReadRecord(ByVal mySQL As String) As SqlDataReader
        Createfile(mySQL)
        Dim MyCommand As New SqlCommand
        If bConn = False Then ConnectDB()

        Try
            MyCommand.Connection = MyConnection
            MyCommand.CommandText = mySQL
            MyCommand.CommandTimeout = 0
            ReadRecord = MyCommand.ExecuteReader
            MyCommand.Dispose()
        Catch MySqlException As SqlException
            ' MsgBox(MySqlException.ToString)
            Createfile(MySqlException.Message.ToString())
            'System.IO.File.AppendAllText("C:\ErrorLog.txt", Date.Now & " From REadRecord: " & mySQL & vbCrLf & MySqlException.Message & vbCrLf)
            Return Nothing
        Catch MyException As Exception
            ' MsgBox(MyException.ToString)
            Createfile(MyException.Message.ToString())
            Return Nothing
        End Try

    End Function



    Public Function GetCount(ByVal mySQL As String) As Integer
        Dim MyCommand As New SqlCommand
        If bConn = False Then ConnectDB()
        Try
            MyCommand.Connection = MyConnection
            MyCommand.CommandText = mySQL
            GetCount = MyCommand.ExecuteNonQuery
            MyCommand.Cancel()
        Catch MySqlException As SqlException

            'MsgBox(MySqlException.ToString)
        Catch MyException As Exception
            'MsgBox(MyException.ToString)
        End Try

    End Function

    Public Function SafeSQL(ByVal sSQL As String) As String
        Dim ii As Integer
        ii = InStr(1, sSQL, "'")
        Do While ii <> 0
            sSQL = Left(sSQL, ii) & "'" & Mid(sSQL, ii + 1)
            ii = InStr(ii + 2, sSQL, "'")
        Loop
        Return "'" & sSQL & "'"
    End Function

    Public Sub ConnectAnotherDB()
        Dim MyConString As String = My.Settings.ConnectionString '"Data Source=IBIZ-JAGADISH\SQLEXPRESS;Initial Catalog=Sales;Integrated Security=True" 'My.Settings.ConnectionString   '"Dsn=IbizMobile;dbq=C:\My Projects\Chee Seng\Database\Sales.mdb;driverid=25;fil=MS Access;maxbuffersize=2048;pagetimeout=5"
        MyAnotherConnection = New SqlConnection(MyConString)
        MyAnotherConnection.Open()
        bAConn = True
    End Sub

    Public Sub DisconnectAnotherDB()
        If bAConn = True Then
            MyAnotherConnection.Close()
            bAConn = False
        End If
    End Sub

    Public Function ReadRecordAnother(ByVal mySQL As String) As SqlDataReader
        Dim MyCommand As New SqlCommand
        Try
            MyCommand.Connection = MyAnotherConnection
            'MyCommand.Connection.ConnectionString = My.Settings.ConnectionString '"Data Source=IBIZ-JAGADISH\SQLEXPRESS;Initial Catalog=Sales;Integrated Security=True" 'My.Settings.ConnectionString  '"Dsn=IbizMobile;dbq=C:\My Projects\Chee Seng\Database\Sales.mdb;driverid=25;fil=MS Access;maxbuffersize=2048;pagetimeout=5"
            'MyCommand.Connection.Open()
            MyCommand.CommandText = mySQL
            MyCommand.CommandTimeout = 0
            ReadRecordAnother = MyCommand.ExecuteReader
            MyCommand.Dispose()
        Catch MySqlException As SqlException

            'MsgBox(MySqlException.ToString)
            System.IO.File.AppendAllText("C:\ErrorLog.txt", Date.Now & " From REadRecordAnother: " & mySQL & vbCrLf & MySqlException.Message & vbCrLf)

            Return Nothing
        Catch MyException As Exception
            'MsgBox(MyException.ToString)
            System.IO.File.AppendAllText("C:\ErrorLog.txt", Date.Now & " From REadRecordAnother: " & mySQL & vbCrLf & MyException.Message & vbCrLf)

            Return Nothing
        End Try

    End Function

    Public Sub ExecuteSQLAnother(ByVal mySQL As String)
        Createfile(mySQL)
        Try
            Dim MyCommand As New SqlCommand
            MyCommand.Connection = New SqlConnection
            MyCommand.Connection.ConnectionString = My.Settings.ConnectionString '"Data Source=IBIZ-JAGADISH\SQLEXPRESS;Initial Catalog=Sales;Integrated Security=True" 'My.Settings.ConnectionString  '"Dsn=IbizMobile;dbq=C:\My Projects\Chee Seng\Database\Sales.mdb;driverid=25;fil=MS Access;maxbuffersize=2048;pagetimeout=5"
            MyCommand.Connection.Open()
            MyCommand.CommandText = mySQL
            MyCommand.CommandTimeout = 0
            MyCommand.ExecuteNonQuery()
            MyCommand.Connection.Close()
            MyCommand.Dispose()
        Catch ex As Exception
            'MsgBox(ex.Message.ToString)
            Createfile(ex.Message.ToString)
            ' System.IO.File.AppendAllText("C:\ErrorLog.txt", Date.Now & " From ExecSQLAnother: " & mySQL & vbCrLf & ex.Message & vbCrLf)
        End Try
    End Sub
End Module
