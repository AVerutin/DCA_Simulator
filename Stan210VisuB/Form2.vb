Public Class Form2

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim Level3ID As Long, BaseID As Long, RollingID As Long, IngotID As Long, tt, th As Long
        Dim IngotX As Double, IngotX2 As Double, IngotY As Double, IngotY2 As Double

        Dim i As Long
        Dim s As String
        Dim n As Long
        Dim vt As Boolean
        n = 0
        n = Form1.m_link.IngotsCount
        eucount.Text = n

        If Not StopUpdate.Checked Then
            ListView1.Clear()
            For i = 0 To Form1.m_link.IngotsCount - 1 Step 1
                Form1.m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
                IngotX = System.Math.Round(IngotX * 10) / 10
                IngotX2 = System.Math.Round(IngotX2 * 10) / 10
                vt = Form1.m_link.GetVisuTrackState(IngotID)
                s = " IngotID=" & IngotID & " X=" & IngotX & " X2=" & IngotX2 & " Y=" & IngotY & " Y2=" & IngotY2 & " Th=" & th _
                & " L3ID=" & Level3ID & " BID=" & BaseID & " RID=" & RollingID & " VT=" & vt
                If th = cr_th.SelectedIndex Or Not f2_thshow.Checked Then
                    If Not DntShowMP.Checked Then
                        ListView1.Items.Add(s)
                    Else
                        If th < 3 Or th > 3 Then
                            ListView1.Items.Add(s)
                        End If
                    End If
                End If
            Next i
        End If
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteIngot.Click
        Form1.m_link.DeleteIngot(DeleteIngotID.Text)
    End Sub

End Class