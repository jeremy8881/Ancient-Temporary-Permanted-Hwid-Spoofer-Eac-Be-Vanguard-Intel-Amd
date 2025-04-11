Imports Microsoft.Win32
Imports System.Diagnostics
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text
Public Class Form1
    Inherits Windows.Forms.Form
    Private timer As Windows.Forms.Timer
    Private animationProgress As Single = 0.0F
    Private steps As Integer = 170
    Private startColor As Drawing.Color = Drawing.Color.FromArgb(23, 23, 23)
    Private middleColor As Drawing.Color = Drawing.Color.FromArgb(248, 248, 248)
    Private endColor As Drawing.Color = Drawing.Color.FromArgb(23, 23, 23)
    Private currentColor As Drawing.Color
    Private isDragging As Boolean
    Private offset As Drawing.Point
    Protected Overrides Sub OnPaint(ByVal e As Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        SetRoundedCorners()

        Dim gradientRect As Drawing.Rectangle = New Drawing.Rectangle(0, 0, Width, Height)
        Using brush As LinearGradientBrush = New LinearGradientBrush(gradientRect, startColor, currentColor, LinearGradientMode.Vertical)
            e.Graphics.FillRectangle(brush, gradientRect)
        End Using
    End Sub
    Public Sub New()
        InitializeComponent()
        SetRoundedCorners()

        timer = New Windows.Forms.Timer()
        timer.Interval = 100
        AddHandler timer.Tick, AddressOf timer_Tick

        DoubleBuffered = True
        timer.Start()
    End Sub
    Private Sub timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentR, currentG, currentB As Integer
        If animationProgress < 0.5F Then
            Dim subProgress = animationProgress * 2
            currentR = CInt(startColor.R + (middleColor.R - startColor.R) * subProgress)
            currentG = CInt(startColor.G + (middleColor.G - startColor.G) * subProgress)
            currentB = CInt(startColor.B + (middleColor.B - startColor.B) * subProgress)
        Else
            Dim subProgress = (animationProgress - 0.5F) * 2
            ''currentR = CInt(middleColor.R + (endColor.R - middleColor.R) * subProgress)
            ''currentG = CInt(middleColor.G + (endColor.G - middleColor.G) * subProgress)
            'currentB = CInt(middleColor.B + (endColor.B - middleColor.B) * subProgress)
        End If
        currentColor = Drawing.Color.FromArgb(currentR, currentG, currentB)

        animationProgress += 1.0F / steps
        If animationProgress >= 1.0F Then
            animationProgress = 0.0F
        End If

        Invalidate()
    End Sub
    'sechex.me
    'sechex.me
    'sechex.me
    'sechex.me
    Private Sub SetRoundedCorners()
        Dim radius = 18
        Dim path As GraphicsPath = New GraphicsPath()
        path.AddArc(0, 0, radius, radius, 180, 90)
        path.AddArc(Width - radius, 0, radius, radius, 270, 90)
        path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90)
        path.AddArc(0, Height - radius, radius, radius, 90, 90)
        path.CloseFigure()
        SetStyle(Windows.Forms.ControlStyles.ResizeRedraw, True)
        Region = New Drawing.Region(path)
        SetGraphicsQuality()
    End Sub
    'sechex.me
    'sechex.me
    'sechex.me
    'sechex.me
    Private Sub SetGraphicsQuality()
        SetStyle(Windows.Forms.ControlStyles.AllPaintingInWmPaint Or Windows.Forms.ControlStyles.UserPaint Or Windows.Forms.ControlStyles.OptimizedDoubleBuffer, True)
        Using g As Drawing.Graphics = CreateGraphics()
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
        End Using
    End Sub
    'sechex.me
    'sechex.me
    'sechex.me
    'sechex.me
    Protected Overrides Sub OnMouseDown(ByVal e As Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isDragging = True
            offset = New Drawing.Point(e.X, e.Y)
        End If
    End Sub
    'sechex.me
    'sechex.me
    'sechex.me
    'sechex.me
    Protected Overrides Sub OnMouseMove(ByVal e As Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If isDragging Then
            Dim newLocation As Drawing.Point = PointToScreen(New Drawing.Point(e.X, e.Y))
            newLocation.Offset(-offset.X, -offset.Y)
            Location = newLocation
        End If
    End Sub
    'sechex.me
    'sechex.me
    'sechex.me
    'sechex.me
    Protected Overrides Sub OnMouseUp(ByVal e As Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isDragging = False
        End If
    End Sub

    Private Sub SaveLogs(ByVal id As String, ByVal logBefore As String, ByVal logAfter As String)
        Dim logsFolderPath = Path.Combine(Windows.Forms.Application.StartupPath, "Logs")
        If Not Directory.Exists(logsFolderPath) Then Directory.CreateDirectory(logsFolderPath)

        Dim logFileName = Path.Combine(logsFolderPath, $"{Date.Now:yyyy-MM-dd_HH-mm-ss}.txt")
        Dim logEntryBefore = $"{Date.Now:HH:mm:ss}: ID {id} -  {logBefore} (Before)"
        Dim logEntryAfter = $"{Date.Now:HH:mm:ss}: ID {id} -  {logAfter} (After)"

        File.AppendAllText(logFileName, logEntryBefore & Environment.NewLine)
        File.AppendAllText(logFileName, logEntryAfter & Environment.NewLine)
    End Sub
    Public Shared Sub Enable_LocalAreaConection(ByVal adapterId As String, ByVal Optional enable As Boolean = True)
        Dim interfaceName = "Ethernet"
        For Each i In NetworkInterface.GetAllNetworkInterfaces()
            If Equals(i.Id, adapterId) Then
                interfaceName = i.Name
                Exit For
            End If
        Next

        Dim control As String
        If enable Then
            control = "enable"
        Else
            control = "disable"
        End If

        Dim psi As ProcessStartInfo = New ProcessStartInfo("netsh", $"interface set interface ""{interfaceName}"" {control}")
        Dim p As Process = New Process()
        p.StartInfo = psi
        p.Start()
        p.WaitForExit()
    End Sub
    'sechex.me
    'sechex.me
    'sechex.me
    'sechex.me

    Public Shared Function RandomId(ByVal length As Integer) As String
        Dim chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim result = ""
        Dim random As Random = New Random()

        For i = 0 To length - 1
            result += chars(random.Next(chars.Length))
        Next

        Return result
    End Function


    Private Function RandomIdprid(ByVal length As Integer) As String
        Const digits = "0123456789"
        Const letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim random = New Random()
        Dim id = New Char(length - 1) {}
        Dim dashIndex = 5
        Dim letterIndex = 17
        For i = 0 To length - 1
            If i = dashIndex Then
                id(i) = "-"c
                dashIndex += 6
            ElseIf i = letterIndex Then
                id(i) = letters(random.Next(letters.Length))
            ElseIf i = letterIndex + 1 Then
                id(i) = letters(random.Next(letters.Length))
            Else
                id(i) = digits(random.Next(digits.Length))
            End If
        Next
        Return New String(id)
    End Function


    'sechex.me
    'sechex.me
    'sechex.me
    'sechex.me

    Public Shared Function RandomMac() As String
        Dim chars = "ABCDEF0123456789"
        Dim windows = "26AE"
        Dim result = ""
        Dim random As Random = New Random()

        result += chars(random.Next(chars.Length))
        result += windows(random.Next(windows.Length))

        For i = 0 To 4
            result += "-"
            result += chars(random.Next(chars.Length))
            result += chars(random.Next(chars.Length))

        Next

        Return result
    End Function

    Private Sub Guna2GradientButton13SpoofAll_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton13SpoofAll.Click
        Dim registryEntriesExist = False

        Try
            Guna2GradientButton12Requirement_Click(sender, e)
            registryEntriesExist = True
        Catch ex As Exception
            ShowNotification("Error executing functions: " & ex.Message, NotificationType.Error)
        End Try

        If registryEntriesExist Then
            Guna2GradientButton1DISK_Click(sender, e)
            Guna2GradientButton2MAC_Click(sender, e)
            Guna2GradientButton6GUID_Click(sender, e)
            Guna2GradientButton5WindowsID_Click(sender, e)
            Guna2GradientButton9PCName_Click(sender, e)
            Guna2GradientButton8DisplayID_Click(sender, e)
            Guna2GradientButton10EFI_Click(sender, e)
            Guna2GradientButton3SMBIOS_Click(sender, e)
            Guna2GradientButton4ProductID_Click(sender, e)

            ShowNotification("All functions executed successfully.", NotificationType.Success)
        Else
            ShowNotification("Error: One or more required registry entries are missing.", NotificationType.Error)
        End If
    End Sub

    Private Sub Guna2GradientButton1DISK_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1DISK.Click
        Try
            Using ScsiPorts = Registry.LocalMachine.OpenSubKey("HARDWARE\DEVICEMAP\Scsi")
                If ScsiPorts IsNot Nothing Then
                    For Each port In ScsiPorts.GetSubKeyNames()
                        Using ScsiBuses = Registry.LocalMachine.OpenSubKey($"HARDWARE\DEVICEMAP\Scsi\{port}")
                            If ScsiBuses IsNot Nothing Then
                                For Each bus In ScsiBuses.GetSubKeyNames()
                                    Using ScsuiBus = Registry.LocalMachine.OpenSubKey($"HARDWARE\DEVICEMAP\Scsi\{port}\{bus}\Target Id 0\Logical Unit Id 0", True)
                                        If ScsuiBus IsNot Nothing Then
                                            Dim deviceTypeValue = ScsuiBus.GetValue("DeviceType")
                                            If deviceTypeValue IsNot Nothing AndAlso Equals(deviceTypeValue.ToString(), "DiskPeripheral") Then
                                                Dim identifierBefore As String = ScsuiBus.GetValue("Identifier").ToString()
                                                Dim serialNumberBefore As String = ScsuiBus.GetValue("SerialNumber").ToString()

                                                Dim identifierAfter = RandomId(14)
                                                Dim serialNumberAfter = RandomId(14)
                                                Dim logBefore = $"DiskPeripheral {bus}\Target Id 0\Logical Unit Id 0 - Identifier: {identifierBefore}, SerialNumber: {serialNumberBefore}"
                                                Dim logAfter = $"DiskPeripheral {bus}\Target Id 0\Logical Unit Id 0 - Identifier: {identifierAfter}, SerialNumber: {serialNumberAfter}"
                                                SaveLogs("disk", logBefore, logAfter)

                                                ScsuiBus.SetValue("DeviceIdentifierPage", Encoding.UTF8.GetBytes(serialNumberAfter))
                                                ScsuiBus.SetValue("Identifier", identifierAfter)
                                                ScsuiBus.SetValue("InquiryData", Encoding.UTF8.GetBytes(identifierAfter))
                                                ScsuiBus.SetValue("SerialNumber", serialNumberAfter)
                                            End If
                                        End If
                                    End Using
                                Next
                            Else
                                ShowNotification("ScsiBuses key not found.", NotificationType.Error)
                                Return
                            End If
                        End Using
                    Next
                Else
                    ShowNotification("ScsiPorts key not found.", NotificationType.Error)
                    Return
                End If
            End Using
            Using diskKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Enum\IDE")
                If diskKey IsNot Nothing Then
                    For Each controllerId In diskKey.GetSubKeyNames()
                        Using controller = diskKey.OpenSubKey(controllerId)
                            If controller IsNot Nothing Then
                                For Each diskId In controller.GetSubKeyNames()
                                    Using disk = controller.OpenSubKey(diskId, True)
                                        If disk IsNot Nothing Then
                                            Dim serialNumberBefore As String = disk.GetValue("SerialNumber")?.ToString()

                                            Dim serialNumberAfter = RandomId(14)
                                            Dim logBefore = $"Hard Disk {diskId} - SerialNumber: {serialNumberBefore}"
                                            Dim logAfter = $"Hard Disk {diskId} - SerialNumber: {serialNumberAfter}"
                                            SaveLogs("disk", logBefore, logAfter)

                                            disk.SetValue("SerialNumber", serialNumberAfter)
                                        End If
                                    End Using
                                Next
                            End If
                        End Using
                    Next
                End If
            End Using

            ShowNotification("Disk Function executed successfully.", NotificationType.Success)
        Catch ex As Exception
            ShowNotification("An error occurred while executing the Disk Function: " & ex.Message, NotificationType.Error)
        End Try
    End Sub

    Private Sub Guna2GradientButton2MAC_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2MAC.Click
        Try
            Dim spoofSuccess As Boolean = SpoofMAC()

            If Not spoofSuccess Then
                ShowNotification("MAC address successfully spoofed.", NotificationType.Success)
            End If
        Catch ex As Exception
            ShowNotification("An error occurred while spoofing the MAC address: " & ex.Message, NotificationType.Error)
        End Try
    End Sub
    Private Function SpoofMAC() As Boolean
        Dim err = False

        Using NetworkAdapters = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}")
            For Each adapter In NetworkAdapters.GetSubKeyNames()
                If Not Equals(adapter, "Properties") Then
                    Try
                        Using NetworkAdapter = Registry.LocalMachine.OpenSubKey($"SYSTEM\CurrentControlSet\Control\Class\{{4d36e972-e325-11ce-bfc1-08002be10318}}\{adapter}", True)
                            If NetworkAdapter.GetValue("BusType") IsNot Nothing Then
                                Dim adapterId As String = NetworkAdapter.GetValue("NetCfgInstanceId").ToString()
                                Dim macBefore As String = NetworkAdapter.GetValue("NetworkAddress")?.ToString()
                                Dim macAfter As String = RandomMac()
                                Dim logBefore = $"MAC Address {adapterId} - Before: {macBefore}"
                                Dim logAfter = $"MAC Address {adapterId} - After: {macAfter}"
                                SaveLogs("mac", logBefore, logAfter)

                                NetworkAdapter.SetValue("NetworkAddress", macAfter)
                                Enable_LocalAreaConection(adapterId, False)
                                Enable_LocalAreaConection(adapterId, True)
                            End If
                        End Using
                    Catch __unusedSecurityException1__ As SecurityException
                        err = True
                        Exit For
                    End Try
                End If
            Next
        End Using

        Return err
    End Function

    Private Sub Guna2GradientButton6GUID_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton6GUID.Click
        Try
            Using HardwareGUID = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001", True)
                If HardwareGUID IsNot Nothing Then
                    HardwareGUID.SetValue("HwProfileGuid", $"{{{System.Guid.NewGuid()}}}")
                    Dim logBefore As String = "HwProfileGuid - Before: " & HardwareGUID.GetValue("HwProfileGuid").ToString()
                    Dim logAfter As String = "HwProfileGuid - After: " & HardwareGUID.GetValue("HwProfileGuid").ToString()
                    SaveLogs("guid", logBefore, logAfter)
                Else
                    ShowNotification("HardwareGUID key not found.", NotificationType.Error)
                    Return
                End If
            End Using

            Using MachineGUID = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Cryptography", True)
                If MachineGUID IsNot Nothing Then
                    MachineGUID.SetValue("MachineGuid", System.Guid.NewGuid().ToString())
                    Dim logBefore As String = "MachineGuid - Before: " & MachineGUID.GetValue("MachineGuid").ToString()
                    Dim logAfter As String = "MachineGuid - After: " & MachineGUID.GetValue("MachineGuid").ToString()
                    SaveLogs("guid", logBefore, logAfter)
                Else
                    ShowNotification("MachineGUID key not found.", NotificationType.Error)
                    Return
                End If
            End Using

            Using MachineId = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\SQMClient", True)
                If MachineId IsNot Nothing Then
                    MachineId.SetValue("MachineId", $"{{{System.Guid.NewGuid()}}}")
                    Dim logBefore As String = "MachineId - Before: " & MachineId.GetValue("MachineId").ToString()
                    Dim logAfter As String = "MachineId - After: " & MachineId.GetValue("MachineId").ToString()
                    SaveLogs("guid", logBefore, logAfter)
                Else
                    ShowNotification("MachineId key not found.", NotificationType.Error)
                    Return
                End If
            End Using

            Using SystemInfo = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\SystemInformation", True)
                If SystemInfo IsNot Nothing Then
                    Dim rnd As Random = New Random()
                    Dim day = rnd.Next(1, 31)
                    Dim dayStr As String = If(day < 10, $"0{day}", day.ToString())

                    Dim month = rnd.Next(1, 13)
                    Dim monthStr As String = If(month < 10, $"0{month}", month.ToString())

                    Dim year = rnd.Next(1990, 2023)
                    Dim yearStr As String = year.ToString()

                    Dim randomDate = $"{monthStr}/{dayStr}/{yearStr}"

                    SystemInfo.SetValue("BIOSReleaseDate", randomDate)
                    Dim logBefore As String = "BIOSReleaseDate - Before: " & SystemInfo.GetValue("BIOSReleaseDate").ToString()
                    Dim logAfter As String = "BIOSReleaseDate - After: " & SystemInfo.GetValue("BIOSReleaseDate").ToString()
                    SaveLogs("guid", logBefore, logAfter)
                Else
                    ShowNotification("SystemInformation key not found.", NotificationType.Error)
                    Return
                End If
            End Using

            ShowNotification("GUIDs successfully generated.", NotificationType.Success)
        Catch ex As Exception
            ShowNotification("An error occurred: " & ex.Message, NotificationType.Error)
        End Try
    End Sub
    Private Function SpoofWinID() As Boolean
        Dim err = False

        Try
            Using winIDKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Cryptography", True)
                If winIDKey IsNot Nothing Then
                    Dim winIDBefore As String = winIDKey.GetValue("MachineGuid").ToString()
                    Dim spoofedWinIDBytes = New Byte(15) {}
                    Using rng = New RNGCryptoServiceProvider()
                        rng.GetBytes(spoofedWinIDBytes)
                    End Using
                    Dim spoofedWinID As String = BitConverter.ToString(spoofedWinIDBytes).Replace("-", "").ToLowerInvariant()
                    winIDKey.SetValue("MachineGuid", spoofedWinID)

                    Dim logBefore = "MachineGuid - Before: " & winIDBefore
                    Dim logAfter As String = "MachineGuid - After: " & winIDKey.GetValue("MachineGuid").ToString()
                    SaveLogs("guid", logBefore, logAfter)

                    ShowNotification("Windows ID spoofed successfully.", NotificationType.Success)
                Else
                    err = True
                    ShowNotification("Windows ID spoofing failed: Registry key not found.", NotificationType.Error)
                End If
            End Using
        Catch ex As Exception
            err = True
            ShowNotification("An error occurred while spoofing the Windows ID: " & ex.Message, NotificationType.Error)
        End Try

        Return err
    End Function

    Private Sub Guna2GradientButton5WindowsID_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton5WindowsID.Click
        If SpoofWinID() Then
            ShowNotification("An error occurred while spoofing the Windows ID.", NotificationType.Error)
        End If
    End Sub

    Private Sub Guna2GradientButton9PCName_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton9PCName.Click
        Try
            Dim originalName As String
            Dim newName = RandomId(8)
            Using computerNameKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\ComputerName\ComputerName", True)
                If computerNameKey IsNot Nothing Then
                    originalName = computerNameKey.GetValue("ComputerName").ToString()

                    computerNameKey.SetValue("ComputerName", newName)
                    computerNameKey.SetValue("ActiveComputerName", newName)
                    computerNameKey.SetValue("ComputerNamePhysicalDnsDomain", "")
                Else
                    ShowNotification("ComputerName key not found.", NotificationType.Error)
                    Return
                End If
            End Using
            Using activeComputerNameKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\ComputerName\ActiveComputerName", True)
                If activeComputerNameKey IsNot Nothing Then
                    activeComputerNameKey.SetValue("ComputerName", newName)
                    activeComputerNameKey.SetValue("ActiveComputerName", newName)
                    activeComputerNameKey.SetValue("ComputerNamePhysicalDnsDomain", "")
                Else
                    ShowNotification("ActiveComputerName key not found.", NotificationType.Error)
                    Return
                End If
            End Using

            Using hostnameKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", True)
                If hostnameKey IsNot Nothing Then
                    hostnameKey.SetValue("Hostname", newName)
                    hostnameKey.SetValue("NV Hostname", newName)
                Else
                    ShowNotification("Hostname key not found.", NotificationType.Error)
                    Return
                End If
            End Using
            Using interfacesKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces", True)
                If interfacesKey IsNot Nothing Then
                    For Each interfaceName In interfacesKey.GetSubKeyNames()
                        Using interfaceKey = interfacesKey.OpenSubKey(interfaceName, True)
                            If interfaceKey IsNot Nothing Then
                                interfaceKey.SetValue("Hostname", newName)
                                interfaceKey.SetValue("NV Hostname", newName)
                            End If
                        End Using
                    Next
                End If
            End Using
            Dim logBefore = "ComputerName - Before: " & originalName
            Dim logAfter = "ComputerName - After: " & newName
            SaveLogs("pcname", logBefore, logAfter)

            ShowNotification("PC name spoofed successfully.", NotificationType.Success)
        Catch ex As Exception
            ShowNotification("An error occurred while spoofing the PC name: " & ex.Message, NotificationType.Error)
        End Try
    End Sub

    Private Sub Guna2GradientButton8DisplayID_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton8DisplayID.Click
        Try
            Dim displaySettings = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU", True)

            If displaySettings IsNot Nothing Then
                Dim rnd As Random = New Random()
                Dim displayId = rnd.Next(1, 100)
                displaySettings.SetValue("MRU0", $"Display{displayId}")
                Dim spoofedDisplayId = $"SpoofedDisplay{displayId}"
                displaySettings.SetValue("MRU1", spoofedDisplayId)
                displaySettings.SetValue("MRU2", spoofedDisplayId)
                displaySettings.SetValue("MRU3", spoofedDisplayId)
                displaySettings.SetValue("MRU4", spoofedDisplayId)
                Dim logBefore As String = "Display ID - Before: " & displayId.ToString()
                Dim logAfter = "Display ID - After: " & spoofedDisplayId
                SaveLogs("display", logBefore, logAfter)

                ShowNotification("Display Function executed successfully.", NotificationType.Success)
            Else
                ShowNotification("Display settings registry key not found.", NotificationType.Error)
            End If
        Catch ex As Exception
            ShowNotification("An error occurred while changing the display ID: " & ex.Message, NotificationType.Error)
        End Try
    End Sub

    Private Sub Guna2GradientButton10EFI_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton10EFI.Click
        Try
            Using efiVariables = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\Nsi\{eb004a03-9b1a-11d4-9123-0050047759bc}\26", True)
                If efiVariables IsNot Nothing Then
                    Dim efiVariableIdBefore As String = efiVariables.GetValue("VariableId")?.ToString()

                    Dim newEfiVariableId As String = System.Guid.NewGuid().ToString()
                    efiVariables.SetValue("VariableId", newEfiVariableId)
                    Dim logBefore = "EFI Variable ID - Before: " & efiVariableIdBefore
                    Dim logAfter = "EFI Variable ID - After: " & newEfiVariableId
                    SaveLogs("efi", logBefore, logAfter)

                    ShowNotification("EFI Function executed successfully.", NotificationType.Success)
                Else
                    ShowNotification("EFI variables registry key not found.", NotificationType.Error)
                End If
            End Using
        Catch ex As Exception
            ShowNotification("An error occurred while executing the EFI Function: " & ex.Message, NotificationType.Error)
        End Try
    End Sub

    Private Sub Guna2GradientButton4ProductID_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton4ProductID.Click
        Try
            Using productKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", True)
                If productKey IsNot Nothing Then
                    Dim originalProductId As String = productKey.GetValue("ProductId")?.ToString()

                    Dim newProductId = RandomIdprid(20)
                    productKey.SetValue("ProductId", newProductId)

                    Dim logBefore = "Product ID - Before: " & originalProductId
                    Dim logAfter = "Product ID - After: " & newProductId
                    SaveLogs("product", logBefore, logAfter)

                    ShowNotification("Product Function executed successfully.", NotificationType.Success)
                Else
                    ShowNotification("Product registry key not found.", NotificationType.Error)
                End If
            End Using
        Catch ex As Exception
            ShowNotification("An error occurred while changing the Product ID: " & ex.Message, NotificationType.Error)
        End Try
    End Sub

    Private Sub Guna2GradientButton11Backup_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton11Backup.Click
        Try
            Dim programDirectory = AppDomain.CurrentDomain.BaseDirectory
            Dim backupFolder = Path.Combine(programDirectory, "Backup")
            Dim backupPath = Path.Combine(backupFolder, "backup.reg")
            Directory.CreateDirectory(backupFolder)
            Call Process.Start("reg", $"export HKEY_LOCAL_MACHINE\SYSTEM ""{backupPath}"" /y").WaitForExit()
            Call Process.Start("reg", $"export HKEY_LOCAL_MACHINE\HARDWARE ""{backupPath}"" /y").WaitForExit()
            Call Process.Start("reg", $"export HKEY_LOCAL_MACHINE\SOFTWARE ""{backupPath}"" /y").WaitForExit()

            Windows.Forms.MessageBox.Show("Registry backup created successfully.", "Backup Successful", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
        Catch ex As Exception
            Windows.Forms.MessageBox.Show("An error occurred while creating the registry backup: " & ex.Message, "Error", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Guna2GradientButton12Requirement_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton12Requirement.Click
        Dim registryEntries = New String() {"HARDWARE\DEVICEMAP\Scsi", "HARDWARE\DESCRIPTION\System\MultifunctionAdapter\0\DiskController\0\DiskPeripheral", "SYSTEM\CurrentControlSet\Control\ComputerName\ComputerName", "SYSTEM\CurrentControlSet\Control\ComputerName\ActiveComputerName", "SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces", "SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001", "SOFTWARE\Microsoft\Cryptography", "SOFTWARE\Microsoft\SQMClient", "SYSTEM\CurrentControlSet\Control\SystemInformation", "SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate", "SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}", "SYSTEM\CurrentControlSet\Control\Nsi\{eb004a03-9b1a-11d4-9123-0050047759bc}\26", "HARDWARE\DESCRIPTION\System\BIOS"}

        Dim missingEntries As List(Of String) = New List(Of String)()

        For Each entry In registryEntries
            Using key = Registry.LocalMachine.OpenSubKey(entry)
                If key Is Nothing Then
                    missingEntries.Add(entry)
                End If
            End Using
        Next

        If missingEntries.Count > 0 Then
            Dim errorMessage = Encoding.UTF8.GetString(Convert.FromBase64String("UmVnaXN0cnkgZW50cmllcyBub3QgZm91bmQ6"))
            For Each entry In missingEntries
                errorMessage += Encoding.UTF8.GetString(Convert.FromBase64String("Cg==")) & entry
            Next
            ShowNotification(errorMessage, NotificationType.Error)
        Else
            ShowNotification(Encoding.UTF8.GetString(Convert.FromBase64String("QWxsIHJlZ2lzdHJ5IGVudHJpZXMgZXhpc3Qu")), NotificationType.Success)
        End If
    End Sub
    Private Sub Enable_LocalAreaConnection(ByVal adapterId As String, ByVal enable As Boolean)
        Using process = New Process()
            process.StartInfo.FileName = "netsh"
            process.StartInfo.Arguments = $"interface set interface ""{adapterId}"" {If(enable, "enable", "disable")}"
            process.StartInfo.UseShellExecute = False
            process.StartInfo.CreateNoWindow = True
            process.Start()
            process.WaitForExit()
        End Using
    End Sub
    Private Sub ShowNotification(ByVal message As String, ByVal type As NotificationType)
        Windows.Forms.MessageBox.Show(message, "Spoofy [Open Source]", Windows.Forms.MessageBoxButtons.OK, GetNotificationIcon(type))
    End Sub
    Private Function GetNotificationIcon(ByVal type As NotificationType) As Windows.Forms.MessageBoxIcon
        Select Case type
            Case NotificationType.Success
                Return Windows.Forms.MessageBoxIcon.Information
            Case NotificationType.Error
                Return Windows.Forms.MessageBoxIcon.Error
            Case NotificationType.Warning
                Return Windows.Forms.MessageBoxIcon.Warning
            Case Else
                Return Windows.Forms.MessageBoxIcon.None
        End Select
    End Function
    Private Enum NotificationType
        Success
        [Error]
        Warning
    End Enum

    Private Sub Guna2GradientButton3SMBIOS_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3SMBIOS.Click
        Try
            Using smbiosData = Registry.LocalMachine.OpenSubKey("HARDWARE\DESCRIPTION\System\BIOS", True)
                If smbiosData IsNot Nothing Then
                    Dim serialNumberBefore As String = smbiosData.GetValue("SystemSerialNumber")?.ToString()

                    Dim newSerialNumber = RandomId(10)
                    smbiosData.SetValue("SystemSerialNumber", newSerialNumber)
                    Dim logBefore = "SMBIOS SystemSerialNumber - Before: " & serialNumberBefore
                    Dim logAfter = "SMBIOS SystemSerialNumber - After: " & newSerialNumber
                    SaveLogs("smbios", logBefore, logAfter)

                    ShowNotification("SMBIOS Function executed successfully.", NotificationType.Success)
                Else
                    ShowNotification("SMBIOS data registry key not found.", NotificationType.Error)
                End If
            End Using
        Catch ex As Exception
            ShowNotification("An error occurred while executing the SMBIOS Function: " & ex.Message, NotificationType.Error)
        End Try
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    End Sub

    Private Sub Guna2GradientButton7TraceCleaner_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
        End
    End Sub
    'sechex.me
    'sechex.me
    'sechex.me
    'sechex.me
End Class
