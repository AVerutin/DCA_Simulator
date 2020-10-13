
Imports System.IO
Imports Npgsql



Public Class Form1

    Dim starttime As Double
    Dim rand As New System.Random
    Dim CrushIngotsCount = 0
    Dim FFBLowSpeedIID As Integer = 0

    Private Sub MainTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainTimer.Tick


        'цикл
        timelabel.Text = Today & " " & TimeOfDay
        m_link.UpdateMTSData()
        mlinkcount.Text = m_link.IngotsCount

        'обновление отрисованных объектов с меньшей частотой
        Dim nowtime As Double
        nowtime = Microsoft.VisualBasic.DateAndTime.Timer
        If (nowtime - starttime > 1) Or (nowtime < starttime) Then
            starttime = Microsoft.VisualBasic.DateAndTime.Timer
            Fur.Refresh()
        End If

        'время ссм
        Dim tssm = m_link.GetMTSTime()
        Dim VBSSMTime As Date = DateAdd("s", tssm, "01/01/1970 00:00:00")
        Dim VBSSMTimeLocal As Date = VBSSMTime.ToLocalTime()
        SSMTimeString.Text = VBSSMTimeLocal.ToString()

        'отображение состояние связи

        dtw.Text = m_link.get_GetSignalValue(777)
        www.Text = m_link.get_GetSignalValue(4000)
        furh.Text = m_link.get_GetSignalValue(4001)
        furm.Text = m_link.get_GetSignalValue(4002)
        mill.Text = m_link.get_GetSignalValue(4003)
        mille.Text = m_link.get_GetSignalValue(4004)
        line.Text = m_link.get_GetSignalValue(4005)
        sund.Text = m_link.get_GetSignalValue(4006)
        LblSimWdg.Text = m_link.get_GetSignalValue(25102)

        LblWDCL1.Text = m_link.get_GetSignalValue(25400)
        MtsKAWD.Text = m_link.get_GetSignalValue(25401)

        CheckDBClearTaskSubShowResult()
        CalcPaspDBTaskSubShowResult()

        ' отображение сигналов
        lblInWeight.Text = Format(m_link.get_GetSignalValue(4100), "f")
        lblWeightOut.Text = Format(m_link.get_GetSignalValue(4101), "f")

        lblTFur0.Text = Format(m_link.get_GetSignalValue(4102), "f")
        lblTFur1.Text = Format(m_link.get_GetSignalValue(4103), "f")
        lblTFur2.Text = Format(m_link.get_GetSignalValue(4104), "f")
        lblTFur3.Text = Format(m_link.get_GetSignalValue(4105), "f")
        lblTFur4.Text = Format(m_link.get_GetSignalValue(4106), "f")
        lblTFur5.Text = Format(m_link.get_GetSignalValue(4107), "f")
        lblTFur6.Text = Format(m_link.get_GetSignalValue(4108), "f")

        lblTFurOut.Text = Format(m_link.get_GetSignalValue(4109), "f")
        lblCutHydrosbiv.Text = Format(m_link.get_GetSignalValue(4110), "f")
        lblStand18.Text = Format(m_link.get_GetSignalValue(4111), "f")
        lblFFB.Text = Format(m_link.get_GetSignalValue(4112), "f")
        lblTFH.Text = Format(m_link.get_GetSignalValue(4113), "f")

        lblR4114.Text = Format(m_link.get_GetSignalValue(4114), "f")
        lblR4115.Text = Format(m_link.get_GetSignalValue(4115), "f")
        lblR4116.Text = Format(m_link.get_GetSignalValue(4116), "f")
        lblR4117.Text = Format(m_link.get_GetSignalValue(4117), "f")
        lblR4118.Text = Format(m_link.get_GetSignalValue(4118), "f")
        lblR4119.Text = Format(m_link.get_GetSignalValue(4119), "f")
        lblR4120.Text = Format(m_link.get_GetSignalValue(4120), "f")
        lblR4121.Text = Format(m_link.get_GetSignalValue(4121), "f")
        lblR4122.Text = Format(m_link.get_GetSignalValue(4122), "f")
        lblR4123.Text = Format(m_link.get_GetSignalValue(4123), "f")

        LblSbiv.Visible = m_link.get_GetSignalValue(4300)

        'отображение простоев
        If m_link.get_GetSignalValue(25305) <> 0 Then
            lblStanState.Text = "РАБОТА"
        Else
            lblStanState.Text = "ПРОСТОЙ"
        End If

        'смена бригада
        lblSmena.Text = m_link.get_GetSignalValue(25306)
        lblBrigada.Text = m_link.get_GetSignalValue(25307)

        'наработка за чистовой
        LblFineWorkedP.Text = m_link.get_GetSignalValue(25310)
        LblFineWorkedW.Text = Format(m_link.get_GetSignalValue(25311), "f")

        ' выполнение эмуляционного кода заменяющего части системы отсутвующие при ПНР
        If cbAddEnter.Checked And m_link.get_CheckSignal(777) Then
            Emulation()
        End If

        'вентиляторы охлаждения
        LblVent1.Visible = m_link.get_GetSignalValue(4304)
        LblVent2.Visible = m_link.get_GetSignalValue(4305)
        LblVent3.Visible = m_link.get_GetSignalValue(4306)
        LblVent4.Visible = m_link.get_GetSignalValue(4307)
        LblVent5.Visible = m_link.get_GetSignalValue(4308)
        LblVent6.Visible = m_link.get_GetSignalValue(4309)



    End Sub


    Private Sub Emulation()

        Dim Level3ID As Long, BaseID As Long, RollingID As Long, IngotID As Long, tt As Long, th As Long
        Dim IngotX As Double, IngotX2 As Double, IngotY As Double, IngotY2 As Double

        'ватчдог симуляции
        Dim valwds = m_link.get_GetSignalValue(25102)
        valwds += 1
        If valwds > 10000 Then
            valwds = 0
        End If
        m_link.SetSignalDoubleValue(25102, valwds)

        'Эмуляция работы операторов ''''''''''''''''''''''''''''''''''''''''''''''''''''

        'автодобавление штук
        Dim tssm = m_link.GetMTSTime()
        Static ssmtAddEnter As Double = 0
        If (tssm - ssmtAddEnter > My.Settings.INSERTTIME) And cbInsertInFur.Checked Then
            m_link.AddIngot2D(0, 1, 0, 0, 12, 0, 0, 2, 2)
            ssmtAddEnter = tssm
        End If

        'перевалки
        Static Dim reinstall As Boolean = False
        Static Dim timereinstall As Double = tssm

        If cbReinstallControl.Checked Then

            cbInsertInFur.Enabled = False

            Dim _TIMEREINSTALL As Double = My.Settings.TIMEREINSTALL
            Dim _TIMETOREINSTALL As Double = My.Settings.TIMETOREINSTALL

            If reinstall Then
                'сейчас перевалка
                If tssm - timereinstall > _TIMEREINSTALL Then
                    reinstall = False
                    timereinstall = tssm
                    CbReturn.Checked = True 'запуск возвратов
                    CrushIngotsCount = 1 + Math.Round(rand.NextDouble())
                End If
            Else
                'сейчас прокатка
                If tssm - timereinstall > _TIMETOREINSTALL Then
                    reinstall = True
                    timereinstall = tssm
                End If
            End If

            'отображение перевалок
            If reinstall Then
                lblReinstallState.Text = "ПЕРЕВАЛКА"
                cbInsertInFur.Checked = False
                LblReinstallTime.Text = Format((_TIMEREINSTALL - tssm + timereinstall), "f")
            Else
                lblReinstallState.Text = "ПРОКАТКА"
                cbInsertInFur.Checked = True
                LblReinstallTime.Text = Format((_TIMETOREINSTALL - tssm + timereinstall), "f")
            End If
        Else
            cbInsertInFur.Enabled = True
        End If

        'управление режимом возвратов
        Static startreturntime As Double = 0
        Dim _TIMERETURN As Double = 120
        If CbReturn.Checked Then

            m_link.SetSignalDoubleValue(25309, 1)
            LblReturn.Text = "ВОЗВРАТЫ"

            If startreturntime = 0 Then
                startreturntime = tssm
            End If

            Dim iid = GetNearRightIngotX(5, -1)
            m_link.GetIngot2D(iid, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
            Dim IsIngotOnReturn As Boolean = (IngotX < 36) And iid <> 0

            LblReturnTime.Text = Format(_TIMERETURN - (tssm - startreturntime), "f")

            If tssm - startreturntime > _TIMERETURN And Not IsIngotOnReturn Then 'окончание режима возвратов
                CbReturn.Checked = False
            End If

        Else
            m_link.SetSignalDoubleValue(25309, 0)
            LblReturn.Text = "НЕТ ВОЗВРАТОВ"
            LblReturnTime.Text = "-"
            startreturntime = 0
        End If

        'терракты по бурежкам
        LblCrashIngotsCount.Text = CrushIngotsCount
        Static prevcing As Integer = 0
        If CbCrashIngots.Checked And CrushIngotsCount > 0 Then
            Dim cing = GetIngotInPoint(5, 75)
            If cing <> 0 And prevcing <> cing Then
                prevcing = cing
                m_link.DeleteIngot(cing)
                CrushIngotsCount -= 1
            End If
        End If

        'автоматические резы
        Static iidcutted3 As Integer = 0
        If CbAutoCut.Checked Then
            Dim iidcut3 = GetIngotInPoint(5, 171 - 1)
            If iidcut3 <> 0 And iidcut3 <> iidcutted3 Then
                m_link.GetIngot2D(iidcut3, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
                If IngotX2 > 268 + 10 Then 'голова в трайбе и коррекция чтобы попасть резом в середину штуки
                    m_link.SetSignalDoubleValue(4132, (m_link.get_GetSignalValue(4132) + 1) Mod 1000) 'нажать кнопку
                    iidcutted3 = iidcut3
                End If
            End If
        Else
            FFBLowSpeedIID = 0
        End If



        ' Эмуляция работы КА ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'параметр веса на входе в печь
        Static Dim IngotIDWSaved = 0
        IngotID = GetIngotInPoint(2, 24)
        Dim wval As Double = m_link.get_GetSignalValue(4100)
        If IngotID <> 0 And wval <> 0 And IngotIDWSaved <> IngotID Then
            m_link.SetIngotParam(IngotID, 1000, wval)
            IngotIDWSaved = IngotID
            lblWINShow.Text = "ID=" + Format(IngotID, "g") + " W=" + Format(wval, "f")
        End If

        'уход в печь
        Static moveInFurTime As Double = 0
        Dim ing = GetIngotInPoint(2, 47.9)
        If ing <> 0 And m_link.get_CheckSignal(25302) And tssm - moveInFurTime > 2 Then
            'движение в печи
            m_link.SetSignalDoubleValue(25302, m_link.get_GetSignalValue(25302) + 1)
            'перенос в печь
            m_link.IngotCoordinateCorrection2D(ing, 0, 0.13, 0, 12, 3, 3)

            moveInFurTime = tssm
        End If

        'выход из печи
        ing = GetNearRightIngotX(3, 19.5)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 19.7, 31.5, 0, 0, 5, 5)
            m_link.SetIngotParam(ing, 1001, m_link.get_GetSignalValue(25306))
        End If

        'уход с реформинга
        ing = GetNearRightIngotX(5, 358)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 7, 9, 0, 0, 6, 6)
        End If


        'уход с вертикального приемного
        ing = GetNearRightIngotX(6, 9.9)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 0, 2, 0, 0, 7, 7)
        End If

        'уход с вертикального нижнего
        ing = GetNearRightIngotX(7, 3.9)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 0, 2, 0, 0, 9, 9)
        End If

        'уход с вертикального выходного
        ing = GetNearRightIngotX(9, 9.9)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 0, 2, 0, 0, 10, 10)
        End If

        'уход с вертикального верхнего
        ing = GetNearRightIngotX(10, 3.9)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 0, 2, 0, 0, 6, 6)
        End If

        'уход с вертикального выходного на тележку
        ing = GetNearRightIngotX(9, 5.0)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 0, 2, 0, 0, 11, 11)
        End If

        'уход с тележки на горизонтальный приемный
        ing = GetNearRightIngotX(11, 3.9)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 5, 7, 0, 0, 12, 12)
        End If

        'уход на горионтальный верхний
        ing = GetNearRightIngotX(12, 21.9)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 0, 2, 0, 0, 13, 13)
        End If

        'уход на горионтальный обвязочный
        ing = GetNearRightIngotX(13, 12.9)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 0, 2, 0, 0, 14, 14)
        End If

        'уход на горионтальный обвязочный
        ing = GetNearRightIngotX(14, 21.9)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 0, 2, 0, 0, 15, 15)
        End If

        'уход на тележку 
        ing = GetNearRightIngotX(15, 6.4)
        If ing <> 0 Then
            m_link.IngotCoordinateCorrection2D(ing, 0, 2, 0, 0, 16, 16)
        End If

        'уход с тележки на шлеппер
        Static moveCarShlepperTime As Double = 0
        ing = GetNearRightIngotX(16, 7.9)
        If ing <> 0 And m_link.get_CheckSignal(25304) And tssm - moveCarShlepperTime > 2 Then
            m_link.SetSignalDoubleValue(25304, m_link.get_GetSignalValue(25304) + 1)
            m_link.IngotCoordinateCorrection2D(ing, 0, 0.9, 0, 0, 17, 17)
            moveCarShlepperTime = tssm
        End If

        'съем с тележки
        Dim i As Integer
        Static delSlepTime As Double = 0
        ing = GetNearRightIngotX(17, 7.9)
        If ing <> 0 And tssm - delSlepTime > 2.0 Then
            For i = 0 To m_link.IngotsCount - 1 Step 1
                m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
                If th = 17 Then
                    m_link.DeleteIngot(IngotID)
                End If
            Next
            delSlepTime = tssm
        End If


        'определение простоя стана
        Static Dim LastStanWorkTime As Double = 0
        If GetIngotInPoint(5, 36.5) <> 0 Then
            LastStanWorkTime = tssm
        End If
        If tssm - LastStanWorkTime < My.Settings.STOPSTATETIME Then
            m_link.SetSignalDoubleValue(25305, 1)
        Else
            m_link.SetSignalDoubleValue(25305, 0)
        End If


        'генерация смены и бригады
        Dim ssmtime As DateTime = DateAdd("s", tssm, "01/01/1970 00:00:00").ToLocalTime()
        Dim hour As Integer = ssmtime.Hour
        Dim smena As Integer
        If hour >= 8 And hour < 20 Then
            smena = 1
        Else
            smena = 2
        End If
        m_link.SetSignalDoubleValue(25306, Val(smena))

        Dim sm1 As String = "1432"
        Dim sm2 As String = "2143"
        Dim StartUniverseBrigaden As DateTime = "01/04/1900 08:00:00"
        Dim calcbrig As Integer
        Dim brigada As String = "0"
        calcbrig = (ssmtime - StartUniverseBrigaden).Days Mod 4

        If smena = 1 Then
            brigada = sm1.Chars(calcbrig)
        Else
            If smena = 2 Then
                brigada = sm2.Chars(calcbrig)
            End If
        End If
        m_link.SetSignalDoubleValue(25307, Val(brigada))

        'резка'''

        'за черновой
        Static WaitState4130 As Double = 0
        Static SavedBID4130 As Integer
        Dim X4130 As Double = 75.0
        If m_link.get_CheckSignal(4130) Then

            Static Dim prevcutrought4130 = m_link.get_GetSignalValue(4130)
            If prevcutrought4130 <> m_link.get_GetSignalValue(4130) Then
                Dim iid As Integer = GetIngotInPoint(5, X4130)
                If iid <> 0 Then
                    m_link.GetIngot2D(iid, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
                    m_link.IngotCoordinateCorrection2D(iid, IngotX, X4130, 0, 0, 5, 5)
                    m_link.AddIngot2D(0, 1, 4130777, X4130, IngotX2, 0, 0, 5, 5)
                    WaitState4130 = tssm
                    SavedBID4130 = BaseID
                End If
            End If
            prevcutrought4130 = m_link.get_GetSignalValue(4130)

            If WaitState4130 <> 0 Then
                If tssm - WaitState4130 > 10.0 Then
                    WaitState4130 = 0
                End If

                For i = 0 To m_link.IngotsCount - 1 Step 1
                    m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
                    If RollingID = 4130777 And BaseID > 1 Then
                        m_link.IngotIDCorrection(IngotID, Level3ID, BaseID, 0)
                        m_link.SetIngotParam(IngotID, 10000, SavedBID4130)
                        WaitState4130 = 0
                        Exit For
                    End If
                Next i
            End If

        End If

        'за промежуточной
        Static WaitState4131 As Double = 0
        Static SavedBID4131 As Integer
        Dim X4131 As Double = 100.0
        If m_link.get_CheckSignal(4131) Then

            Static Dim prevcutrought4131 = m_link.get_GetSignalValue(4131)
            If prevcutrought4131 <> m_link.get_GetSignalValue(4131) Then
                Dim iid As Integer = GetIngotInPoint(5, X4131)
                If iid <> 0 Then
                    m_link.GetIngot2D(iid, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
                    m_link.IngotCoordinateCorrection2D(iid, IngotX, X4131, 0, 0, 5, 5)
                    m_link.AddIngot2D(0, 1, 4131777, X4131, IngotX2, 0, 0, 5, 5)
                    WaitState4131 = tssm
                    SavedBID4131 = BaseID
                End If
            End If
            prevcutrought4131 = m_link.get_GetSignalValue(4131)

            If WaitState4131 <> 0 Then
                If tssm - WaitState4131 > 10.0 Then
                    WaitState4131 = 0
                End If

                For i = 0 To m_link.IngotsCount - 1 Step 1
                    m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
                    If RollingID = 4131777 And BaseID > 1 Then
                        m_link.IngotIDCorrection(IngotID, Level3ID, BaseID, 0)
                        m_link.SetIngotParam(IngotID, 10000, SavedBID4131)
                        WaitState4131 = 0
                        Exit For
                    End If
                Next i
            End If

        End If


        'перед скоростным блоком прокатки FFB
        Static WaitState4132 As Double = 0
        Static SavedBID4132 As Integer
        Dim X4132 As Double = 171.0
        If m_link.get_CheckSignal(4132) Then

            Static Dim prevcutrought4132 = m_link.get_GetSignalValue(4132)
            If prevcutrought4132 <> m_link.get_GetSignalValue(4132) Then
                Dim iid As Integer = GetIngotInPoint(5, X4132)
                If iid <> 0 Then
                    m_link.GetIngot2D(iid, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
                    m_link.IngotCoordinateCorrection2D(iid, IngotX, X4132, 0, 0, 5, 5)
                    FFBLowSpeedIID = iid 'снизить скорость линии проволки для второй штуки
                    m_link.AddIngot2D(0, 1, 4132777, 181, IngotX2, 0, 0, 5, 5)
                    WaitState4132 = tssm
                    SavedBID4132 = BaseID
                End If
            End If
            prevcutrought4132 = m_link.get_GetSignalValue(4132)

            If WaitState4132 <> 0 Then
                If tssm - WaitState4132 > 10.0 Then
                    WaitState4132 = 0
                End If

                For i = 0 To m_link.IngotsCount - 1 Step 1
                    m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
                    If RollingID = 4132777 And BaseID > 1 Then
                        m_link.IngotIDCorrection(IngotID, Level3ID, BaseID, 0)
                        m_link.SetIngotParam(IngotID, 10000, SavedBID4132)
                        WaitState4132 = 0
                        Exit For
                    End If
                Next i
            End If

        End If

        'параметр веса на вsходе в SUNDCO
        Static IngotIDWSSaved = 0
        IngotID = GetIngotInPoint(15, 4)
        Dim wsval As Double = m_link.get_GetSignalValue(4101)
        If IngotID <> 0 And wsval <> 0 And IngotIDWSSaved <> IngotID Then
            m_link.SetIngotParam(IngotID, 1002, wsval)
            IngotIDWSSaved = IngotID
            LblWSShow.Text = "ID=" + Format(IngotID, "g") + " W=" + Format(wsval, "f")
        End If

        'наработки за чистовой
        Static IngotIDFWSaved = 0
        IngotID = GetIngotInPoint(5, 120)
        If IngotID <> 0 And IngotIDFWSaved <> IngotID And m_link.get_CheckSignal(25310) And m_link.get_CheckSignal(25311) Then

            IngotIDFWSaved = IngotID
            Dim iidworked As Integer = m_link.get_GetSignalValue(25310) + 1
            If iidworked > 999999 Then
                iidworked = 0
            End If
            m_link.SetSignalDoubleValue(25310, iidworked)

            Dim weightworked As Double = m_link.get_GetSignalValue(25311) + m_link.GetIngotParamWait(IngotID, 1000, 500) / 1000.0
            If weightworked > 999999.9 Then
                weightworked = 0.0
            End If
            m_link.SetSignalDoubleValue(25311, weightworked)

        End If



        ' Эмуляция поступающих данных от ПЛК ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'генерация случайных сигналов
        SetSimSignal(4100, 2, 24, 1500, 1600, 1550, 0) ' вес по посаду
        SetSimSignal(4101, 15, 4, 1400, 1500, 1450, 0) ' вес бунта

        SetSimSignal(4109, 5, 36.5, 970, 1150, 910, 600) ' температура заготовки на выходе из печи
        SetSimSignal(4110, 5, 48, 900, 920, 910, 600) ' температура горячего металла перед ножницами
        SetSimSignal(4111, 5, 122, 600, 620, 610, 250) ' температурный контроль, клеть №18
        SetSimSignal(4112, 5, 175, 600, 620, 610, 250) ' пирометр на входе в блок FFB BGV10P
        SetSimSignal(4113, 5, 269, 600, 620, 610, 250) ' термодатчик горяч металла перед линией охлаждения

        'датчики наличия металла
        SetSensSignal(4221, 2, 9.0, False)
        SetSensSignal(4222, 2, 25.0, False)
        SetSensSignal(4223, 2, 31.0, False)

        SetSensSignal(4230, 3, 20.0, False)

        SetSensSignal(4255, 5, 35.5, False)

        SetSensSignal(4257, 5, 45.8, False)

        SetSensSignal(4259, 5, 56.0, False)
        SetSensSignal(4260, 5, 61.0, False)
        SetSensSignal(4261, 5, 66.0, False)
        SetSensSignal(4262, 5, 71.0, False)
        SetSensSignal(4263, 5, 73.0, False)
        SetSensSignal(4264, 5, 77.0, False)
        SetSensSignal(4265, 5, 79.0, False)
        SetSensSignal(4266, 5, 83.6, False)
        SetSensSignal(4267, 5, 89.6, False)
        SetSensSignal(4268, 5, 94.4, False)
        SetSensSignal(4269, 5, 97.8, False)
        SetSensSignal(4270, 5, 102.0, False)
        SetSensSignal(4271, 5, 105.5, False)
        SetSensSignal(4272, 5, 108.5, False)
        SetSensSignal(4273, 5, 111.5, False)
        SetSensSignal(4274, 5, 114.5, False)
        SetSensSignal(4275, 5, 117.5, False)
        SetSensSignal(4276, 5, 120.0, False)

        SetSensSignal(4278, 5, 170.0, False)
        SetSensSignal(4279, 5, 172.0, False)

        SetSensSignal(4281, 5, 184.0, False)
        SetSensSignal(4282, 5, 267.0, False)

        SetSensSignal(4284, 5, 348.0, False)
        SetSensSignal(4285, 5, 348.0, False)

        SetSensSignal(4300, 5, 47.0, False) ' гидросбив
        m_link.SetSignalDoubleValue(4310, GetIngotInPoint(2, 29) = 0 And GetIngotInPoint(2, 31) = 0) ' аварийная дверца в печи
        m_link.SetSignalDoubleValue(4301, GetIngotInPoint(5, 14) = 0 And GetIngotInPoint(5, 16) = 0) ' аварийная дверца в печи
        m_link.SetSignalDoubleValue(4302, GetIngotInPoint(5, 35) = 0 And GetIngotInPoint(5, 37) = 0) ' выходная дверца в печи

        'сигналы не требующие точного соответcтвия прохождению штуки
        Static Dim LastSigSim As Double = 0
        If tssm - LastSigSim > 1.0 Then
            LastSigSim = tssm

            'генерация ватчдогов
            If cbWWD.Checked Then
                m_link.SetSignalDoubleValue(4000, (m_link.get_GetSignalValue(4000) + 1) Mod 1000)
            End If
            If cbFurH.Checked Then
                m_link.SetSignalDoubleValue(4001, (m_link.get_GetSignalValue(4001) + 2) Mod 1000)
            End If
            If cbFurM.Checked Then
                m_link.SetSignalDoubleValue(4002, (m_link.get_GetSignalValue(4002) + 3) Mod 1000)
            End If
            If cbMILL.Checked Then
                m_link.SetSignalDoubleValue(4003, (m_link.get_GetSignalValue(4003) + 4) Mod 1000)
            End If
            If cbMILLE.Checked Then
                m_link.SetSignalDoubleValue(4004, (m_link.get_GetSignalValue(4004) + 5) Mod 1000)
            End If
            If cbLine.Checked Then
                m_link.SetSignalDoubleValue(4005, (m_link.get_GetSignalValue(4005) + 6) Mod 1000)
            End If
            If cbSUND.Checked Then
                m_link.SetSignalDoubleValue(4006, (m_link.get_GetSignalValue(4006) + 7) Mod 1000)
            End If

            'температуры в печах
            SetSimSignal(4102, -1, 0, 800, 900, 850, 0) ' Температура печи. Зона  безогневая -0
            SetSimSignal(4103, -1, 0, 800, 900, 850, 0) ' Температура печи. Зона 1 верхнего предварительного нагрева.
            SetSimSignal(4104, -1, 0, 800, 900, 850, 0) ' Температура печи. Зона 2 нижнего предварительного нагрева.
            SetSimSignal(4105, -1, 0, 1100, 1200, 1150, 0) ' Температура печи. Зона 3 верхнего нагрева
            SetSimSignal(4106, -1, 0, 1100, 1200, 1150, 0) ' Температура печи. Зона 4 нижнего нагрева
            SetSimSignal(4107, -1, 0, 1180, 1200, 1190, 0) ' Температура печи. Зона 5 верхнего томления.
            SetSimSignal(4108, -1, 0, 1180, 1200, 1190, 0) ' Температура печи. Зона 6 нижнего томления.

            'расходы на стане
            SetSimSignal(4114, -1, 0, 0, 16000, 11000, 11000) 'Расход  природного газа - печь
            SetSimSignal(4115, -1, 0, 140, 160, 150, 150) 'Расход воды охлаждения (суммарный) - печь
            SetSimSignal(4116, -1, 0, 17500, 17700, 17600, 17600) 'Расход воздуха горения - печь
            SetSimSignal(4117, -1, 0, 1000, 1500, 1250, 1250) 'Расход масла (суммарный) - в черн стане
            SetSimSignal(4118, -1, 0, 1000, 1500, 1250, 1250) 'Расход масла (суммарный).- промеж стан
            SetSimSignal(4119, -1, 0, 1000, 1500, 1250, 1250) 'Расход масла (суммарный) - отделочный стан
            SetSimSignal(4120, -1, 0, 140, 160, 150, 150) 'Расход подаваемой воды - отделочный стан
            SetSimSignal(4121, -1, 0, 1000, 1500, 1250, 1250) 'Расход масла (суммарный) - чистовой стан
            SetSimSignal(4122, -1, 0, 15, 25, 20, 20) 'Расход масла (суммарный)- уч. термообработки
            SetSimSignal(4123, -1, 0, 190, 210, 200, 200) 'Расход подаваемой воды (суммарный) - уч. термообработки

            'включение вентиляторов - перенесено в MtsClient 1
            'IngotID = GetNearRightIngotX(5, 269)
            'm_link.GetIngot2D(IngotID, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
            'Dim venton As Boolean = IngotID <> 0 And IngotX < 350 And tt = 5
            'm_link.SetSignalDoubleValue(4304, venton)
            'm_link.SetSignalDoubleValue(4305, venton)
            'm_link.SetSignalDoubleValue(4306, venton)
            'm_link.SetSignalDoubleValue(4307, venton)
            'm_link.SetSignalDoubleValue(4308, venton)
            'm_link.SetSignalDoubleValue(4309, venton)

            'скорость рольгангв выдачи при режиме возвратов
            If m_link.get_GetSignalValue(25309) Then
                m_link.SetSignalDoubleValue(4303, -1.0)
            Else
                m_link.SetSignalDoubleValue(4303, +1.0)
            End If

            'включениеи клетей черновой промежуточной финишной
            IngotID = GetNearRightIngotX(5, 24)
            m_link.GetIngot2D(IngotID, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
            Dim StandOn As Boolean = IngotID <> 0 And IngotX < 268 And tt = 5
            m_link.SetSignalDoubleValue(4530, StandOn)
            m_link.SetSignalDoubleValue(4531, StandOn)
            m_link.SetSignalDoubleValue(4532, StandOn)
            Dim SS1, SS2, SS3, SS4, SS5, SS6, SS7, SS8, SS9, SS10, SS11, SS12, SS13, SS14, SS15, SS16, SS17, SS18, SS19, SS20, SS21 As Double
            If StandOn Then
                SS1 = 1.0
                SS2 = 1.05
                SS3 = 1.1
                SS4 = 1.15
                SS5 = 1.2
                SS6 = 1.25

                SS7 = 1.25
                SS8 = 1.5
                SS9 = 2.0
                SS10 = 2.5
                SS11 = 3.0
                SS12 = 4.0

                SS13 = 5.0
                SS14 = 7.0
                SS15 = 9.0
                SS16 = 11.0
                SS17 = 13.0
                SS18 = 15.0


                If GetIngotInPoint(5, 180) = FFBLowSpeedIID And FFBLowSpeedIID <> 0 Then 'FFB
                    SS19 = 3.0
                Else
                    SS19 = 15
                End If

                If GetIngotInPoint(5, 268) = FFBLowSpeedIID And FFBLowSpeedIID <> 0 Then 'трайб
                    If GetIngotInPoint(5, 180) = FFBLowSpeedIID Then
                        SS20 = 3.0
                    Else
                        SS20 = 5.0 ' регулировка расстояния между частями
                    End If
                Else
                    SS20 = 15
                End If

                SS21 = 1  'реформинг
            Else
                SS1 = 0
                SS2 = 0
                SS3 = 0
                SS4 = 0
                SS5 = 0
                SS6 = 0

                SS7 = 0
                SS8 = 0
                SS9 = 0
                SS10 = 0
                SS11 = 0
                SS12 = 0

                SS13 = 0
                SS14 = 0
                SS15 = 0
                SS16 = 0
                SS17 = 0
                SS18 = 0

                SS19 = 0
                SS20 = 0
                SS21 = 1
            End If
            m_link.SetSignalDoubleValue(4501, SS1)
            m_link.SetSignalDoubleValue(4502, SS2)
            m_link.SetSignalDoubleValue(4503, SS3)
            m_link.SetSignalDoubleValue(4504, SS4)
            m_link.SetSignalDoubleValue(4505, SS5)
            m_link.SetSignalDoubleValue(4506, SS6)
            m_link.SetSignalDoubleValue(4507, SS7)
            m_link.SetSignalDoubleValue(4508, SS8)
            m_link.SetSignalDoubleValue(4509, SS9)
            m_link.SetSignalDoubleValue(4510, SS10)
            m_link.SetSignalDoubleValue(4511, SS11)
            m_link.SetSignalDoubleValue(4512, SS12)
            m_link.SetSignalDoubleValue(4513, SS13)
            m_link.SetSignalDoubleValue(4514, SS14)
            m_link.SetSignalDoubleValue(4515, SS15)
            m_link.SetSignalDoubleValue(4516, SS16)
            m_link.SetSignalDoubleValue(4517, SS17)
            m_link.SetSignalDoubleValue(4518, SS18)
            m_link.SetSignalDoubleValue(4519, SS19)
            m_link.SetSignalDoubleValue(4520, SS20)
            m_link.SetSignalDoubleValue(4521, SS21)

        End If


    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        m_link.InitSSM(0, My.Settings.CONFIGPATH)

        Dim f As StreamWriter = New StreamWriter("abnsimlog.txt", True)
        f.WriteLine(Now.ToString() + " " + "запуск приложения")
        f.Close()

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Dim str As String = "CloseReason " + e.CloseReason.ToString() + ", Cancel " + e.Cancel.ToString()
        Dim f As StreamWriter = New StreamWriter("abnsimlog.txt", True)
        f.WriteLine(Now.ToString() + " " + "закрытие приложения " + str)
        f.Close()

    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeBar.Scroll
        Time.Text() = TimeBar.Value
        MainTimer.Interval = TimeBar.Value
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Form2.ShowDialog()
    End Sub


    Private Sub Createingot_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles createingot.Click

        Dim th As Integer
        th = cr_th.SelectedIndex
        m_link.AddIngot2D(Val(cr_l3id.Text), Val(cr_bid.Text), Val(cr_rid.Text), Val(cr_x1.Text), Val(cr_x2.Text), Val(cr_y1.Text), Val(cr_y2.Text), th, th)

    End Sub


    Private Sub Ch_iid_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ch_iid.TextChanged

        Dim fingotid As Integer

        If Val(ch_iid.Text) < 999 Or Val(ch_iid.Text) > 99999 Then
            Exit Sub
        End If

        fingotid = 0

        Dim Level3ID As Long, BaseID As Long, RollingID As Long, IngotID As Long, tt As Long, th As Long
        Dim IngotX As Double, IngotX2 As Double, IngotY As Double, IngotY2 As Double
        Dim i As Integer

        For i = 0 To m_link.IngotsCount - 1 Step 1
            m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
            If IngotID = ch_iid.Text Then
                fingotid = IngotID
                Exit For
            End If
        Next

        If fingotid <> 0 Then
            ch_bid.Text = BaseID
            ch_rid.Text = RollingID
            ch_l3id.Text = Level3ID
            ch_x1.Text = IngotX
            ch_x2.Text = IngotX2
            ch_y1.Text = IngotY
            ch_y2.Text = IngotY2
            ch_th.SelectedIndex = th
        End If

    End Sub

    Private Sub Ch_changefunc()

        Dim th As Integer
        th = ch_th.SelectedIndex
        m_link.IngotIDCorrection(Val(ch_iid.Text), Val(ch_l3id.Text), Val(ch_bid.Text), Val(ch_rid.Text))
        m_link.IngotCoordinateCorrection2D(Val(ch_iid.Text), Val(ch_x1.Text), Val(ch_x2.Text), Val(ch_y1.Text), Val(ch_y2.Text), th, th)

    End Sub

    Private Sub Ch_change_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ch_iid.Click, ch_change.Click

        Ch_changefunc()

    End Sub

    Private Sub Ch_right_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ch_right.Click

        ch_x1.Text = Val(ch_x1.Text) + Val(ch_size.Text)
        ch_x2.Text = Val(ch_x2.Text) + Val(ch_size.Text)
        Ch_changefunc()

    End Sub

    Private Sub Ch_left_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ch_left.Click

        ch_x1.Text = Val(ch_x1.Text) - Val(ch_size.Text)
        ch_x2.Text = Val(ch_x2.Text) - Val(ch_size.Text)
        Ch_changefunc()

    End Sub

    Private Sub Ch_up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage2.Click, ch_up.Click
        ch_y1.Text = Val(ch_y1.Text) + Val(ch_size.Text)
        ch_y2.Text = Val(ch_y2.Text) + Val(ch_size.Text)
        Ch_changefunc()

    End Sub

    Private Sub Ch_down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ch_down.Click
        ch_y1.Text = Val(ch_y1.Text) - Val(ch_size.Text)
        ch_y2.Text = Val(ch_y2.Text) - Val(ch_size.Text)
        Ch_changefunc()
    End Sub


    Private Sub Fur_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Fur.Paint
        Dim drawBrush As New SolidBrush(Color.Black)
        Dim i As Integer
        Dim x, y, w, h As Integer
        Dim th As Long
        Dim Level3ID As Long, BaseID As Long, RollingID As Long, IngotID As Long, tt As Long
        Dim IngotX As Double, IngotX2 As Double, IngotY As Double, IngotY2 As Double
        For i = 0 To m_link.IngotsCount - 1 Step 1
            m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
            If th = 3 Then
                x = (Fur.Width / 20.6) * (IngotY + 4)
                y = IngotX2 * (Fur.Height / 20.0)
                w = (Fur.Width / 20.6) * (IngotY2 - IngotY)
                h = (IngotX2 - IngotX) * (Fur.Height / 20.0)
                If (h = 0) Then
                    h = 0.13 * (Fur.Height / 20.0)
                End If
                e.Graphics.FillRectangle(Brushes.Red, x, y, w, h)
            End If
        Next i

    End Sub

    Private Function GetIngotInPoint(thread As Integer, x As Double) As Integer
        Dim Level3ID As Long, BaseID As Long, RollingID As Long, IngotID As Long, th As Long, tt As Long
        Dim IngotX As Double, IngotX2 As Double, IngotY As Double, IngotY2 As Double
        Dim i As Integer
        Dim result As Integer = 0
        For i = 0 To m_link.IngotsCount - 1 Step 1
            m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
            If th = thread Then
                If (IngotX <= x And IngotX2 >= x And IngotX <= IngotX2) Or (IngotX >= x And IngotX2 <= x And IngotX >= IngotX2) Then
                    result = IngotID
                    Exit For
                End If
            End If
        Next i
        Return result
    End Function

    Private Function GetNearRightIngotX(thread As Integer, x As Double) As Integer
        Dim Level3ID As Long, BaseID As Long, RollingID As Long, IngotID As Long, th As Long, tt As Long
        Dim IngotX As Double, IngotX2 As Double, IngotY As Double, IngotY2 As Double
        Dim i As Integer
        Dim result As Integer = 0
        Dim minx As Double = 1.0E+128
        For i = 0 To m_link.IngotsCount - 1 Step 1
            m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
            If th = thread Then
                If IngotX >= x And IngotX < minx Then
                    minx = IngotX
                    result = IngotID
                End If
            End If
        Next i
        Return result
    End Function

    Private Sub BtnFullFur_Click(sender As Object, e As EventArgs) Handles BtnFullFur.Click

        Dim x As Double = 0
        While (x <= 19.8)
            m_link.AddIngot2D(0, 0, 0, x, x + 0.13, 0, 12, 3, 3)
            x = x + 0.2
        End While

    End Sub

    Private Sub BtnClearMill_Click(sender As Object, e As EventArgs) Handles BtnClearMill.Click

        Dim Level3ID As Long, BaseID As Long, RollingID As Long, IngotID As Long, th As Long, tt As Long
        Dim IngotX As Double, IngotX2 As Double, IngotY As Double, IngotY2 As Double
        Dim i As Integer
        For i = 0 To m_link.IngotsCount - 1 Step 1
            m_link.GetIngot2D(i, Level3ID, BaseID, RollingID, IngotID, IngotX, IngotX2, IngotY, IngotY2, th, tt)
            If th = 5 Then
                m_link.DeleteIngot(IngotID)
            End If
        Next i

    End Sub

    Private Function SetSimSignal(id As Double, thread As Integer, x As Double, min As Double, max As Double, valwithobject As Double, valwithoutobject As Double) As Double

        Dim simval As Double

        If thread < 0 Then
            simval = valwithobject + (max - min) * (rand.NextDouble() - 0.5) * 0.1 'сигнал без привязки к штуке
        Else
            If GetIngotInPoint(thread, x) <> 0 Then
                simval = valwithobject + (max - min) * (rand.NextDouble() - 0.5) 'сигнал при наличии штуки
            Else
                simval = valwithoutobject 'сигнал при отсутствии штуки
            End If
        End If


        m_link.SetSignalDoubleValue(id, simval)

    End Function

    Private Function SetSensSignal(id As Double, thread As Integer, x As Double, reverse As Boolean) As Boolean

        Dim simval As Boolean = GetIngotInPoint(thread, x) <> 0

        If reverse Then
            simval = Not simval
        End If

        m_link.SetSignalDoubleValue(id, simval)

        Return simval

    End Function

    Private Sub BtnSmena_Click(sender As Object, e As EventArgs) Handles BtnSmena.Click
        m_link.SetSignalDoubleValue(25308, (m_link.get_GetSignalValue(25308) + 1) Mod 1000)
    End Sub

    Private Sub BtnCutRought_Click(sender As Object, e As EventArgs) Handles BtnCutRought.Click
        m_link.SetSignalDoubleValue(4130, (m_link.get_GetSignalValue(4130) + 1) Mod 1000)
    End Sub

    Private Sub BtnCutMiddle_Click(sender As Object, e As EventArgs) Handles BtnCutMiddle.Click
        m_link.SetSignalDoubleValue(4131, (m_link.get_GetSignalValue(4131) + 1) Mod 1000)
    End Sub

    Private Sub BtnCutFFB_Click(sender As Object, e As EventArgs) Handles BtnCutFFB.Click
        m_link.SetSignalDoubleValue(4132, (m_link.get_GetSignalValue(4132) + 1) Mod 1000)
    End Sub

    Private Sub BtnParamSaveNum_Click(sender As Object, e As EventArgs) Handles btnParamSaveNum.Click
        m_link.SetIngotParam(Val(tbParamIID.Text), Val(tbParamNum.Text), Val(tbParamVal.Text))
    End Sub

    Private Sub BtnParamSaveStr_Click(sender As Object, e As EventArgs) Handles btnParamSaveStr.Click
        m_link.SetIngotParamStr(Val(tbParamIID.Text), Val(tbParamNum.Text), tbParamVal.Text)
    End Sub

    Private Sub BtnParamRead_Click(sender As Object, e As EventArgs) Handles btnParamRead.Click
        tbParamVal.Text = m_link.GetIngotParamWaitStr(Val(tbParamIID.Text), Val(tbParamNum.Text), 2000)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>

    Async Sub CheckDBTask_Tick(sender As Object, e As EventArgs) Handles CheckDBTask.Tick

        If Not cbTestDB.Checked Then
            Return
        End If
        cbTestDB.Checked = False
        cbTestDB.Enabled = False

        Dim result As String

        Try

            Dim strconn As String = My.Settings.BDCONNSTR

            Using cnn As New NpgsqlConnection(strconn)

                Await cnn.OpenAsync()
                Dim stsql As String = "Select * from f_schedules('" + DateTime.Now + "')"
                Dim comm As New NpgsqlCommand(stsql, cnn)

                Dim reader As NpgsqlDataReader = Await comm.ExecuteReaderAsync()

                If Await reader.ReadAsync() Then
                    result = reader.GetInt32(0).ToString()
                Else
                    result = "0"
                End If
                Await reader.CloseAsync()
                Await cnn.CloseAsync()

            End Using

        Catch ex As Exception
            result = ex.ToString()

        Finally
            cbTestDB.Checked = True
            cbTestDB.Enabled = True
        End Try

        If result.Length() <= 4 Then
            lblBDTaskNum.ForeColor = Color.Green
            lblBDTaskNum.Text = result
        Else
            lblBDTaskNum.ForeColor = Color.Red
            lblBDTaskNum.Text = "error"
        End If

        ToolTipBDConnect.SetToolTip(lblBDTaskNum, result)

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' 
    Dim DBClearTask As Threading.Tasks.Task
    Public dbclearresult As String = "OK"
    Dim newdbclearcheckdata As Boolean = False

    Private Sub BtnCloseBDIntervals_Click(sender As Object, e As EventArgs) Handles BtnCloseBDIntervals.Click
        DBClearTask = New Threading.Tasks.Task(Sub() ClearDBTaskSub()) 'удаление необязательно
        DBClearTask.Start()

    End Sub

    Public Sub ClearDBTaskSub()

        ' вызов функции 
        Try

            Dim strconn As String = My.Settings.BDCONNSTR
            dbclearresult = "OK"

            Using cnn As New NpgsqlConnection(strconn)
                cnn.Open()
                Dim comm As New NpgsqlCommand("CALL p_locations_losses()", cnn)
                Dim reader As NpgsqlDataReader = comm.ExecuteReader()
                cnn.Close()
            End Using

        Catch ex As Exception
            dbclearresult = ex.ToString()
        End Try

        newdbclearcheckdata = True

    End Sub

    Public Sub CheckDBClearTaskSubShowResult()

        If newdbclearcheckdata Then
            ToolTipDBClear.SetToolTip(BtnCloseBDIntervals, dbclearresult)
            newdbclearcheckdata = False
        End If

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    Dim DBCalcPaspTask As Threading.Tasks.Task
    Public dbcalcpaspresult As String = "OK"
    Dim newdbcalcpaspdata As Boolean = False

    Private Sub DBCalcPaspTimer_Tick(sender As Object, e As EventArgs) Handles DBCalcPaspTimer.Tick

        If Not CbCalcPaspDB.Checked Then
            Return
        End If

        If Not IsNothing(DBCalcPaspTask) Then
            If DBCalcPaspTask.Status = Threading.Tasks.TaskStatus.Running Then
                Return
            End If
        End If

        DBCalcPaspTask = New Threading.Tasks.Task(Sub() CalcPaspDBTaskSub()) 'удаление необязательно
        DBCalcPaspTask.Start()

    End Sub

    Public Sub CalcPaspDBTaskSub()

        Try

            Dim strconn As String = My.Settings.BDCONNSTR
            dbcalcpaspresult = "OK"

            Using cnn As New NpgsqlConnection(strconn)
                cnn.Open()
                Dim comm As New NpgsqlCommand("CALL public.p_autocalc_passport()", cnn)
                Dim reader As NpgsqlDataReader = comm.ExecuteReader()
                cnn.Close()
            End Using


        Catch ex As Exception
            dbcalcpaspresult = ex.ToString()
        End Try

        newdbcalcpaspdata = True

    End Sub


    Public Sub CalcPaspDBTaskSubShowResult()

        If newdbcalcpaspdata Then

            ToolTipCalcPasp.SetToolTip(CbCalcPaspDB, dbcalcpaspresult)
            newdbcalcpaspdata = False

        End If

    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnCrushIngotsPlus_Click(sender As Object, e As EventArgs) Handles BtnCrushIngotsPlus.Click
        CrushIngotsCount += 1
    End Sub

End Class