Public Class FMenu

    Private Sub btn_Click(sender As Object, e As EventArgs) Handles btn.Click
        DataPeminjam.Show()
    End Sub

    Private Sub btn1_Click(sender As Object, e As EventArgs) Handles btn1.Click
        DataPinjaman.Show()
    End Sub

    Private Sub btn2_Click(sender As Object, e As EventArgs) Handles btn2.Click
        TrPeminjaman.Show()
    End Sub
End Class