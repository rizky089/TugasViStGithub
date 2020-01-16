Imports System.Threading.Tasks

Public Class DataPeminjam
    Dim tempID As Integer
    Dim lst As ListViewItem
    Private Sub DataPeminjam_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadGrid(Nothing)
    End Sub

    Private Sub btn_Click(sender As Object, e As EventArgs) Handles btn.Click
        Dim sql As String

        If tempID = 0 Then
            sql = "insert into Peminjam (idpeminjam, nmpeminjam, alamat, notelp) " & _
            "values ('" & TextBox1.Text.Trim & "' ,'" & tb.Text.Trim & "' , '" & tb1.Text.Trim & "', " & _
            "'" & tb2.Text.Trim & "')"
        Else
            sql = "update Peminjam set idpeminjam = '" & TextBox1.Text.Trim & "', nmpeminjam = '" & tb.Text.Trim & "' , " & _
                "alamat = '" & tb1.Text.Trim & "', notelp = '" & tb2.Text.Trim & "' " & _
                "where idpeminjam = " & tempID
        End If

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Sub kosong()
        tempID = 0
        tb.Text = Nothing
        tb1.Text = Nothing
        tb2.Text = Nothing
    End Sub

    Private Sub btn1_Click(sender As Object, e As EventArgs) Handles btn1.Click
        Dim sql As String = "delete from Peminjam where idpeminjam = " & tempID

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub lv_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        With lv
            tempID = .SelectedItems.Item(0).Text
            tb.Text = .SelectedItems.Item(0).SubItems(1).Text
            tb1.Text = .SelectedItems.Item(0).SubItems(2).Text
            tb2.Text = .SelectedItems.Item(0).SubItems(3).Text
        End With
    End Sub
    Async Function loadGrid(ByVal cari As String) As Task
        MProgress.showProgress(ProgressBar1)

        Dim sql As String

        If cari = Nothing Then
            sql = "select * from Peminjam"
        Else
            sql = "select * from Peminjam where nmpeminjam like '%" & cari & "%' " & _
                    "or alamat like '%" & cari & "%' or notelp like '%" & cari & "%'"
        End If

        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        lv.Items.Clear()

        For Each dr As DataRow In dt.Rows
            lst = lv.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(2))
            lst.SubItems.Add(dr(3))
        Next

        tempID = 0
        MProgress.hideProgress(ProgressBar1)
    End Function

    Private Sub btn2_Click(sender As Object, e As EventArgs) Handles btn2.Click
        Call loadGrid(tb2.Text.Trim)
    End Sub
End Class