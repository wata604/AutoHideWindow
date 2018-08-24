Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Windows.Interop

Public Class MainViewModel
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _MouseX As Integer
    Public Property MouseX As Integer
        Get
            Return _MouseX
        End Get
        Set(value As Integer)
            If value.Equals(_MouseX) Then Return
            _MouseX = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MouseX"))
        End Set
    End Property

    Private _MouseY As Integer
    Public Property MouseY As Integer
        Get
            Return _MouseY
        End Get
        Set(value As Integer)
            If value.Equals(_MouseY) Then Return
            _MouseY = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MouseY"))
        End Set
    End Property

    Private _WindowTop As Integer
    Public Property WindowTop As Integer
        Get
            Return _WindowTop
        End Get
        Set(value As Integer)
            If _WindowTop = value Then Return
            _WindowTop = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("WindowTop"))
        End Set
    End Property

    Private _WindowLeft As Integer
    Public Property WindowLeft As Integer
        Get
            Return _WindowLeft
        End Get
        Set(value As Integer)
            If _WindowLeft = value Then Return
            _WindowLeft = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("WindowLeft"))
        End Set
    End Property

    Private _WindowTopmost As Boolean
    Public Property WindowTopmost As Boolean
        Get
            Return _WindowTopmost
        End Get
        Set(value As Boolean)
            If _WindowTopmost = value Then Return
            _WindowTopmost = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("WindowTopmost"))
        End Set
    End Property

    Private _WindowVisibility As Visibility
    Public Property WindowVisibility As Visibility
        Get
            Return _WindowVisibility
        End Get
        Set(value As Visibility)
            If _WindowVisibility = value Then Return
            _WindowVisibility = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("WindowVisibility"))
        End Set
    End Property

    Private _IsContains As Boolean
    Public Property IsContains As Boolean
        Get
            Return _IsContains
        End Get
        Set(value As Boolean)
            If _IsContains = value Then Return
            _IsContains = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsContains"))
        End Set
    End Property

    Public Property WindowWidth As Integer = 525
    Public Property WindowHeight As Integer = 350

    Public Sub New()

        Dim mainWindow As Window = Nothing
        Dim dpiScaleFactor As Point = New Point(1.0, 1.0)
        Dim handle As IntPtr = Nothing

        Dim timer As Threading.DispatcherTimer = New Threading.DispatcherTimer With {
            .Interval = TimeSpan.FromSeconds(0.1)
        }
        AddHandler timer.Tick,
            Sub(sender As Object, e As EventArgs)
                Me.MouseX = Cursor.Position.X
                Me.MouseY = Cursor.Position.Y

                If mainWindow Is Nothing Then
                    Dim x = HwndSource.FromHwnd(Process.GetCurrentProcess().MainWindowHandle)
                    mainWindow = CType(x.RootVisual, Window)
                    dpiScaleFactor = mainWindow.GetDpiScaleFactor()
                    handle = New WindowInteropHelper(mainWindow).Handle
                End If

                Dim deviceLeft = Me.WindowLeft * dpiScaleFactor.X
                Dim deviceTop = Me.WindowTop * dpiScaleFactor.Y
                Dim deviceWidth = Me.WindowWidth * dpiScaleFactor.X
                Dim deviceHeight = Me.WindowHeight * dpiScaleFactor.Y

                SetWindowPos(
                    handle,
                    HWND_TOP,
                    CType(Math.Round(deviceLeft), Integer),
                    CType(Math.Round(deviceTop), Integer),
                    CType(Math.Round(deviceWidth), Integer),
                    CType(Math.Round(deviceHeight), Integer),
                    SWP_NOACTIVATE)

                If Me.MouseX >= deviceLeft AndAlso
                    Me.MouseX <= deviceLeft + deviceWidth AndAlso
                    Me.MouseY >= deviceTop AndAlso
                    Me.MouseY <= deviceTop + deviceHeight Then
                    Me.IsContains = True
                Else
                    Me.IsContains = False
                End If

                If Me.MouseY = 0 AndAlso Me.IsContains AndAlso
                    Not (Control.MouseButtons And Control.MouseButtons.Left) = MouseButtons.Left Then
                    Me.WindowTopmost = True
                    Me.WindowVisibility = Visibility.Visible
                End If

                If Me.IsContains Then Return

                If Me.WindowTop = 0 Then
                    Me.WindowTopmost = True
                    Me.WindowVisibility = Visibility.Hidden
                Else
                    Me.WindowTopmost = False
                    Me.WindowVisibility = Visibility.Visible
                End If
            End Sub
        timer.Start()
    End Sub

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SetWindowPos(
                            ByVal hWnd As IntPtr,
                            ByVal hWndInsertAfter As Integer,
                            ByVal x As Integer,
                            ByVal y As Integer,
                            ByVal cx As Integer,
                            ByVal cy As Integer,
                            ByVal uFlags As Integer) _
                            As Boolean
    End Function

    ' 最前面
    Private Const HWND_TOPMOST As Integer = -1
    ' 前面
    Private Const HWND_TOP As Integer = 0
    ' すべての最前面ウィンドウの後ろ
    Private Const HWND_NOTOPMOST As Integer = -2
    ' Z オーダーの最後
    Private Const HWND_BOTTOM As Integer = 1

    ''ウィンドウのサイズと位置の変更に関するフラグ
    ' ウィンドウを表示
    Private Const SWP_SHOWWINDOW As Integer = &H40
    ' 現在のサイズを維持（cx、cyは無視）
    Private Const SWP_NOSIZE As Integer = &H1
    ' 現在の位置を維持します（x、yは無視）
    Private Const SWP_NOMOVE As Integer = &H2
    ' ウィンドウをアクティブにしない
    Private Const SWP_NOACTIVATE As Integer = &H10
End Class
