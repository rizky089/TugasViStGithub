Imports System.Threading.Tasks

Public Class TrPeminjaman
    Dim dtDataPeminjam As DataTable

    Private Property DataPeminjam As String

    Private Sub TrPeminjaman_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initTrPeminjaman()
    End Sub

    Async Function loadSupplier() As Threading.Tasks.Task
        'ambil data supplier dan tampilkan dalam bentuk key - value 
        'seperti <option value="xxx">yyy</option> pada HTML
        MProgress.showProgress()

        Dim sql As String = "select idpeminjam, nmpeminjam, alamat, notelp from Peminjam"

        'ambil data table
        dtDataPeminjam = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        tb1.Text = DataPeminjam
        tb1.Text = "nmpeminjam"
        tb1.Text = "idpeminjam"
        MProgress.hideProgress()
    End Function

    Sub initTrPeminjaman()
        With lv
            .View = View.Details
            .GridLines = True
            .FullRowSelect = True
            .Columns.Clear()
            .Columns.Add("notransaksi", 100, HorizontalAlignment.Left)
            .Columns.Add("nmpeminjam", 30, HorizontalAlignment.Left)
            .Columns.Add("nmpinjaman", 4, HorizontalAlignment.Right)
            .Columns.Add("tglpinjaman", 12, HorizontalAlignment.Right)
            .Columns.Add("jumlah", 15, HorizontalAlignment.Right)
        End With
    End Sub

    Private Sub tb_TextChanged(sender As Object, e As EventArgs)
        Dim sid As String = tb.SelectedText.ToString

        'ambil alamat yang tertapung pada datatable 
        Try
            Dim row As DataRow() = dtDataPeminjam.Select("idpeminjam = " & sid)

            tb1.Text = row(0)(2) & vbCrLf & row(0)(3)   'vbCrLf = enter
        Catch ex As Exception
            Console.Write(ex.ToString)
        End Try
    End Sub

    Sub hitungtotal()
        Dim subtotal, total As Long
        For i As Integer = 0 To lv.Items.Count - 1
            subtotal += Val(lv.Items(i).SubItems(4).Text)

        Next
        total = subtotal
        tb3.Text = subtotal : tb3.Text = total
    End Sub

    Private Sub lv_MouseDown(sender As Object, e As MouseEventArgs)
        If lv.Items.Count = 0 Then Exit Sub


        If e.Button = Windows.Forms.MouseButtons.Right Then
            ContextMenuStrip.Show(MousePosition.X, MousePosition.Y)
        End If
    End Sub

    Private Sub tb3_TextChanged(sender As Object, e As EventArgs)
        hitungtotal()
    End Sub

    Private Async Sub tb_Click(sender As Object, e As EventArgs) Handles btn.Click
        If btn.Text = "Simpan" Then
            sql = "insert into TrPeminjaman (tanggal, id_supplier) values ( " & _
                "'" & dtp.Value & "', " & tb1.SelectedText.ToString & ", " & _
                "')"
            'simpan
            MProgress.showProgress()
            Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
            Task.WaitAll(myTask) 'wait

            'ambil ID
            MProgress.hideProgress()
            MsgBox("Data Berhaisl Disimpan", , "Pesan")

        ElseIf btn.Text = "Perbarui" Then
        End If

        initform()
    End Sub

    Sub initform()
        lv.Items.Clear()
        tb.Text = "0"
        tb1.Text = "0"
        tb2.Text = "0"
        tb3.Text = Nothing
        dtp.Value = Date.Now
    End Sub
End Class