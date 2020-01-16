Imports System.Threading.Tasks
Imports System.Security.Cryptography
Imports System.Text
Public Class DataPinjaman
    Dim tempID As Integer
    Dim lst As ListViewItem

    Private Sub DataPinjaman_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadGrid(Nothing)
    End Sub

    Private Sub btn_Click(sender As Object, e As EventArgs) Handles btn.Click
        Dim sql As String

        If tempID = 0 Then
            sql = "insert into Pinjaman (nopinjaman, nmpinjaman, tglpinjaman, jumlah) " & _
            "values ('" & tb.Text.Trim & "' , '" & tb1.Text.Trim & "', " & _
            "'" & dtp.Text.Trim & "' , '" & tb2.Text.Trim & " ')"
        Else
            sql = "update Pinjaman set nopinjaman = '" & tb1.Text.Trim & "', " & _
                "nmpinjaman = '" & tb1.Text.Trim & "', tglpinjaman = '" & dtp.Text.Trim & "' " & _
                "', jumlah = '" & dtp.Text.Trim & "' , where nopinjaman = " & tempID
        End If

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub kosong()
        tempID = Nothing
        tb.Text = Nothing
        tb1.Text = Nothing
        tb2.Text = Nothing
        dtp.Value = Date.Now
    End Sub

    Async Function loadGrid(ByVal cari As String) As Task
        MProgress.showProgress(ProgressBar1)

        Dim sql As String

        If cari = Nothing Then
            sql = "select * from Pinjaman"
        Else
            sql = "select * from Pinjaman " & _
                    "where nmpinjaman like '%" & cari & "%'"
        End If

        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        lv.Items.Clear()
        For Each dr As DataRow In dt.Rows
            lst = lv.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(2))
            lst.SubItems.Add(dr(3))

        Next

        tempID = Nothing
        MProgress.hideProgress(ProgressBar1)
    End Function

    Private Sub btn1_Click(sender As Object, e As EventArgs) Handles btn1.Click
        Dim sql As String = "delete from Pinjaman where nopinjaman = " & tempID

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub lv_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lv.MouseDoubleClick
        With lv
            tempID = .SelectedItems.Item(0).Text
            tb.Text = .SelectedItems.Item(0).SubItems(1).Text
            tb1.Text = .SelectedItems.Item(0).SubItems(2).Text
            tb2.Text = .SelectedItems.Item(0).SubItems(3).Text
            dtp.Text = .SelectedItems.Item(0).SubItems(4).Text
        End With
    End Sub

    Private Sub btn2_Click(sender As Object, e As EventArgs) Handles btn2.Click
        Call loadGrid(tb2.Text.Trim)
    End Sub

End Class