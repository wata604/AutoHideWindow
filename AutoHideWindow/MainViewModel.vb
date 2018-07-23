Imports System.ComponentModel
Imports System.Windows.Forms

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

        Dim timer As Threading.DispatcherTimer = New Threading.DispatcherTimer With {
            .Interval = TimeSpan.FromSeconds(0.1)
        }
        AddHandler timer.Tick,
            Sub(sender As Object, e As EventArgs)
                Me.MouseX = Cursor.Position.X
                Me.MouseY = Cursor.Position.Y
                If Me.MouseX >= Me.WindowLeft AndAlso
                    Me.MouseX <= Me.WindowLeft + Me.WindowWidth AndAlso
                    Me.MouseY >= Me.WindowTop AndAlso
                    Me.MouseY <= Me.WindowTop + Me.WindowHeight Then
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

End Class
